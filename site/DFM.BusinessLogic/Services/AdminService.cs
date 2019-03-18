using System;
using System.Collections.Generic;
using DFM.BusinessLogic.Exceptions;
using DFM.BusinessLogic.InterfacesAndBases;
using DFM.BusinessLogic.Repositories;
using DFM.Entities;
using DFM.Entities.Enums;
using DFM.Generic;
using DFM.Multilanguage;

namespace DFM.BusinessLogic.Services
{
	public class AdminService : BaseService
	{
		private readonly AccountRepository accountRepository;
		private readonly CategoryRepository categoryRepository;
		private readonly YearRepository yearRepository;
		private readonly MonthRepository monthRepository;
		private readonly SummaryRepository summaryRepository;
		private readonly ConfigRepository configRepository;
		private readonly ScheduleRepository scheduleRepository;

		internal AdminService(ServiceAccess serviceAccess, AccountRepository accountRepository,
			CategoryRepository categoryRepository, YearRepository yearRepository, MonthRepository monthRepository,
			SummaryRepository summaryRepository, ConfigRepository configRepository, ScheduleRepository scheduleRepository)
			: base(serviceAccess)
		{
			this.accountRepository = accountRepository;
			this.categoryRepository = categoryRepository;
			this.yearRepository = yearRepository;
			this.monthRepository = monthRepository;
			this.summaryRepository = summaryRepository;
			this.configRepository = configRepository;
			this.scheduleRepository = scheduleRepository;
		}



		#region Account
		public IList<Account> GetAccountList(Boolean open)
		{
			Parent.Safe.VerifyUser();

			var query = accountRepository.NewQuery();

			query.SimpleFilter(a => a.User.ID == Parent.Current.User.ID);

			if (open)
				query.SimpleFilter(a => a.EndDate == null);
			else
				query.SimpleFilter(a => a.EndDate != null);

			return query.OrderBy(a => a.Name).Result;
		}

		public Account GetAccountByUrl(String url)
		{
			Parent.Safe.VerifyUser();
			return GetAccountByUrlInternal(url);
		}

		internal Account GetAccountByUrlInternal(String url)
		{
			var account = accountRepository.GetByUrl(url, Parent.Current.User);

			if (account == null)
				throw DFMCoreException.WithMessage(ExceptionPossibilities.InvalidAccount);

			return account;
		}

		public void CreateAccount(Account account)
		{
			saveOrUpdateAccount(account, OperationType.Creation);
		}

		public void UpdateAccount(Account account, String newUrl)
		{
			saveOrUpdateAccount(account, OperationType.Edition, newUrl);
		}

		private void saveOrUpdateAccount(Account account, OperationType opType, String newUrl = null)
		{
			Parent.Safe.VerifyUser();

			account.User = Parent.Current.User;

			InTransaction(() =>
			{
				if (opType == OperationType.Edition)
				{
					var oldAccount = GetAccountByUrlInternal(account.Url);

					account.ID = oldAccount.ID;

					if (!String.IsNullOrEmpty(newUrl))
						account.Url = newUrl;
				}

				accountRepository.SaveOrUpdate(account);
			});
		}

		public void CloseAccount(String url)
		{
			Parent.Safe.VerifyUser();

			InTransaction(() =>
			{
				var account = accountRepository.GetByUrl(url, Parent.Current.User);

				if (account != null)
				{
					scheduleRepository.DisableAll(account);
				}

				accountRepository.Close(account);
			});
		}

		public void DeleteAccount(String url)
		{
			Parent.Safe.VerifyUser();

			InTransaction(() =>
			{
				var account = GetAccountByUrlInternal(url);

				foreach (var year in account.YearList)
				{
					foreach (var month in year.MonthList)
					{
						foreach (var summary in month.SummaryList)
						{
							summaryRepository.Delete(summary);
						}

						monthRepository.Delete(month);
					}

					foreach (var summary in year.SummaryList)
					{
						summaryRepository.Delete(summary);
					}

					yearRepository.Delete(year);
				}

				scheduleRepository.DeleteAll(account);

				accountRepository.Delete(url, Parent.Current.User);
			});
		}
		#endregion Account



		#region Category
		public IList<Category> GetCategoryList(Boolean? active = null)
		{
			Parent.Safe.VerifyUser();

			var query = categoryRepository.NewQuery()
				.SimpleFilter(a => a.User.ID == Parent.Current.User.ID);

			if (active.HasValue)
			{
				query.SimpleFilter(c => c.Active == active.Value);
			}
			else
			{
				query.OrderBy(c => c.Active, false);
			}

			return query.OrderBy(a => a.Name).Result;
		}

		public Category GetCategoryByName(String name)
		{
			Parent.Safe.VerifyUser();
			return GetCategoryByNameInternal(name);
		}

		internal Category GetCategoryByNameInternal(String name)
		{
			verifyCategoriesEnabled();

			var category = categoryRepository.GetByName(name, Parent.Current.User);

			if (category == null)
				throw DFMCoreException.WithMessage(ExceptionPossibilities.InvalidCategory);

			return category;
		}

		public void CreateCategory(Category category)
		{
			saveOrUpdateCategory(category, OperationType.Creation, category.Name);
		}

		public void UpdateCategory(Category category, String newName)
		{
			saveOrUpdateCategory(category, OperationType.Edition, newName);
		}

		private void saveOrUpdateCategory(Category category, OperationType opType, String newName)
		{
			Parent.Safe.VerifyUser();
			verifyCategoriesEnabled();

			category.User = Parent.Current.User;

			InTransaction(() =>
			{
				if (opType == OperationType.Edition)
				{
					var oldCategory = GetCategoryByNameInternal(category.Name);

					category.ID = oldCategory.ID;
					category.Active = oldCategory.Active;
					category.Name = newName;
				}

				categoryRepository.SaveOrUpdate(category);
			});
		}

		public void DisableCategory(String name)
		{
			Parent.Safe.VerifyUser();
			verifyCategoriesEnabled();

			InTransaction(() =>
			{
				categoryRepository.Disable(name, Parent.Current.User);
			});
		}

		public void EnableCategory(String name)
		{
			Parent.Safe.VerifyUser();
			verifyCategoriesEnabled();

			InTransaction(() =>
			{
				categoryRepository.Enable(name, Parent.Current.User);
			});
		}

		private void verifyCategoriesEnabled()
		{
			if (!Parent.Current.User.Config.UseCategories)
				throw DFMCoreException.WithMessage(ExceptionPossibilities.CategoriesDisabled);
		}
		#endregion Category



		#region Config

		public void UpdateConfig(ConfigOptions configOptions)
		{
			Parent.Safe.VerifyUser();

			InTransaction(() =>
			{
				UpdateConfigWithinTransaction(configOptions);
			});
		}

		internal void UpdateConfigWithinTransaction(ConfigOptions configOptions)
		{
			var config = Parent.Current.User.Config;

			if (!String.IsNullOrEmpty(configOptions.Language) && !PlainText.AcceptedLanguage().Contains(configOptions.Language.ToLower()))
				throw DFMCoreException.WithMessage(ExceptionPossibilities.LanguageUnknown);

			if (!String.IsNullOrEmpty(configOptions.TimeZone) && !DateTimeGMT.TimeZoneList().ContainsKey(configOptions.TimeZone))
				throw DFMCoreException.WithMessage(ExceptionPossibilities.TimezoneUnknown);

			if (!String.IsNullOrEmpty(configOptions.Language))
				config.Language = configOptions.Language;

			if (!String.IsNullOrEmpty(configOptions.TimeZone))
				config.TimeZone = configOptions.TimeZone;

			if (configOptions.SendMoveEmail.HasValue)
				config.SendMoveEmail = configOptions.SendMoveEmail.Value;

			if (configOptions.UseCategories.HasValue)
				config.UseCategories = configOptions.UseCategories.Value;

			if (configOptions.MoveCheck.HasValue)
				config.MoveCheck = configOptions.MoveCheck.Value;

			if (configOptions.Wizard.HasValue)
				config.Wizard = configOptions.Wizard.Value;

			configRepository.Update(config);
		}

		public void EndWizard()
		{
			UpdateConfig(new ConfigOptions { Wizard = false });
		}
		#endregion Config



		#region Theme

		public void ChangeTheme(BootstrapTheme theme)
		{
			if (theme == BootstrapTheme.None)
				throw DFMCoreException.WithMessage(ExceptionPossibilities.InvalidTheme);

			var config = Parent.Current.User.Config;
			config.Theme = theme;

			InTransaction(() =>
			{
				configRepository.Update(config);
			});
		}

		#endregion Theme



	}
}
