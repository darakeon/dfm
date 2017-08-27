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
        private readonly CategoryRepository categoryService;
        private readonly AccountRepository accountService;
        private readonly YearRepository yearService;
        private readonly MonthRepository monthService;
        private readonly SummaryRepository summaryService;

        internal AdminService(ServiceAccess serviceAccess, AccountRepository accountService, CategoryRepository categoryService, YearRepository yearService, MonthRepository monthService, SummaryRepository summaryService)
            : base(serviceAccess)
        {
            this.accountService = accountService;
            this.categoryService = categoryService;
            this.yearService = yearService;
            this.monthService = monthService;
            this.summaryService = summaryService;
        }



        public Account GetAccountByUrl(String url)
        {
            VerifyUser();

            var account = accountService.GetByUrl(url, Parent.Current.User);

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
            VerifyUser();

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

                accountService.SaveOrUpdate(account);
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
            VerifyUser();

            BeginTransaction();

            try
            {
                accountService.Close(url, Parent.Current.User);
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
            VerifyUser();

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
                            summaryService.Delete(summary);
                        }

                        monthService.Delete(month);
                    }

                    foreach (var summary in year.SummaryList)
                    {
                        summaryService.Delete(summary);
                    }

                    yearService.Delete(year);
                }

                accountService.Delete(url, Parent.Current.User);

                CommitTransaction();
            }
            catch
            {
                RollbackTransaction();
                throw;
            }
        }



        public Category GetCategoryByName(String name)
        {
            VerifyUser();

            var category = categoryService.GetByName(name, Parent.Current.User);

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
            VerifyUser();

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

                categoryService.SaveOrUpdate(category);
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
            VerifyUser();

            BeginTransaction();

            try
            {
                categoryService.Disable(name, Parent.Current.User);
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
            VerifyUser();

            BeginTransaction();

            try
            {
                categoryService.Enable(name, Parent.Current.User);
                CommitTransaction();
            }
            catch
            {
                RollbackTransaction();
                throw;
            }
        }


        internal IList<Account> GetAccountsByUser(User user)
        {
            return accountService.List(a => a.User.ID == user.ID);
        }


    }
}
