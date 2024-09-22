﻿using System;
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

		public static String ToCsv(Line line, Boolean addConversion)
		{
			var result =
				$"{line.Description}," +
				$"{line.Date:yyyy-MM-dd}," +
				$"{line.Category},";

			if (line.Nature != null)
				result += $"{line.Nature},";

			result +=
				$"{line.Out}," +
				$"{line.In}," +
				$"{line.Value:F2}";

			if (line.Conversion != null || addConversion)
			{
				result += $",{line.Conversion:F2}";
			}

			foreach (var detail in line.DetailList)
			{
				result += $",{detail.Description},{detail.Amount},{detail.Value:F2}";

				if (detail.Conversion != null)
				{
					result += $",{detail.Conversion:F2}";
				}
			}

			return result;
		}
	}
}
