using System;
using Keon.Util.DB;

namespace DFM.Entities
{
	public partial class User : IEntityLong
	{
		public User()
		{
			init();
		}

		public virtual Int64 ID { get; set; }

		public virtual String Password { get; set; }
		public virtual String Email { get; set; }

		public virtual DateTime Creation { get; set; }
		public virtual Boolean Active { get; set; }
		public virtual Int32 RemovalWarningSent { get; set; }

		public virtual Int32 WrongLogin { get; set; }

		public virtual String TFASecret { get; set; }
		public virtual Boolean TFAPassword { get; set; }

		public virtual Config Config { get; set; }

		public virtual Boolean IsAdm { get; set; }

		public virtual Boolean IsRobot { get; set; }
		public virtual DateTime RobotCheck { get; set; }
	}
}
