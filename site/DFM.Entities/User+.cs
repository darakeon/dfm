using System;
using System.Collections.Generic;
using System.Linq;
using DFM.Entities.Enums;
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
		
		
		public virtual Boolean IsAdm()
		{
			return Email == "{admin-email}";
		}

		public virtual DateTime Now()
		{
			return DateTimeGMT.Now(Config.TimeZone);
		}

		public virtual Boolean WrongPassExceeded()
		{
			return WrongLogin >= Cfg.PasswordErrorLimit;
		}



	}
}
