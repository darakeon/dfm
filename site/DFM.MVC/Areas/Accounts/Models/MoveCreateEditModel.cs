using System;
using DFM.Email;
using DFM.Generic;
using DFM.Entities;
using DFM.MVC.Helpers.Controllers;
using DFM.MVC.Helpers.Global;

namespace DFM.MVC.Areas.Accounts.Models
{
    public class MoveCreateEditModel : BaseMoveModel
    {
        public MoveCreateEditModel() 
            : base(new Move())  { }

        public MoveCreateEditModel(OperationType type) 
            : base(new Move(), type) { }

        public MoveCreateEditModel(Int32 id, OperationType type) 
            : base(Money.GetMoveById(id), type) { }


        internal override void SaveOrUpdate(AccountSelector selector)
        {
            var result = Money.SaveOrUpdateMove(Move, selector.AccountOutUrl, selector.AccountInUrl, CategoryName);

            if (result.Error.IsWrong())
            {
                var message = MultiLanguage.Dictionary["MoveSave"];
                var error = MultiLanguage.Dictionary[result.Error].ToLower();
                var final = String.Format(message, error);

                ErrorAlert.AddTranslated(final);
            }
        }


        public Move Move
        {
            get { return (Move) GenericMove; }
            set { GenericMove = value; }
        }


    }

}