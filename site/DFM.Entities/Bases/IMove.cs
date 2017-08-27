using System;
using System.Collections.Generic;
using Ak.Generic.DB;
using DFM.Entities.Enums;

namespace DFM.Entities.Bases
{
    public interface IMove : IEntity
    {
        String Description { get; set; }
        DateTime Date { get; set; }
        MoveNature Nature { get; set; }
        Category Category { get; set; }
        IList<Detail> DetailList { get; set; }

        void AddDetail(Detail detail);
        Double Value();
        User User { get; }
        Account AccIn();
        Account AccOut();

    }

}
