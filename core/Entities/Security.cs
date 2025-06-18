﻿using System;
using DFM.Entities.Enums;
using Keon.Util.DB;
using token = Keon.Util.Extensions.Token;

namespace DFM.Entities
{
	public class Security : IEntityLong
	{
		public virtual Int64 ID { get; set; }

		public virtual String Token { get; set; }
		public virtual Boolean Active { get; set; }
		public virtual DateTime Expire { get; set; }
		public virtual SecurityAction Action { get; set; }
		public virtual Boolean Sent { get; set; }

		public virtual User User { get; set; }
		public virtual Wipe Wipe { get; set; }

		private static readonly Object tokenLock = new();
		public virtual void CreateToken()
		{
			lock (tokenLock)
			{
				Token = token.New();
			}
		}

		public virtual Boolean IsValid()
		{
			return Active && Expire >= DateTime.UtcNow;
		}

		public override String ToString()
		{
			return $"[{ID}] {Token}";
		}
	}
}
