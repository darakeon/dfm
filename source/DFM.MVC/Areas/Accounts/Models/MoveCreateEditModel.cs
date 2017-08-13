using DFM.Generic;
using DFM.Entities;
using DFM.MVC.Helpers.Controllers;
using DFM.Repositories;

namespace DFM.MVC.Areas.Accounts.Models
{
    public class MoveCreateEditModel : GenericMoveModel
    {
        public MoveCreateEditModel() 
            : base(new Move())  { }

        public MoveCreateEditModel(OperationType type) 
            : this(new Move(), type) { }

        public MoveCreateEditModel(Move move, OperationType type) 
            : base(move, type) { }


        internal override void SaveOrUpdate(AccountSelector selector)
        {
            Services.Money.SaveOrUpdateMove(Move, selector.AccountOutName, selector.AccountInName, CategoryName);
        }


        public Move Move
        {
            get { return (Move) GenericMove; }
            set { GenericMove = value; }
        }


    }

}