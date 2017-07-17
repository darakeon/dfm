using System;
using System.Runtime.Serialization;
using DFM.Core.Enums;
using DFM.Service.Helpers;

namespace DFM.Service.Entities
{
    [DataContract]
    public class Summary : Core.Entities.Summary
    {
        public Summary(Core.Entities.Summary summary)
        {
            ID = summary.ID;
            Value = summary.Value;
            IsValid = summary.IsValid;
            Nature = summary.Nature;
            base.Category = summary.Category;
            base.Month = summary.Month;
            base.Year = summary.Year;
        }



        [DataMember]
		public new Int32 ID { get{ return base.ID; } set{ ID = value; } }
		[DataMember]
		public new Double Value { get{ return base.Value; } set{ Value = value; } }
		[DataMember]
		public new Boolean IsValid { get{ return base.IsValid; } set{ IsValid = value; } }
		[DataMember]
		public new SummaryNature Nature { get{ return base.Nature; } set{ Nature = value; } }
		[DataMember]
        public new Category Category { get { return base.Category.Cast(); } set { Category = value; } }
		[DataMember]
        public new Month Month { get { return base.Month.Cast(); } set { Month = value; } }
		[DataMember]
        public new Year Year { get { return base.Year.Cast(); } set { Year = value; } }

    }
}
