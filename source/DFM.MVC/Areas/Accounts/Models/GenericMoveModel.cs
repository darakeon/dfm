using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Ak.MVC.Forms;
using DFM.Entities.Bases;
using DFM.Entities.Enums;
using DFM.Entities.Extensions;
using DFM.Entities;
using DFM.Generic;
using DFM.MVC.Helpers;
using DFM.MVC.Helpers.Controllers;
using DFM.MVC.Models;

namespace DFM.MVC.Areas.Accounts.Models
{
    public abstract class GenericMoveModel : BaseLoggedModel
    {
        protected GenericMoveModel(IMove iMove, OperationType type)
            : this(iMove)
        {
            Type = type;
        }

        protected GenericMoveModel(IMove iMove)
            : this()
        {
            GenericMove = iMove;

            Date = DateTime.Today;

            AccountName = iMove.Nature == MoveNature.Transfer
                ? iMove.AccIn().Name : null;

            if (iMove.Category != null)
                CategoryName = iMove.Category.Name;

        }

        private GenericMoveModel()
        {
            var transferIsPossible =
                Current.User.AccountList
                    .Where(a => a.IsOpen())
                    .Count() > 1;

            populateDropDowns(transferIsPossible);
        }

        private void populateDropDowns(Boolean transferIsPossible)
        {
            NatureSelectList = transferIsPossible ?
                SelectListExtension.CreateSelect(MultiLanguage.GetEnumNames<MoveNature>()) :
                SelectListExtension.CreateSelect(MultiLanguage.GetEnumNames<PrimalMoveNature>());

            var categoryList = Current.User.CategoryList.Where(c => c.Active).ToList();

            CategorySelectList = SelectListExtension.CreateSelect(
                    categoryList, mv => mv.Name, mv => mv.Name
                );
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
        public String AccountName { get; set; }


        public Boolean IsDetailed { get; set; }





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

            IsDetailed = GenericMove.HasDetails() && GenericMove.IsDetailed();

            if (!GenericMove.DetailList.Any())
                GenericMove.AddDetail(new Detail { Amount = 1 });
        }



        internal abstract void SaveOrUpdate(AccountSelector selector);



    }
}