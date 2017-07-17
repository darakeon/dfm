using System;
using System.Runtime.Serialization;
using DFM.Service.Helpers;

namespace DFM.Service.Entities
{
    [DataContract]
    public class Category : Core.Entities.Category
    {
        public Category(Core.Entities.Category category)
        {
            ID = category.ID;
            Name = category.Name;
            Active = category.Active;
            base.User = category.User;
        }



		[DataMember]
		public new Int32 ID { get{ return base.ID; } set{ ID = value; } }
		[DataMember]
		public new String Name { get{ return base.Name; } set{ Name = value; } }
		[DataMember]
		public new Boolean Active { get{ return base.Active; } set{ Active = value; } }
		[DataMember]
		public new User User { get{ return base.User.Cast(); } set{ User = value; } }
    }

}
