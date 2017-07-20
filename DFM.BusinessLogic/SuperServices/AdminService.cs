using System;
using DFM.BusinessLogic.Services;
using DFM.Entities;

namespace DFM.BusinessLogic.SuperServices
{
    public class AdminService
    {
        private readonly MoveService moveService;
        private readonly ScheduleService scheduleService;
        private readonly CategoryService categoryService;
        private readonly AccountService accountService;
        private readonly DetailService detailService;

        internal AdminService(AccountService accountService, CategoryService categoryService, ScheduleService scheduleService, MoveService moveService, DetailService detailService)
        {
            this.accountService = accountService;
            this.detailService = detailService;
            this.moveService = moveService;
            this.scheduleService = scheduleService;
            this.categoryService = categoryService;
        }


        public Category SelectCategoryById(Int32 id)
        {
            return categoryService.SelectById(id);
        }

        public void SaveOrUpdateSchedule(Schedule schedule)
        {
            scheduleService.SaveOrUpdate(schedule);
        }

        public Account SelectAccountById(Int32 id)
        {
            return accountService.SelectById(id);
        }

        public Move SelectMoveById(Int32 id)
        {
            return moveService.SelectById(id);
        }

        public void SaveOrUpdateCategory(Category category)
        {
            categoryService.SaveOrUpdate(category);
        }

        public void SaveOrUpdateAccount(Account account)
        {
            accountService.SaveOrUpdate(account);
        }

        public void DisableCategory(Category category)
        {
            categoryService.Disable(category);
        }

        public void CloseAccount(Account account)
        {
            accountService.Close(account);
        }

        public void DeleteAccount(Account account)
        {
            accountService.Delete(account);
        }

        public void EnableCategory(Category category)
        {
            categoryService.Enable(category);
        }

        public Detail SelectDetailById(Int32 id)
        {
            return detailService.SelectById(id);
        }
    }
}
