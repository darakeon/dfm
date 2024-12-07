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

			if (tip is TipLocal tipLocal)
			{
				return Type == TipType.Local
					&& ((TipLocal)Permanent & tipLocal) == tipLocal;
			}

			return true;
		}

		public virtual String LastGiven()
		{
			var all = listValids();
			return all.ContainsKey(Last)
				? all[Last]
				: null;
		}

		private static readonly Random random = new();
		public virtual String Random()
		{
			var available = listValids()
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

		private IDictionary<UInt64, String> listValids()
		{
			return Type switch
			{
				TipType.Browser => Enum.GetValues<TipBrowser>()
					.Where(n => n != TipBrowser.None)
					.ToDictionary(v => (UInt64)v, v => v.ToString()),

				TipType.Mobile => Enum.GetValues<TipMobile>()
					.Where(n => n != TipMobile.None)
					.ToDictionary(v => (UInt64)v, v => v.ToString()),

				TipType.Local => Enum.GetValues<TipLocal>()
					.Where(n => n != TipLocal.None)
					.ToDictionary(v => (UInt64)v, v => v.ToString()),

				TipType.Tests => Enum.GetValues<TipTests>()
					.Where(n => n != TipTests.None)
					.ToDictionary(v => (UInt64)v, v => v.ToString()),

				_ => new Dictionary<UInt64, String>()
			};
		}

		public virtual Boolean IsFull()
		{
			return (Permanent | Temporary) >= sum();
		}

		private UInt64 sum()
		{
			UInt64 sum = 0;

			foreach (var tip in listValids())
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
