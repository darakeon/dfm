using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;
using DFM.Core.Enums;
using DFM.Service.Helpers;

namespace DFM.Service.Entities
{
    [DataContract]
    public class Move : Core.Entities.Move
    {
        public Move(Core.Entities.Move move)
        {
            ID = move.ID;
            Description = move.Description;
            Date = move.Date;
            Nature = move.Nature;
            base.Category = move.Category;
            base.In = move.In;
            base.Out = move.Out;
            base.DetailList = move.DetailList;
        }



        [DataMember]
        public new Int32 ID { get { return base.ID; } set { ID = value; } }
        [DataMember]
        public new String Description { get { return base.Description; } set { Description = value; } }
        [DataMember]
        public new DateTime Date { get { return base.Date; } set { Date = value; } }
        [DataMember]
        public new MoveNature Nature { get { return base.Nature; } set { Nature = value; } }
        [DataMember]
        public new Category Category { get { return base.Category.Cast(); } set { Category = value; } }
        [DataMember]
        public new Month In { get { return base.In.Cast(); } set { In = value; } }
        [DataMember]
        public new Month Out { get { return base.Out.Cast(); } set { Out = value; } }
        [DataMember]
        public new IList<Detail> DetailList { get { return base.DetailList.Cast(); } set { DetailList = value; } }
        [DataMember]
        public new Double Value { get { return base.Value; } }

        [OperationContract]
        public void AddDetail(Detail detail)
        {
            AddDetail(detail);
        }

        [OperationContract]
        public new Boolean HasRealDetails()
        {
            return HasRealDetails();
        }
    }
}