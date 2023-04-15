using System;
using System.Collections.Generic;
using System.Linq;
using DFM.Entities.Enums;
using Keon.Util.DB;

namespace DFM.Entities
{
	public class Tips : IEntityLong
	{
		public virtual Int64 ID { get; set; }

		public virtual TipType Type { get; set; }
		public virtual UInt64 Temporary { get; set; }
		public virtual UInt64 Permanent { get; set; }
		public virtual UInt64 Last { get; set; }

		public virtual Int16 Countdown { get; set; }
		public virtual Int16 Repeat { get; set; }

		public virtual User User { get; set; }

		public virtual Boolean IsPermanent<T>(T tip)
			where T : struct
		{
			if (tip is TipTests tipTests)
			{
				return Type == TipType.Tests
				    && ((TipTests)Permanent & tipTests) == tipTests;
			}

			if (tip is TipBrowser tipBrowser)
			{
				return Type == TipType.Browser
					&& ((TipBrowser)Permanent & tipBrowser) == tipBrowser;
			}

			if (tip is TipMobile tipMobile)
			{
				return Type == TipType.Mobile
					&& ((TipMobile)Permanent & tipMobile) == tipMobile;
			}

			return false;
		}

		public virtual String LastGiven()
		{
			return Last == 0 ? null : all()[Last];
		}

		private static readonly Random random = new();
		public virtual String Random()
		{
			var available = all()
				.Where(n => n.Key != 0)
				.Where(n => (n.Key & Permanent) == 0)
				.Where(n => (n.Key & Temporary) == 0)
				.ToList();

			if (available.Count == 0)
				return null;

			var index = random.Next(available.Count);
			var chosen = available[index];

			Last = chosen.Key;
			Temporary += chosen.Key;

			return chosen.Value;
		}

		private IDictionary<UInt64, String> all()
		{
			return Type switch
			{
				TipType.Tests => Enum.GetValues<TipTests>().ToDictionary(v => (UInt64)v, v => v.ToString()),
				TipType.Browser => Enum.GetValues<TipBrowser>().ToDictionary(v => (UInt64)v, v => v.ToString()),
				TipType.Mobile => Enum.GetValues<TipMobile>().ToDictionary(v => (UInt64)v, v => v.ToString()),
				_ => throw new ArgumentOutOfRangeException()
			};
		}

		public virtual Boolean IsFull()
		{
			return Permanent + Temporary == sum();
		}

		private UInt64 sum()
		{
			UInt64 sum = 0;

			foreach (var tip in all())
			{
				sum += tip.Key;
			}

			return sum;
		}

		public override String ToString()
		{
			return $"[{ID}] [{Type}]";
		}
	}
}
