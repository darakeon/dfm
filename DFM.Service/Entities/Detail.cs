using System;
using System.Runtime.Serialization;
using DFM.Service.Helpers;

namespace DFM.Service.Entities
{
    [DataContract]
    public class Detail : Core.Entities.Detail
    {
        public Detail(Core.Entities.Detail detail)
        {
            ID = detail.ID;
            Amount = detail.Amount;
            Description = detail.Description;
            base.Move = detail.Move;
        }



        [DataMember]
		public new Int32 ID { get{ return base.ID; } set{ ID = value; } }
		[DataMember]
		public new String Description { get{ return base.Description; } set{ Description = value; } }
		[DataMember]
		public new Int32 Amount { get{ return base.Amount; } set{ Amount = value; } }
		[DataMember]
		public new Double Value { get{ return base.Value; } set{ Value = value; } }
		[DataMember]
        public new Move Move { get { return base.Move.Cast(); } set { Move = value; } }
    }
}
