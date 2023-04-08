using System;
using System.Collections.Generic;
using System.Linq;
using DFM.BusinessLogic.Exceptions;
using DFM.BusinessLogic.Repositories;
using DFM.BusinessLogic.Response;
using DFM.Entities;
using DFM.Entities.Enums;
using DFM.Generic;
using DFM.Generic.Datetime;
using DFM.Language;

namespace DFM.BusinessLogic.Services
{
	public class AdminService : Service
	{
		internal AdminService(ServiceAccess serviceAccess, Repos repos)
			: base(serviceAccess, repos) { }

		#region Account
		public IList<AccountListItem> GetAccountList(Boolean open)
		{
			parent.Safe.VerifyUser();

			var user = parent.Safe.GetCurrent();

			return repos.Account.Get(user, open)
				.Select(makeAccountListItem)
				.ToList();
		}

		private AccountListItem makeAccountListItem(Account account)
		{
			var hasMoves = repos.Move.AccountHasMoves(account);
			var total = repos.Summary.GetTotal(account);

			var item = AccountListItem.Convert(account, total, hasMoves);

			return item;
		}

		public AccountInfo GetAccount(String url)
		{
			parent.Safe.VerifyUser();
			var account = GetAccountEntity(url);
			return AccountInfo.Convert(account);
		}

		internal Account GetAccountEntity(String url)
		{
			var account = getAccount(url);

			if (account == null)
				throw Error.InvalidAccount.Throw();

			return account;
		}

		private Account getAccount(String url)
		{
			var user = parent.Safe.GetCurrent();
			return repos.Account.GetByUrl(url, user);
		}

		public void CreateAccount(AccountInfo info)
		{
			var account = new Account
			{
				User = parent.Safe.GetCurrent(),
				Open = true,
			};

			saveAccount(info, account);
		}

		public void UpdateAccount(AccountInfo info)
		{
			var account = GetAccountEntity(info.OriginalUrl);
			saveAccount(info, account);
		}

		private void saveAccount(AccountInfo info, Account account)
		{
			parent.Safe.VerifyUser();

			inTransaction("SaveAccount", () =>
			{
				info.Update(account);
				repos.Account.Save(account);
			});
		}

		public void CloseAccount(String url)
		{
			parent.Safe.VerifyUser();

			inTransaction("CloseAccount", () =>
			{
				var account = GetAccountEntity(url);

				var hasMoves = repos.Move.AccountHasMoves(account);

				if (!hasMoves)
					throw Error.CantCloseEmptyAccount.Throw();

				repos.Schedule.DisableAll(account);

				repos.Account.Close(account);
			});
		}

		public void DeleteAccount(String url)
		{
			parent.Safe.VerifyUser();

			inTransaction("DeleteAccount", () =>
			{
				var account = GetAccountEntity(url);

				var hasMoves = repos.Move.AccountHasMoves(account);

				if (hasMoves)
					throw Error.CantDeleteAccountWithMoves.Throw();

				repos.Summary.DeleteAll(account);

				repos.Schedule.DeleteAll(account);

				repos.Account.Delete(account);
			});
		}

		public void ReopenAccount(String url)
		{
			parent.Safe.VerifyUser();

			inTransaction("ReopenAccount", () =>
			{
				var account = GetAccountEntity(url);
				repos.Account.Reopen(account);
			});

		}
		#endregion Account

		#region Category
		public IList<CategoryListItem> GetCategoryList(Boolean? active = null)
		{
			parent.Safe.VerifyUser();

			var user = parent.Safe.GetCurrent();
			var categories = repos.Category.Get(user, active);

			return categories
				.Select(CategoryListItem.Convert)
				.ToList();
		}

		public CategoryInfo GetCategory(String name)
		{
			parent.Safe.VerifyUser();
			return CategoryInfo.Convert(
				GetCategoryEntity(name)
			);
		}

		internal Category GetCategoryEntity(String name)
		{
			var category = getCategory(name);

			if (category == null)
				throw Error.InvalidCategory.Throw();

			return category;
		}

		private Category getCategory(String name)
		{
			verifyCategoriesEnabled();

			var user = parent.Safe.GetCurrent();
			return repos.Category.GetByName(name, user);
		}

		public void CreateCategory(CategoryInfo info)
		{
			var category = new Category
			{
				User = parent.Safe.GetCurrent()
			};

			saveCategory(info, category);
		}

		public void UpdateCategory(CategoryInfo info)
		{
			parent.Safe.VerifyUser();

			var category = GetCategoryEntity(info.OriginalName);

			saveCategory(info, category);
		}

		private void saveCategory(CategoryInfo info, Category category)
		{
			parent.Safe.VerifyUser();
			verifyCategoriesEnabled();

			inTransaction("SaveCategory", () =>
			{
				info.Update(category);
				repos.Category.Save(category);
			});
		}

		public void DisableCategory(String name)
		{
			parent.Safe.VerifyUser();
			verifyCategoriesEnabled();

			var category = GetCategoryEntity(name);

			inTransaction("DisableCategory", () =>
			{
				repos.Category.Disable(category);
			});
		}

		public void EnableCategory(String name)
		{
			parent.Safe.VerifyUser();
			verifyCategoriesEnabled();

			var category = GetCategoryEntity(name);

			inTransaction("EnableCategory", () =>
			{
				repos.Category.Enable(category);
			});
		}

		private void verifyCategoriesEnabled()
		{
			if (!parent.Current.UseCategories)
				throw Error.CategoriesDisabled.Throw();
		}

		public void UnifyCategory(String categoryToKeep, String categoryToDelete)
		{
			parent.Safe.VerifyUser();
			verifyCategoriesEnabled();

			if (categoryToKeep == categoryToDelete)
				throw Error.CannotMergeSameCategory.Throw();

			var keep = GetCategoryEntity(categoryToKeep);
			var delete = GetCategoryEntity(categoryToDelete);

			if (!keep.Active)
				throw Error.DisabledCategory.Throw();

			inTransaction("UnifyCategory_MoveChildren", () =>
			{
				var movesFromDeleted = repos.Move.ByCategory(delete);

				foreach (var move in movesFromDeleted)
				{
					move.Category = keep;

					if (move.DetailList.Any())
						move.Value = 0;

					parent.BaseMove.SaveMove(move, OperationType.Edition);
				}

				var schedulesFromDeleted = repos.Schedule.ByCategory(delete);

				foreach (var schedule in schedulesFromDeleted)
				{
					repos.Schedule.UpdateCategory(schedule, keep);
				}
			});

			var user = parent.Safe.GetCurrent();
			parent.BaseMove.FixSummaries(user);

			var summaries = repos.Summary.ByCategory(delete);

			foreach (var summary in summaries)
			{
				if (summary.Out > 0 || summary.In > 0)
					throw Error.CategoryUnifyFail.Throw();
			}

			inTransaction("UnifyCategory_Delete", () =>
			{
				summaries.ToList()
					.ForEach(repos.Summary.Delete);

				repos.Category.Delete(delete);
			});
		}
		#endregion Category

		#region Settings

		public void UpdateSettings(SettingsInfo info)
		{
			parent.Safe.VerifyUser();

			inTransaction("UpdateSettings", () =>
			{
				var user = parent.Safe.GetCurrent();
				updateSettings(info, user.Settings);
			});
		}

		private void updateSettings(SettingsInfo info, Settings settings)
		{
			if (info.Language != null && !PlainText.AcceptLanguage(info.Language))
				throw Error.LanguageUnknown.Throw();

			if (info.TimeZone != null && !TZ.IsValid(info.TimeZone))
				throw Error.TimeZoneUnknown.Throw();

			if (!String.IsNullOrEmpty(info.Language))
				settings.Language = info.Language;

			if (!String.IsNullOrEmpty(info.TimeZone))
				settings.TimeZone = info.TimeZone;

			if (info.UseCategories.HasValue)
				settings.UseCategories = info.UseCategories.Value;

			if (info.UseAccountsSigns.HasValue)
				settings.UseAccountsSigns = info.UseAccountsSigns.Value;

			if (info.MoveCheck.HasValue)
				settings.MoveCheck = info.MoveCheck.Value;

			if (info.SendMoveEmail.HasValue)
				settings.SendMoveEmail = info.SendMoveEmail.Value;

			if (info.Wizard.HasValue)
				settings.Wizard = info.Wizard.Value;

			repos.Settings.Update(settings);
		}

		public void EndWizard()
		{
			parent.Safe.VerifyUser();

			inTransaction("EndWizard", () =>
			{
				var settings = parent.Safe.GetCurrent().Settings;
				settings.Wizard = false;
				repos.Settings.Update(settings);
			});
		}

		public void UnsubscribeMoveMail(String token)
		{
			inTransaction("UnsubscribeMoveMail", () =>
			{
				var security = repos.Security.ValidateAndGet(
					token, SecurityAction.UnsubscribeMoveMail
				);

				var user = security.User;
				if (!parent.Safe.IsLastContractAccepted(user))
					throw Error.NotSignedLastContract.Throw();

				var settings = user.Settings;
				settings.SendMoveEmail = false;
				repos.Settings.Update(settings);

				repos.Security.Disable(token);
			});
		}
		#endregion Settings

		#region Theme
		public void ChangeTheme(Theme theme)
		{
			parent.Safe.VerifyUser();

			if (theme == Theme.None)
				throw Error.InvalidTheme.Throw();

			var user = parent.Safe.GetCurrent();

			var settings = user.Settings;
			settings.Theme = theme;

			inTransaction("ChangeTheme", () =>
			{
				repos.Settings.Update(settings);
			});
		}
		#endregion Theme
	}
}
