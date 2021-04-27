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
		}

		public override String ToString()
		{
			return $"[{ID}] {Email}";
		}

		public virtual DateTime Now()
		{
			return TZ.Now(Config.TimeZone);
		}

		public virtual Boolean WrongPassExceeded()
		{
			return WrongLogin >= Cfg.PasswordErrorLimit;
		}

		public virtual void SetRobotCheckDay()
		{
			var firstTime = RobotCheck == DateTime.MinValue;

			var nowUser = Now();
			var nowUtc = DateTime.UtcNow;
			var diff = nowUtc - nowUser;
			RobotCheck = nowUser.Date + diff;

			if (!firstTime)
				RobotCheck = RobotCheck.AddDays(1);
		}
	}
}
