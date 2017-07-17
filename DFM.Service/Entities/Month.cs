using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;
using DFM.Service.Helpers;

namespace DFM.Service.Entities
{
    [DataContract]
    public class Month : Core.Entities.Month
    {
        public Month(Core.Entities.Month month)
        {
            ID = month.ID;
            base.InList = month.InList;
            base.OutList = month.OutList;
            base.SummaryList = month.SummaryList;
            Time = month.Time;
            base.Year = month.Year;
        }


        [DataMember]
		public new Int32 ID { get{ return base.ID; } set{ ID = value; } }
		[DataMember]
		public new Int32 Time { get{ return base.Time; } set{ Time = value; } }
		[DataMember]
        public new Year Year { get { return base.Year.Cast(); } set { Year = value; } }
		[DataMember]
        public new IList<Summary> SummaryList { get { return base.SummaryList.Cast(); } set { SummaryList = value; } }
		[DataMember]
        public new IList<Move> InList { get { return base.InList.Cast(); } set { InList = value; } }
		[DataMember]
        public new IList<Move> OutList { get { return base.OutList.Cast(); } set { OutList = value; } }
		[DataMember]
		public new Double Value { get{ return base.Value; } }
        
        [OperationContract]
		public Double CheckUp(Category category)
        {
            return CheckUp(category);
        }

        [OperationContract]
		public void AjustSummaryList(Summary summary)
        {
            AjustSummaryList(summary);
        }

    }

}
