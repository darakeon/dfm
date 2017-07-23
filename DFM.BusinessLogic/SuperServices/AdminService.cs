using System;
using DFM.BusinessLogic.Exceptions;
using DFM.BusinessLogic.Services;
using DFM.Entities;

namespace DFM.BusinessLogic.SuperServices
{
    public class AdminService
    {
        private readonly CategoryService categoryService;
        private readonly AccountService accountService;

        internal AdminService(AccountService accountService, CategoryService categoryService)
        {
            this.accountService = accountService;
            this.categoryService = categoryService;
        }



        public Account SelectAccountById(Int32 id)
        {
            var account = accountService.SelectById(id);

            if (account == null)
                throw DFMCoreException.WithMessage(ExceptionPossibilities.InvalidAccount);

            return account;
        }

        public Account SelectAccountByName(String name, User user)
        {
            var account = accountService.SelectByName(name, user);

            if (account == null)
                throw DFMCoreException.WithMessage(ExceptionPossibilities.InvalidAccount);

            return account;
        }

        public void SaveOrUpdateAccount(Account account)
        {
            accountService.BeginTransaction();

            try
            {
                accountService.SaveOrUpdate(account);
                accountService.CommitTransaction();
            }
            catch (DFMCoreException)
            {
                accountService.RollbackTransaction();
                throw;
            }
        }

        public void CloseAccount(Int32 id)
        {
            accountService.BeginTransaction();

            try
            {
                accountService.Close(id);
                accountService.CommitTransaction();
            }
            catch (DFMCoreException)
            {
                accountService.RollbackTransaction();
                throw;
            }
        }

        public void DeleteAccount(Int32 id)
        {
            accountService.BeginTransaction();

            try
            {
                accountService.Delete(id);
                accountService.CommitTransaction();
            }
            catch (DFMCoreException)
            {
                accountService.RollbackTransaction();
                throw;
            }
        }



        public Category SelectCategoryById(Int32 id)
        {
            var category = categoryService.SelectById(id);

            if (category == null)
                throw DFMCoreException.WithMessage(ExceptionPossibilities.InvalidCategory);

            return category;
        }

        public Category SelectCategoryByName(String name, User user)
        {
            var category = categoryService.SelectByName(name, user);

            if (category == null)
                throw DFMCoreException.WithMessage(ExceptionPossibilities.InvalidCategory);

            return category;
        }

        public void SaveOrUpdateCategory(Category category)
        {
            categoryService.BeginTransaction();

            try
            {
                categoryService.SaveOrUpdate(category);
                categoryService.CommitTransaction();
            }
            catch (DFMCoreException)
            {
                categoryService.RollbackTransaction();
                throw;
            }
        }

        public void DisableCategory(Int32 id)
        {
            categoryService.BeginTransaction();

            try
            {
                categoryService.Disable(id);
                categoryService.CommitTransaction();
            }
            catch (DFMCoreException)
            {
                categoryService.RollbackTransaction();
                throw;
            }
        }

        public void EnableCategory(Int32 id)
        {
            categoryService.BeginTransaction();

            try
            {
                categoryService.Enable(id);
                categoryService.CommitTransaction();
            }
            catch (DFMCoreException)
            {
                categoryService.RollbackTransaction();
                throw;
            }
        }



    }
}
