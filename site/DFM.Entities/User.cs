using System;
using DK.Generic.DB;

namespace DFM.Entities
{
	public partial class User : IEntity
	{
		public User()
		{
			init();
		}


		public virtual Int32 ID { get; set; }

		public virtual String Password { get; set; }
		public virtual String Email { get; set; }

		public virtual DateTime Creation { get; set; }
		public virtual Boolean Active { get; set; }

		public virtual Int32 WrongLogin { get; set; }

		public virtual String TFASecret { get; set; }

		public virtual Config Config { get; set; }
	}
}
