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

		private string description { get; }
		private string amount { get; }
		private string value { get; }
		private string conversion { get; }

		public static string Convert(IEnumerable<Detail> details)
		{
			var list = details
				.Select(d => new DetailCsv(d))
				.Select(d => d.ToString());

			return string.Join(" + ", list);
		}

		public override string ToString()
		{
			var conversionText = conversion == null
				? string.Empty
				: $"({conversion})";

			var valueText = $"{amount}x{value}{conversionText}";

			return $"{description} [{valueText}]";
		}
	}
}
