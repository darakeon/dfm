using System;
using DFM.Generic;
using DFM.Generic.Datetime;

namespace DFM.Entities
{
	public partial class User
	{
		private void init()
		{
			Config = new Config();
			Control = new Control();
		}

		public override String ToString()
		{
			return $"[{ID}] {Email}";
		}

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
	}
}
