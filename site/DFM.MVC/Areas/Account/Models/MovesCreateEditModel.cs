using System;
using DFM.Email;
using DFM.Generic;
using DFM.Entities;
using DFM.MVC.Helpers.Global;

namespace DFM.MVC.Areas.Account.Models
{
    public class MovesCreateEditModel : BaseMovesModel
    {
        public MovesCreateEditModel() 
            : base(new Move())  { }

        public MovesCreateEditModel(OperationType type) 
            : base(new Move(), type) { }

        public MovesCreateEditModel(Int32 id, OperationType type) 
            : base(Money.GetMoveById(id), type) { }


        internal override void SaveOrUpdate()
        {
            var result = Money.SaveOrUpdateMove(Move, AccountOutUrl, AccountInUrl, CategoryName);

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