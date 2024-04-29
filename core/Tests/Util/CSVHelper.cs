using System;
using System.Collections.Generic;
using System.Linq;
using DFM.Entities;
using TechTalk.SpecFlow;

namespace DFM.Tests.Util
{
	public static class CSVHelper
	{
		public static IEnumerable<String> ToCsv(this Table table)
		{
			return new[] { table.Header }
				.Union(table.Rows.Select(row => row.Values))
				.Select(csvLine);
		}

		private static String csvLine(ICollection<String> fields)
		{
			return String.Join(
				",",
				fields.Select(v => v.Replace("\"", "\\\""))
			);
		}

		public static String ToCsv(Line line)
		{
			var result = $"{line.Description},{line.Date.ToShortDateString()},{line.Category},{line.Nature},{line.Out},{line.In},{line.Value}";

			if (line.Conversion != null)
			{
				result += $",{line.Conversion}";
			}

			foreach (var detail in line.DetailList)
			{
				result += $",{detail.Description},{detail.Amount},{detail.Value}";

				if (detail.Conversion != null)
				{
					result += $",{detail.Conversion}";
				}
			}

			return result;
		}
	}
}
