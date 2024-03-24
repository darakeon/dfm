using System;
using System.Linq.Expressions;
using DFM.Generic;
using DFM.Generic.Datetime;
using Keon.Util.DB;

namespace DFM.Entities
{
	public class User : IEntityLong
	{
		public User()
		{
			Settings = new Settings();
			Control = new Control();
		}

		public virtual Int64 ID { get; set; }

		public virtual String Password { get; set; }

		public virtual String Username { get; set; }
		public virtual String Domain { get; set; }

		public virtual String Email
		{
			get
			{
				if (Username == null && Domain == null)
					return null;

				return $"{Username}@{Domain}";
			}
			set
			{
				if (String.IsNullOrEmpty(value))
				{
					Username = null;
					Domain = null;
				}
				else if (value.Contains("@"))
				{
					var parts = value.Split("@", 2);
					Username = parts[0];
					Domain = parts[1];
				}
				else
				{
					Username = value;
					Domain = "";
				}
			}
		}

		public virtual String TFASecret { get; set; }
		public virtual Boolean TFAPassword { get; set; }

		public virtual Settings Settings { get; set; }
		public virtual Control Control { get; set; }

		public virtual DateTime Now()
		{
			return TZ.Now(Settings.TimeZone);
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

		public virtual Misc GenerateMisc()
		{
			return new Misc(
				Control.MiscDna,
				Settings.Theme.Colors()
			);
		}

		public override String ToString()
		{
			return $"[{ID}] {Email}";
		}

		public virtual void Deconstruct(out String username, out String domain)
		{
			username = Username;
			domain = Domain;
		}

		public static Expression<Func<User, Boolean>> Compare(String email)
		{
			var (username, domain) =
				new User {Email = email};

			return u => u.Username == username
				&& u.Domain == domain;
		}
	}
}
