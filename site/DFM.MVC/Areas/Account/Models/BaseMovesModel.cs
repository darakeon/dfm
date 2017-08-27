using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Ak.MVC.Forms;
using DFM.BusinessLogic.Exceptions;
using DFM.Entities.Bases;
using DFM.Entities.Enums;
using DFM.Entities;
using DFM.Entities.Extensions;
using DFM.Generic;
using DFM.MVC.Helpers.Controllers;
using DFM.MVC.Helpers.Global;

namespace DFM.MVC.Areas.Account.Models
{
    public abstract class BaseMovesModel : BaseAccountModel
    {
        protected BaseMovesModel(IMove iMove, OperationType type)
            : this(iMove)
        {
            Type = type;
        }

        protected BaseMovesModel(IMove iMove)
            : this()
        {
            GenericMove = iMove;

            if (Date == DateTime.MinValue)
                Date = Today;

            if (iMove.Nature == MoveNature.Transfer)
                ChosenAccountUrl = iMove.AccIn().Url;

            if (iMove.Category != null)
                CategoryName = iMove.Category.Name;

            arrangeDetails();
        }

        private BaseMovesModel()
        {
            var transferIsPossible =
                Current.User.VisibleAccountList().Count() > 1;

            populateDropDowns(transferIsPossible);
        }

        private void populateDropDowns(Boolean transferIsPossible)
        {
            if (GenericMove == null)
                GenericMove = initIMove();

            makeNatureList(transferIsPossible);

            makeCategoryList();

            makeAccountTransferList();

            arrangeDetails();
        }

        private IMove initIMove()
        {
            if (GetType() == typeof(SchedulesCreateModel))
                return new Schedule();

            return new Move();
        }

        private void makeNatureList(bool transferIsPossible)
        {
            NatureSelectList = 
                transferIsPossible
                    ? SelectListExtension.CreateSelect(MultiLanguage.GetEnumNames<MoveNature>())
                    : SelectListExtension.CreateSelect(MultiLanguage.GetEnumNames<PrimalMoveNature>());
        }

        private void makeCategoryList()
        {
            var categoryList = Current.User.VisibleCategoryList();

            CategorySelectList = SelectListExtension
                .CreateSelect(categoryList, mv => mv.Name, mv => mv.Name);
        }

        private void makeAccountTransferList()
        {
            var accountList =
                Current.User.VisibleAccountList()
                    .Where(a => a.Url != CurrentAccountUrl)
                    .ToList();

            AccountSelectList = SelectListExtension
                .CreateSelect(accountList, a => a.Url, a => a.Name);
        }

        private void arrangeDetails()
        {
            if (!GenericMove.HasDetails())
                GenericMove.AddDetail(new Detail { Amount = 1 });

            IsDetailed = GenericMove.HasDetails();
        }


        
        public OperationType Type { get; set; }


        protected internal IMove GenericMove { get; set; }


        public IList<Detail> DetailList
        {
            get { return GenericMove.DetailList; }
            set { GenericMove.DetailList = value; }
        }


        [Required(ErrorMessage = "*")]
        public String Description
        {
            get { return GenericMove.Description; } 
            set { GenericMove.Description = value; }
        }

        
        [Required(ErrorMessage = "*")]
        public DateTime Date
        {
            get { return GenericMove.Date; } 
            set { GenericMove.Date = value; }
        }


        [Required(ErrorMessage = "*")]
        public MoveNature Nature
        {
            get { return GenericMove.Nature; } 
            set { GenericMove.Nature = value; }
        }

        public SelectList NatureSelectList { get; set; }

        

        [Required(ErrorMessage = "*")]
        public SelectList CategorySelectList { get; set; }
        public String CategoryName { get; set; }

        public SelectList AccountSelectList { get; set; }
        public String ChosenAccountUrl { get; set; }


        public Boolean IsDetailed { get; set; }

        

        internal abstract void SaveOrUpdate(AccountSelector selector);


        public IList<String> CreateEditSchedule()
        {
            var errors = new List<String>();

            try
            {
                var selector = new AccountSelector(GenericMove.Nature, CurrentAccountUrl, ChosenAccountUrl);

                SaveOrUpdate(selector);
            }
            catch (DFMCoreException e)
            {
                errors.Add(MultiLanguage.Dictionary[e]);
            }

            return errors;
        }


    }
}