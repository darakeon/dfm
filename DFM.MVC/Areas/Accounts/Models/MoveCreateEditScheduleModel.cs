using System;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Ak.MVC.Forms;
using DFM.Entities.Enums;
using DFM.Entities.Extensions;
using DFM.MVC.Authentication;
using DFM.Entities;
using DFM.MVC.Models;
using DFM.MVC.MultiLanguage.Helpers;

namespace DFM.MVC.Areas.Accounts.Models
{
    public abstract class MoveCreateEditScheduleModel<T> : BaseLoggedModel
        where T : Move, new()
    {
        protected MoveCreateEditScheduleModel()
        {
            var transferIsPossible = Current.User.AccountList
                                        .Where(a => a.Open())
                                        .Count() > 1;

            NatureSelectList = transferIsPossible ?
                SelectListExtension.CreateSelect(EnumHelper.GetEnumNames<MoveNature>()) :
                SelectListExtension.CreateSelect(EnumHelper.GetEnumNames<PrimalMoveNature>());

            FrequencySelectList =
                SelectListExtension.CreateSelect(EnumHelper.GetEnumNames<ScheduleFrequency>());

            CategorySelectList = SelectListExtension.CreateSelect(
                        Current.User.CategoryList.Where(c => c.Active).ToList(),
                        mv => mv.ID, mv => mv.Name
                    );

            Move = new T();
            Date = DateTime.Today;
        }

        protected MoveCreateEditScheduleModel(T move) : this()
        {
            Move = move;

            AccountID = Move.Nature == MoveNature.Transfer
                ? Move.In.Year.Account.ID : (Int32?)null;
        }


        public T Move { get; set; }

        [Required(ErrorMessage = "*")]
        public String Description { get { return Move.Description; } set { Move.Description = value; } }

        
        [Required(ErrorMessage = "*")]
        public DateTime Date { get { return Move.Date; } set { Move.Date = value; } }


        [Required(ErrorMessage = "*")]
        public MoveNature Nature { get { return Move.Nature; } set { Move.Nature = value; } }
        public SelectList NatureSelectList { get; set; }

        

        [Required(ErrorMessage = "*")]
        public Int32? CategoryID { get; set; }
        public SelectList CategorySelectList { get; set; }


        public Int32? AccountID { get; set; }
        public SelectList AccountSelectList { get; set; }


        public Boolean IsDetailed { get; set; }


        public Boolean IsSchedule { get; set; }



        private Schedule schedule
        {
            get
            {
                return !IsSchedule
                    ? new Schedule()
                    : Move.Schedule
                        ?? (Move.Schedule = new Schedule());
            }
        }

        [Required]
        public Boolean Boundless { get { return schedule.Boundless; } set { schedule.Boundless = value; } }
        [Required]
        public Int16 Times { get { return schedule.Times; } set { schedule.Times = value; } }
        [Required]
        public ScheduleFrequency Frequency { get { return schedule.Frequency; } set { schedule.Frequency = value; } }

        public SelectList FrequencySelectList { get; set; }



        public void MakeAccountTransferList(Int32 accountIdToExclude)
        {
            var accountList = 
                Current.User.AccountList
                    .Where(a => a.Open() && a.ID != accountIdToExclude)
                    .ToList();

            AccountSelectList = SelectListExtension
                .CreateSelect(accountList, a => a.ID, a => a.Name);
        }


        public void Populate(Int32 accountID)
        {
            MakeAccountTransferList(accountID);


            IsDetailed = Move.IsDetailed();

            IsSchedule = Move is FutureMove;

            if (!Move.DetailList.Any())
            {
                var detail = new Detail { Amount = 1 };
                Move.AddDetail(detail);
            }

            if (Move.Category != null)
            {
                CategoryID = Move.Category.ID;
            }
        }

    }
}