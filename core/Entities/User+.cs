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
	}
}
