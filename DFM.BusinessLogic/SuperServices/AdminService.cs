using System;
using DFM.BusinessLogic.Exceptions;
using DFM.BusinessLogic.Services;
using DFM.Entities;

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
            BeginTransaction();

            try
            {
                accountService.SaveOrUpdate(account);
                CommitTransaction();
            }
            catch (DFMCoreException)
            {
                RollbackTransaction();
                throw;
            }
        }

        public void CloseAccount(Int32 id)
        {
            BeginTransaction();

            try
            {
                accountService.Close(id);
                CommitTransaction();
            }
            catch (DFMCoreException)
            {
                RollbackTransaction();
                throw;
            }
        }

        public void DeleteAccount(Int32 id)
        {
            BeginTransaction();

            try
            {
                accountService.Delete(id);
                CommitTransaction();
            }
            catch (DFMCoreException)
            {
                RollbackTransaction();
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
            BeginTransaction();

            try
            {
                categoryService.SaveOrUpdate(category);
                CommitTransaction();
            }
            catch (DFMCoreException)
            {
                RollbackTransaction();
                throw;
            }
        }

        public void DisableCategory(Int32 id)
        {
            BeginTransaction();

            try
            {
                categoryService.Disable(id);
                CommitTransaction();
            }
            catch (DFMCoreException)
            {
                RollbackTransaction();
                throw;
            }
        }

        public void EnableCategory(Int32 id)
        {
            BeginTransaction();

            try
            {
                categoryService.Enable(id);
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
