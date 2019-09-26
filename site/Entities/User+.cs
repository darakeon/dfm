using System;
using DFM.Generic;

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
			return Config.TimeZone.Now();
		}

		public virtual Boolean WrongPassExceeded()
		{
			return WrongLogin >= Cfg.PasswordErrorLimit;
		}
	}
}
