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



        //public Account SelectAccountById(Int32 id)
        //{
        //    VerifyUser();

        //    var account = accountService.SelectById(id);

        //    if (account == null)
        //        throw DFMCoreException.WithMessage(ExceptionPossibilities.InvalidAccount);

        //    return account;
        //}

        public Account SelectAccountByName(String name)
        {
            VerifyUser();

            var account = accountService.SelectByName(name, Parent.Current.User);

            if (account == null)
                throw DFMCoreException.WithMessage(ExceptionPossibilities.InvalidAccount);

            return account;
        }

        public void SaveOrUpdateAccount(Account account)
        {
            VerifyUser();

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



        //public Category SelectCategoryById(Int32 id)
        //{
        //    VerifyUser();

        //    var category = categoryService.SelectById(id);

        //    if (category == null)
        //        throw DFMCoreException.WithMessage(ExceptionPossibilities.InvalidCategory);

        //    return category;
        //}

        public Category SelectCategoryByName(String name)
        {
            VerifyUser();

            var category = categoryService.SelectByName(name, Parent.Current.User);

            if (category == null)
                throw DFMCoreException.WithMessage(ExceptionPossibilities.InvalidCategory);

            return category;
        }

        public void SaveOrUpdateCategory(Category category)
        {
            VerifyUser();

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
