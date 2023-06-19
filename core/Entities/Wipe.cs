using System;
using DFM.Entities.Bases;
using DFM.Entities.Enums;
using DFM.Generic;
using Keon.Util.Crypto;
using Keon.Util.DB;

namespace DFM.Entities
{
	public class Wipe : IEntityLong, ITalkable
	{
		public virtual Int64 ID { get; set; }

		public virtual String HashedEmail { get; set; }
		public virtual String UsernameStart { get; set; }
		public virtual String DomainStart { get; set; }

		public virtual DateTime When { get; set; }
		public virtual RemovalReason Why { get; set; }

		public virtual String Password { get; set; }
		public virtual String S3 { get; set; }

		public virtual Theme Theme { get; set; }
		public virtual String Language { get; set; }

		public override String ToString()
		{
			return $"[{ID}] {UsernameStart}...@{DomainStart}...";
		}

		public static Wipe FromUser(User user)
		{
			return new Wipe
			{
				HashedEmail = Crypt.Do(user.Email ?? ""),
				UsernameStart = user.Username?[..MaxLen.WipeUsernameStart],
				DomainStart = user.Domain?[..MaxLen.WipeDomainStart],
				When = DateTime.UtcNow,
				Password = user.Password,
				Theme = user.Settings.Theme,
				Language = user.Settings.Language,
			};
		}
	}
}
