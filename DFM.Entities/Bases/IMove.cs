using System;
using System.Collections.Generic;
using DFM.Entities.Enums;

namespace DFM.Entities.Bases
{
    public interface IMove<T> : IEntity
    {
        //T In { get; set; }
        //T Out { get; set; }
        String Description { get; set; }
        DateTime Date { get; set; }
        MoveNature Nature { get; set; }
        Category Category { get; set; }
        IList<Detail> DetailList { get; set; }

        Double Value();
        Account AccIn();
        Account AccOut();
    }

}
