using System;
using DFM.Generic.Datetime;
using Keon.Util.DB;

namespace DFM.Entities
{
	public class User : IEntityLong
	{
		public User()
		{
			Config = new Config();
			Control = new Control();
		}

		public virtual Int64 ID { get; set; }

		public virtual String Password { get; set; }
		public virtual String Email { get; set; }

		public virtual String TFASecret { get; set; }
		public virtual Boolean TFAPassword { get; set; }

		public virtual Config Config { get; set; }
		public virtual Control Control { get; set; }

		public virtual DateTime Now()
		{
			return TZ.Now(Config.TimeZone);
		}

		public virtual void SetRobotCheckDay()
		{
			var firstTime = Control.RobotCheck == DateTime.MinValue;

			var nowUser = Now();
			var nowUtc = DateTime.UtcNow;
			var diff = nowUtc - nowUser;
			Control.RobotCheck = nowUser.Date + diff;

			if (!firstTime)
				Control.RobotCheck = Control.RobotCheck.AddDays(1);
		}

		public override String ToString()
		{
			return $"[{ID}] {Email}";
		}
	}
}
