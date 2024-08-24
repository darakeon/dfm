using System;
using System.Collections.Generic;
using System.Linq;
using DFM.Entities;

namespace DFM.Exchange.Exporter
{
	class DetailCsv
	{
		private DetailCsv(Detail detail)
		{
			description = detail.Description;
			amount = detail.Amount.ToString();
			value = detail.Value.ToCsv();
			conversion = detail.Conversion.ToCsv();
		}

		private String description { get; }
		private String amount { get; }
		private String value { get; }
		private String conversion { get; }

		public static String Convert(IEnumerable<Detail> details)
		{
			var list = details
				.Select(d => new DetailCsv(d))
				.Select(d => d.ToString());

			return String.Join(" + ", list);
		}

		public override String ToString()
		{
			var conversionText = conversion == null
				? String.Empty
				: $"({conversion})";

			var valueText = $"{amount}x{value}{conversionText}";

			return $"{description} [{valueText}]";
		}
	}
}
