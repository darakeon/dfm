using System;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Ak.MVC.Forms;
using DFM.Entities.Bases;
using DFM.Entities.Enums;
using DFM.Entities.Extensions;
using DFM.Entities;
using DFM.MVC.Helpers;
using DFM.MVC.Models;

namespace DFM.MVC.Areas.Accounts.Models
{
    public class MoveCreateEditScheduleModel : BaseLoggedModel
    {
        public MoveCreateEditScheduleModel()
        {
            var transferIsPossible =
                Current.User.AccountList
                    .Where(a => a.IsOpen())
                    .Count() > 1;

            populateDropDowns(transferIsPossible);

            Move = new BaseMove();
            Date = DateTime.Today;
        }

        private void populateDropDowns(Boolean transferIsPossible)
        {
            NatureSelectList = transferIsPossible ?
                SelectListExtension.CreateSelect(MultiLanguage.GetEnumNames<MoveNature>()) :
                SelectListExtension.CreateSelect(MultiLanguage.GetEnumNames<PrimalMoveNature>());

            FrequencySelectList =
                SelectListExtension.CreateSelect(MultiLanguage.GetEnumNames<ScheduleFrequency>());

            var categoryList = Current.User.CategoryList.Where(c => c.Active).ToList();

            CategorySelectList = SelectListExtension.CreateSelect(
                    categoryList, mv => mv.Name, mv => mv.Name
                );
        }

        public MoveCreateEditScheduleModel(BaseMove baseMove)
            : this()
        {
            Move = baseMove;

            AccountName = Move.Nature == MoveNature.Transfer
                ? Move.AccIn().Name : null;

            CategoryName = Move.Category.Name;
        }


        public BaseMove Move { get; set; }

        [Required(ErrorMessage = "*")]
        public String Description { get { return Move.Description; } set { Move.Description = value; } }

        
        [Required(ErrorMessage = "*")]
        public DateTime Date { get { return Move.Date; } set { Move.Date = value; } }


        [Required(ErrorMessage = "*")]
        public MoveNature Nature { get { return Move.Nature; } set { Move.Nature = value; } }
        public SelectList NatureSelectList { get; set; }

        

        [Required(ErrorMessage = "*")]
        public SelectList CategorySelectList { get; set; }
        public String CategoryName { get; set; }


        public SelectList AccountSelectList { get; set; }
        public String AccountName { get; set; }


        public Boolean IsDetailed { get; set; }


        public Boolean IsSchedule { get; set; }


        public Schedule Schedule { get; set; }
        private Schedule schedule { get { return Schedule ?? new Schedule(); } }

        [Required] public ScheduleFrequency Frequency { get { return schedule.Frequency; } set { schedule.Frequency = value; } }
        [Required] public Boolean Boundless { get { return schedule.Boundless; } set { schedule.Boundless = value; } }
        [Required] public Int16 Times { get { return schedule.Times; } set { schedule.Times = value; } }
        public Boolean ShowInstallment { get { return schedule.ShowInstallment; } set { schedule.ShowInstallment = value; } }

        public SelectList FrequencySelectList { get; set; }



        public void MakeAccountTransferList(String accountNameToExclude)
        {
            var accountList = 
                Current.User.AccountList
                    .Where(a => a.IsOpen() && a.Name != accountNameToExclude)
                    .ToList();

            AccountSelectList = SelectListExtension
                .CreateSelect(accountList, a => a.Name, a => a.Name);
        }


        public void PopulateExcludingAccount(String accountName)
        {
            MakeAccountTransferList(accountName);

            IsDetailed = Move.HasDetails() && Move.IsDetailed();

            IsSchedule = Move is FutureMove || IsSchedule;

            if (!Move.DetailList.Any())
                Move.AddDetail(new Detail { Amount = 1 });
        }


    }
}