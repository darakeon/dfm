using System;
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
            return accountService.SelectById(id);
        }

        public void SaveOrUpdateAccount(Account account)
        {
            accountService.SaveOrUpdate(account);
        }

        public void CloseAccount(Account account)
        {
            accountService.Close(account);
        }

        public void DeleteAccount(Account account)
        {
            accountService.Delete(account);
        }


        
        public Category SelectCategoryById(Int32 id)
        {
            return categoryService.SelectById(id);
        }

        public void SaveOrUpdateCategory(Category category)
        {
            categoryService.SaveOrUpdate(category);
        }

        public void DisableCategory(Category category)
        {
            categoryService.Disable(category);
        }

        public void EnableCategory(Category category)
        {
            categoryService.Enable(category);
        }

    }
}
