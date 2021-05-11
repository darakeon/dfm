using System;
using System.Collections.Generic;
using System.Linq;
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
	}
}
