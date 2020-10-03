using System;
using System.Collections.Generic;
using System.Linq;
using DFM.BusinessLogic.Exceptions;
using DFM.BusinessLogic.Repositories;
using DFM.BusinessLogic.Response;
using DFM.Entities;
using DFM.Entities.Enums;
using DFM.Generic;
using DFM.Language;

namespace DFM.BusinessLogic.Services
{
	public class AdminService : BaseService
	{
		private readonly AccountRepository accountRepository;
		private readonly CategoryRepository categoryRepository;
		private readonly SummaryRepository summaryRepository;
		private readonly ConfigRepository configRepository;
		private readonly ScheduleRepository scheduleRepository;
		private readonly MoveRepository moveRepository;

		internal AdminService(
			ServiceAccess serviceAccess, AccountRepository accountRepository,
			CategoryRepository categoryRepository, SummaryRepository summaryRepository,
			ConfigRepository configRepository, ScheduleRepository scheduleRepository, MoveRepository moveRepository)
			: base(serviceAccess)
		{
			this.accountRepository = accountRepository;
			this.categoryRepository = categoryRepository;
			this.summaryRepository = summaryRepository;
			this.configRepository = configRepository;
			this.scheduleRepository = scheduleRepository;
			this.moveRepository = moveRepository;
		}

		#region Account
		public IList<AccountListItem> GetAccountList(Boolean open)
		{
			parent.Safe.VerifyUser();

			var user = parent.Safe.GetCurrent();

			return accountRepository.NewQuery()
				.SimpleFilter(a => a.User.ID == user.ID)
				.SimpleFilter(a => a.Open == open)
				.OrderBy(a => a.Name)
				.Result
				.Select(makeAccountListItem)
				.ToList();
		}

		private AccountListItem makeAccountListItem(Account account)
		{
			var hasMoves = moveRepository.AccountHasMoves(account);
			var total = summaryRepository.GetTotal(account);
			var sign = getSign(account, total);

			var item = AccountListItem.Convert(account, total, sign, hasMoves);

			return item;
		}

		private AccountSign getSign(Account account, Decimal total)
		{
			var hasRed = account.RedLimit != null;
			var hasYellow = account.YellowLimit != null;

			if (hasRed && total < account.RedLimit)
				return AccountSign.Red;

			if (hasYellow && total < account.YellowLimit)
				return AccountSign.Yellow;

			if (hasRed || hasYellow)
				return AccountSign.Green;

			return AccountSign.None;
		}

		public AccountInfo GetAccount(String url)
		{
			parent.Safe.VerifyUser();
			var account = GetAccountByUrlInternal(url);
			return AccountInfo.Convert(account);
		}

		internal Account GetAccountByUrlInternal(String url)
		{
			var account = getAccountByUrl(url);

			if (account == null)
				throw Error.InvalidAccount.Throw();

			return account;
		}

		private Account getAccountByUrl(String url)
		{
			var user = parent.Safe.GetCurrent();
			return accountRepository.GetByUrl(url, user);
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
			var account = GetAccountByUrlInternal(info.OriginalUrl);
			saveAccount(info, account);
		}

		private void saveAccount(AccountInfo info, Account account)
		{
			parent.Safe.VerifyUser();

			inTransaction("SaveAccount", () =>
			{
				info.Update(account);
				accountRepository.Save(account);
			});
		}

		public void CloseAccount(String url)
		{
			parent.Safe.VerifyUser();

			inTransaction("CloseAccount", () =>
			{
				var account = GetAccountByUrlInternal(url);

				var hasMoves = moveRepository.AccountHasMoves(account);

				if (!hasMoves)
					throw Error.CantCloseEmptyAccount.Throw();

				scheduleRepository.DisableAll(account);

				accountRepository.Close(account);
			});
		}

		public void DeleteAccount(String url)
		{
			parent.Safe.VerifyUser();

			inTransaction("DeleteAccount", () =>
			{
				var account = GetAccountByUrlInternal(url);

				var hasMoves = moveRepository.AccountHasMoves(account);

				if (hasMoves)
					throw Error.CantDeleteAccountWithMoves.Throw();

				summaryRepository.DeleteAll(account);

				scheduleRepository.DeleteAll(account);

				accountRepository.Delete(account);
			});
		}

		public void ReopenAccount(String url)
		{
			parent.Safe.VerifyUser();

			inTransaction("ReopenAccount", () =>
			{
				var account = GetAccountByUrlInternal(url);
				accountRepository.Reopen(account);
			});

		}
		#endregion Account



		#region Category
		public IList<CategoryListItem> GetCategoryList(Boolean? active = null)
		{
			parent.Safe.VerifyUser();

			var user = parent.Safe.GetCurrent();
			var query = categoryRepository.NewQuery()
				.SimpleFilter(a => a.User.ID == user.ID);

			if (active.HasValue)
			{
				query.SimpleFilter(c => c.Active == active.Value);
			}
			else
			{
				query.OrderBy(c => c.Active, false);
			}

			return query.OrderBy(a => a.Name).Result
				.Select(CategoryListItem.Convert).ToList();
		}

		public CategoryInfo GetCategory(String name)
		{
			parent.Safe.VerifyUser();
			return CategoryInfo.Convert(
				GetCategoryByNameInternal(name)
			);
		}

		internal Category GetCategoryByNameInternal(String name)
		{
			var category = getCategoryByName(name);

			if (category == null)
				throw Error.InvalidCategory.Throw();

			return category;
		}

		private Category getCategoryByName(String name)
		{
			verifyCategoriesEnabled();

			var user = parent.Safe.GetCurrent();
			return categoryRepository.GetByName(name, user);
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
			var category = GetCategoryByNameInternal(info.OriginalName);

			saveCategory(info, category);
		}

		private void saveCategory(CategoryInfo info, Category category)
		{
			parent.Safe.VerifyUser();
			verifyCategoriesEnabled();

			inTransaction("SaveCategory", () =>
			{
				info.Update(category);
				categoryRepository.Save(category);
			});
		}

		public void DisableCategory(String name)
		{
			parent.Safe.VerifyUser();
			verifyCategoriesEnabled();

			var category = GetCategoryByNameInternal(name);

			inTransaction("DisableCategory", () =>
			{
				categoryRepository.Disable(category);
			});
		}

		public void EnableCategory(String name)
		{
			parent.Safe.VerifyUser();
			verifyCategoriesEnabled();

			var category = GetCategoryByNameInternal(name);

			inTransaction("EnableCategory", () =>
			{
				categoryRepository.Enable(category);
			});
		}

		private void verifyCategoriesEnabled()
		{
			if (!parent.Current.UseCategories)
				throw Error.CategoriesDisabled.Throw();
		}
		#endregion Category



		#region Config

		public void UpdateConfig(ConfigInfo configInfo)
		{
			parent.Safe.VerifyUser();

			inTransaction("UpdateConfig", () =>
			{
				UpdateConfigWithinTransaction(configInfo);
			});
		}

		internal void UpdateConfigWithinTransaction(ConfigInfo info)
		{
			var user = parent.Safe.GetCurrent();
			var config = user.Config;

			if (info.Language != null && !PlainText.AcceptLanguage(info.Language))
				throw Error.LanguageUnknown.Throw();

			if (info.TimeZone != null && !info.TimeZone.IsTimeZone())
				throw Error.TimeZoneUnknown.Throw();

			if (!String.IsNullOrEmpty(info.Language))
				config.Language = info.Language;

			if (!String.IsNullOrEmpty(info.TimeZone))
				config.TimeZone = info.TimeZone;

			if (info.SendMoveEmail.HasValue)
				config.SendMoveEmail = info.SendMoveEmail.Value;

			if (info.UseCategories.HasValue)
				config.UseCategories = info.UseCategories.Value;

			if (info.MoveCheck.HasValue)
				config.MoveCheck = info.MoveCheck.Value;

			if (info.Wizard.HasValue)
				config.Wizard = info.Wizard.Value;

			configRepository.Update(config);
		}

		public void EndWizard()
		{
			UpdateConfig(new ConfigInfo { Wizard = false });
		}
		#endregion Config



		#region Theme
		public void ChangeTheme(BootstrapTheme theme)
		{
			if (theme == BootstrapTheme.None)
				throw Error.InvalidTheme.Throw();

			var user = parent.Safe.GetCurrent();
			var config = user.Config;
			config.Theme = theme;

			inTransaction("ChangeTheme", () =>
			{
				configRepository.Update(config);
			});
		}
		#endregion Theme
	}
}
