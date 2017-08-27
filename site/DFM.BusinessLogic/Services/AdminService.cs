using System;
using System.Collections.Generic;
using DFM.BusinessLogic.Exceptions;
using DFM.BusinessLogic.Repositories;
using DFM.Entities;
using DFM.Generic;

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

        internal AdminService(ServiceAccess serviceAccess, AccountRepository accountRepository, CategoryRepository categoryRepository, YearRepository yearRepository, MonthRepository monthRepository, SummaryRepository summaryRepository, ConfigRepository configRepository, ScheduleRepository scheduleRepository)
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
        public Account GetAccountByUrl(String url)
        {
            Parent.Safe.VerifyUser();

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
            saveOrUpdateAccount(account, OperationType.Edit, newUrl);
        }

        private void saveOrUpdateAccount(Account account, OperationType opType, String newUrl = null)
        {
            Parent.Safe.VerifyUser();

            account.User = Parent.Current.User;

            BeginTransaction();

            try
            {
                if (opType == OperationType.Edit)
                {
                    var oldAccount = GetAccountByUrl(account.Url);

                    account.ID = oldAccount.ID;

                    if (!String.IsNullOrEmpty(newUrl))
                        account.Url = newUrl;
                }

                accountRepository.SaveOrUpdate(account);
                CommitTransaction();
            }
            catch
            {
                RollbackTransaction();
                throw;
            }
        }

        public void CloseAccount(String url)
        {
            Parent.Safe.VerifyUser();

            BeginTransaction();

            try
            {
                var account = accountRepository.GetByUrl(url, Parent.Current.User);

                if (account != null)
                {
                    scheduleRepository.DisableAll(account);
                }

                accountRepository.Close(account);
                CommitTransaction();
            }
            catch
            {
                RollbackTransaction();
                throw;
            }
        }

        public void DeleteAccount(String url)
        {
            Parent.Safe.VerifyUser();

            BeginTransaction();

            try
            {
                var account = GetAccountByUrl(url);

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

                CommitTransaction();
            }
            catch
            {
                RollbackTransaction();
                throw;
            }
        }

        internal IList<Account> GetAccounts()
        {
            return accountRepository.List(a => a.User.ID == Parent.Current.User.ID);
        }
        #endregion



        #region Category
        public Category GetCategoryByName(String name)
        {
            Parent.Safe.VerifyUser();
            verifyCategoriesEnabled();

            var category = categoryRepository.GetByName(name, Parent.Current.User);

            if (category == null)
                throw DFMCoreException.WithMessage(ExceptionPossibilities.InvalidCategory);

            return category;
        }

        public void CreateCategory(Category category)
        {
            saveOrUpdateCategory(category, OperationType.Creation);
        }

        public void UpdateCategory(Category category, String newName)
        {
            saveOrUpdateCategory(category, OperationType.Edit, newName);
        }

        private void saveOrUpdateCategory(Category category, OperationType opType, String newName = null)
        {
            Parent.Safe.VerifyUser();
            verifyCategoriesEnabled();

            category.User = Parent.Current.User;

            BeginTransaction();

            try
            {
                if (opType == OperationType.Edit)
                {
                    var oldCategory = GetCategoryByName(category.Name);

                    category.ID = oldCategory.ID;
                    category.Active = oldCategory.Active;

                    if (!String.IsNullOrEmpty(newName))
                        category.Name = newName;
                }

                categoryRepository.SaveOrUpdate(category);
                CommitTransaction();
            }
            catch
            {
                RollbackTransaction();
                throw;
            }
        }

        public void DisableCategory(String name)
        {
            Parent.Safe.VerifyUser();
            verifyCategoriesEnabled();

            BeginTransaction();

            try
            {
                categoryRepository.Disable(name, Parent.Current.User);
                CommitTransaction();
            }
            catch
            {
                RollbackTransaction();
                throw;
            }
        }

        public void EnableCategory(String name)
        {
            Parent.Safe.VerifyUser();
            verifyCategoriesEnabled();

            BeginTransaction();

            try
            {
                categoryRepository.Enable(name, Parent.Current.User);
                CommitTransaction();
            }
            catch
            {
                RollbackTransaction();
                throw;
            }
        }

        private void verifyCategoriesEnabled()
        {
            if (!Parent.Current.User.Config.UseCategories)
                throw DFMCoreException.WithMessage(ExceptionPossibilities.CategoriesDisabled);
        }
        #endregion



        #region Config
        public void UpdateConfig(String language, String timeZone, Boolean? sendMoveEmail, Boolean? useCategories)
        {
            Parent.Safe.VerifyUser();

            BeginTransaction();

            try
            {
                UpdateConfigWithinTransaction(language, timeZone, sendMoveEmail, useCategories);
                CommitTransaction();
            }
            catch
            {
                RollbackTransaction();
                throw;
            }
        }

        internal void UpdateConfigWithinTransaction(String language, String timeZone, Boolean? sendMoveEmail, Boolean? useCategories)
        {
            var config = Parent.Current.User.Config;

            if (!String.IsNullOrEmpty(language))
                config.Language = language;

            if (!String.IsNullOrEmpty(timeZone))
                config.TimeZone = timeZone;

            if (sendMoveEmail.HasValue)
                config.SendMoveEmail = sendMoveEmail.Value;

            if (useCategories.HasValue)
                config.UseCategories = useCategories.Value;

            configRepository.Update(config);
        }
        #endregion


    }
}
