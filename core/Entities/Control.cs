using System;
using DFM.Generic;
using Keon.Util.DB;

namespace DFM.Entities
{
	public class Control : IEntityLong
	{
		public Control()
		{
			Active = false;
			Creation = DateTime.UtcNow;
		}

		public virtual Int64 ID { get; set; }

		public virtual DateTime Creation { get; set; }
		public virtual DateTime? LastAccess { get; set; }

		public virtual Boolean Active { get; set; }

		public virtual Boolean IsAdm { get; set; }
		public virtual Boolean IsRobot { get; set; }

		public virtual Int32 WrongLogin { get; set; }

		public virtual Int32 RemovalWarningSent { get; set; }
		public virtual Boolean ProcessingDeletion { get; set; }
		public virtual DateTime? WipeRequest { get; set; }

		public virtual DateTime RobotCheck { get; set; }

		public virtual Int32 MiscDna { get; set; }

		public virtual Boolean WrongPassExceeded()
		{
			return WrongLogin >= Cfg.PasswordErrorLimit;
		}

		public virtual DateTime LastInteraction()
		{
			return LastAccess ?? Creation;
		}

		public static DateTime AllowedPeriod =>
			DateTime.UtcNow.AddDays(-7);

		public virtual Boolean ActiveOrAllowedPeriod()
		{
			return Active || Creation >= AllowedPeriod;
		}
	}
}
