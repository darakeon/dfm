using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Ak.MVC.Forms;
using DFM.BusinessLogic.Exceptions;
using DFM.Entities.Bases;
using DFM.Entities.Enums;
using DFM.Entities;
using DFM.Entities.Extensions;
using DFM.Generic;
using DFM.MVC.Helpers.Global;
using account = DFM.Entities.Account;

namespace DFM.MVC.Areas.Account.Models
{
    public abstract class BaseMovesModel : BaseAccountModel
    {
	    private readonly IList<account> accountList;


		private BaseMovesModel()
		{
			accountList = Current.User.VisibleAccountList();

			AccountOutUrl = CurrentAccountUrl;
			AccountInUrl = CurrentAccountUrl;

			var transferIsPossible = accountList.Count > 1;

			populateDropDowns(transferIsPossible);
		}

		protected BaseMovesModel(IMove iMove)
            : this()
        {
            GenericMove = iMove;

            if (Date == DateTime.MinValue)
                Date = Today;

	        AccountOutUrl = iMove.AccOut()?.Url;
	        AccountInUrl = iMove.AccIn()?.Url;

            if (iMove.Category != null)
                CategoryName = iMove.Category.Name;

            arrangeDetails();
        }

		protected BaseMovesModel(IMove iMove, OperationType type)
			: this(iMove)
		{
			Type = type;
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

            AccountOutSelectList = SelectListExtension.CreateSelect(accountList, a => a.Url, a => a.Name);
            AccountInSelectList = SelectListExtension.CreateSelect(accountList, a => a.Url, a => a.Name);
        }

        private void arrangeDetails()
        {
	        IsDetailed = GenericMove.ID != 0 && GenericMove.IsDetailed();
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

        public SelectList AccountOutSelectList { get; set; }
		public String AccountOutUrl { get; set; }

		public SelectList AccountInSelectList { get; set; }
        public String AccountInUrl { get; set; }


        public Boolean IsDetailed { get; set; }


        public Decimal? Value
		{
			get { return GenericMove.Value; }
			set { GenericMove.Value = value; }
		}


        internal abstract void SaveOrUpdate();


        public IList<String> CreateEditSchedule()
        {
            var errors = new List<String>();

            try
            {
                SaveOrUpdate();
            }
            catch (DFMCoreException e)
            {
                errors.Add(MultiLanguage.Dictionary[e]);
            }

            return errors;
        }


    }
}