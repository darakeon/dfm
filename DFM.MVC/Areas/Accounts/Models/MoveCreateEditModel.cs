using System;
using System.Linq;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Ak.MVC.Forms;
using DFM.MVC.Authentication;
using DFM.Core.Entities;
using DFM.Core.Enums;
using DFM.MVC.Models;

namespace DFM.MVC.Areas.Accounts.Models
{
    public class MoveCreateEditModel : BaseLoggedModel
    {
        public MoveCreateEditModel()
        {
            var transferIsPossible = Current.User.AccountList.Count > 1;

            NatureSelectList = transferIsPossible ?
                SelectListExtension.CreateSelect<MoveNature>() :
                SelectListExtension.CreateSelect<PrimalMoveNature>();

            Move = new Move();
        }




        public Move Move { get; set; }

        [Required(ErrorMessage = "Mandatory Field")]
        public String Description
        {
            get { return Move.Description; }
            set { Move.Description = value; }
        }



        [Required(ErrorMessage = "Mandatory Field")]
        public DateTime Date
        {
            get { return Move.Date; }
            set { Move.Date = value; }
        }


        [Required(ErrorMessage = "Mandatory Field")]
        public MoveNature Nature
        {
            get { return Move.Nature; }
            set { Move.Nature = value; }
        }
        public SelectList NatureSelectList { get; set; }
        

        public virtual Double? Value
        {
            get
            {
                return Move.DetailList.Any()
                    ? Move.DetailList.First().Value
                    : (Double?)null;
            }
            set
            {
                if (!Move.HasRealDetails() && value.HasValue)
                {
                    Move.MakePseudoDetail(value.Value);
                }
            }
        }

        

        [DisplayName("Category"), Required(ErrorMessage = "Mandatory Field")]
        public Int32? CategoryID { get; set; }
        public SelectList CategorySelectList { get; set; }


        [DisplayName("Account")]
        public Int32? AccountID { get; set; }
        public SelectList AccountSelectList { get; set; }


        public Boolean IsDetailed { get; set; }

        public String Title { get; set; }


        public void MakeAccountTransferList(int accountIdToExclude)
        {
            var accountList = 
                Current.User.AccountList
                    .Where(a => a.ID != accountIdToExclude)
                    .ToList();

            AccountSelectList = SelectListExtension
                .CreateSelect(accountList, a => a.ID, a => a.Name);

        }
    }
}