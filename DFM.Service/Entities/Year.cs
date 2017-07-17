using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;
using DFM.Service.Helpers;

namespace DFM.Service.Entities
{
    [DataContract]
    public class Year : Core.Entities.Year
    {
        public Year(Core.Entities.Year year)
        {
            ID = year.ID;
            Time = year.Time;
            base.Account = year.Account;
            base.MonthList = year.MonthList;
            base.SummaryList = year.SummaryList;
        }

        [DataMember]
		public new Int32 ID { get{ return base.ID; } set{ ID = value; } }
		[DataMember]
		public new Int32 Time { get{ return base.Time; } set{ Time = value; } }
		[DataMember]
        public new Account Account { get { return base.Account.Cast(); } set { Account = value; } }
		[DataMember]
        public new IList<Month> MonthList { get { return base.MonthList.Cast(); } set { MonthList = value; } }
		[DataMember]
        public new IList<Summary> SummaryList { get { return base.SummaryList.Cast(); } set { SummaryList = value; } }

        [OperationContract]
		public Double CheckUp(Category category)
        {
            return base.CheckUp(category);
        }

        [OperationContract]
		public void AjustSummaryList(Summary summary)
        {
            base.AjustSummaryList(summary);
        }

    }

}
