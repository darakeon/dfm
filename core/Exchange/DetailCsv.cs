using System;
using System.Collections.Generic;
using System.Linq;
using DFM.Entities;

namespace DFM.Exchange
{
	class DetailCsv
	{
		private DetailCsv(Detail detail)
		{
			description = detail.Description;
			amount = detail.Amount.ToString();
			value = detail.Value.ToString("F2");
		}

		private String description { get; }
		private String amount { get; }
		private String value { get; }

		public static String Convert(IEnumerable<Detail> details)
		{
			var list = details
				.Select(d => new DetailCsv(d))
				.Select(d => d.ToString());

			return String.Join(" + ", list);
		}

		public override string ToString()
		{
			return $"{description} ({amount}x{value})";
		}
	}
}
