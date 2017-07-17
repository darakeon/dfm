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
using DFM.MVC.MultiLanguage;

namespace DFM.MVC.Areas.Accounts.Models
{
    public class MoveCreateEditModel : BaseLoggedModel
    {
        public MoveCreateEditModel()
        {
            var transferIsPossible = Current.User.AccountList
                                        .Where(a => a.Open)
                                        .Count() > 1;

            NatureSelectList = transferIsPossible ?
                SelectListExtension.CreateSelect(PlainText.GetEnumNames<MoveNature>()) :
                SelectListExtension.CreateSelect(PlainText.GetEnumNames<PrimalMoveNature>());

            Move = new Move();
        }

        public MoveCreateEditModel(Move move) : this()
        {
            Move = move;

            AccountID = Move.Nature == MoveNature.Transfer
                ? Move.In.ID : (Int32?)null;
        }


        public Move Move { get; set; }

        [Required(ErrorMessage = "Mandatory Field")]
        public String Description { get { return Move.Description; } set { Move.Description = value; } }

        
        [Required(ErrorMessage = "Mandatory Field")]
        public DateTime Date { get { return Move.Date; } set { Move.Date = value; } }


        [Required(ErrorMessage = "Mandatory Field")]
        public MoveNature Nature { get { return Move.Nature; } set { Move.Nature = value; } }
        public SelectList NatureSelectList { get; set; }

        

        [Required(ErrorMessage = "Mandatory Field")]
        public Int32? CategoryID { get; set; }
        public SelectList CategorySelectList { get; set; }


        public Int32? AccountID { get; set; }
        public SelectList AccountSelectList { get; set; }


        public Boolean IsDetailed { get; set; }



        public void MakeAccountTransferList(Int32 accountIdToExclude)
        {
            var accountList = 
                Current.User.AccountList
                    .Where(a => a.Open && a.ID != accountIdToExclude)
                    .ToList();

            AccountSelectList = SelectListExtension
                .CreateSelect(accountList, a => a.ID, a => a.Name);

        }


        public void PlaceAccountsInMove(Account currentAccount, Account otherAccount)
        {
            switch (Move.Nature)
            {
                case MoveNature.Out:
                    Move.Out = currentAccount;
                    break;
                case MoveNature.In:
                    Move.In = currentAccount;
                    break;
                case MoveNature.Transfer:
                    Move.Out = currentAccount;
                    Move.In = otherAccount;
                    break;
                default:
                    throw new Exception("Move Nature doesn't exist");
            }
        }

        public void Populate(Int32 accountID)
        {
            MakeAccountTransferList(accountID);


            IsDetailed = Move.HasRealDetails();

            if (!Move.DetailList.Any())
            {
                var detail = new Detail { Amount = 1 };
                Move.AddDetail(detail);
            }

            if (Move.Category != null)
            {
                CategoryID = Move.Category.ID;
            }



            CategorySelectList = SelectListExtension.CreateSelect(
                        Current.User.CategoryList.Where(c => c.Active).ToList(),
                        mv => mv.ID, mv => mv.Name
                    );
        }

    }
}