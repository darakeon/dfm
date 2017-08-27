using System;
using DFM.Generic;
using DFM.Entities;
using DFM.MVC.Helpers.Controllers;

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
            Money.SaveOrUpdateMove(Move, selector.AccountOutName, selector.AccountInName, CategoryName);
        }


        public Move Move
        {
            get { return (Move) GenericMove; }
            set { GenericMove = value; }
        }


    }

}