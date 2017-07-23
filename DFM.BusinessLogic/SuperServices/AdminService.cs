using System;
using DFM.BusinessLogic.Exceptions;
using DFM.BusinessLogic.Services;
using DFM.Entities;
using DFM.Generic;

namespace DFM.BusinessLogic.SuperServices
{
    public class AdminService : BaseSuperService
    {
        private readonly CategoryService categoryService;
        private readonly AccountService accountService;

        internal AdminService(ServiceAccess serviceAccess, AccountService accountService, CategoryService categoryService)
            : base(serviceAccess)
        {
            this.accountService = accountService;
            this.categoryService = categoryService;
        }



        public Account SelectAccountByName(String name)
        {
            VerifyUser();

            var account = accountService.SelectByName(name, Parent.Current.User);

            if (account == null)
                throw DFMCoreException.WithMessage(ExceptionPossibilities.InvalidAccount);

            return account;
        }



        public void CreateAccount(Account account)
        {
            saveOrUpdateAccount(account, OperationType.Creation);
        }

        public void UpdateAccount(Account account, String newName)
        {
            saveOrUpdateAccount(account, OperationType.Update, newName);
        }

        private void saveOrUpdateAccount(Account account, OperationType opType, String newName = null)
        {
            VerifyUser();

            BeginTransaction();

            try
            {
                if (opType == OperationType.Update)
                {
                    var oldAccount = SelectAccountByName(account.Name);

                    account.ID = oldAccount.ID;
                    // TODO: use Current User
                    account.User = oldAccount.User;

                    if (!String.IsNullOrEmpty(newName))
                        account.Name = newName;
                }

                accountService.SaveOrUpdate(account);
                CommitTransaction();
            }
            catch (DFMCoreException)
            {
                RollbackTransaction();
                throw;
            }
        }

        public void CloseAccount(String name)
        {
            VerifyUser();

            BeginTransaction();

            try
            {
                accountService.Close(name, Parent.Current.User);
                CommitTransaction();
            }
            catch (DFMCoreException)
            {
                RollbackTransaction();
                throw;
            }
        }

        public void DeleteAccount(String name)
        {
            VerifyUser();

            BeginTransaction();

            try
            {
                accountService.Delete(name, Parent.Current.User);
                CommitTransaction();
            }
            catch (DFMCoreException)
            {
                RollbackTransaction();
                throw;
            }
        }



        public Category SelectCategoryByName(String name)
        {
            VerifyUser();

            var category = categoryService.SelectByName(name, Parent.Current.User);

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
            saveOrUpdateCategory(category, OperationType.Update, newName);
        }

        private void saveOrUpdateCategory(Category category, OperationType opType, String newName = null)
        {
            VerifyUser();

            BeginTransaction();

            try
            {
                if (opType == OperationType.Update)
                {
                    var oldCategory = SelectCategoryByName(category.Name);

                    category.ID = oldCategory.ID;
                    // TODO: use Current User
                    category.User = oldCategory.User;

                    if (!String.IsNullOrEmpty(newName))
                        category.Name = newName;
                }

                categoryService.SaveOrUpdate(category);
                CommitTransaction();
            }
            catch (DFMCoreException)
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
            catch (DFMCoreException)
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
            catch (DFMCoreException)
            {
                RollbackTransaction();
                throw;
            }
        }



    }
}
