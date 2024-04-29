using System;
using System.Collections.Generic;
using DFM.Entities;
using DFM.Entities.Bases;

namespace DFM.Exchange.Exporter
{
	class MoveCsv
	{
		private MoveCsv(IMove move)
		{
			guid = move.Guid;

			Description = move.Description;
			Date = move.GetDate().ToCsv();
			Category = move.Category?.Name;
			Nature = move.Nature.ToString();
			In = move.In?.Name;
			Out = move.Out?.Name;
			Value = move.Value.ToCsv();
			Conversion = move.Conversion.ToCsv();

			Details = DetailCsv.Convert(move.DetailList);
		}

		public static MoveCsv Convert(Move move)
		{
			return new(move);
		}

		public static IEnumerable<MoveCsv> Convert(Schedule schedule, DateTime? maxDate)
		{
			while (schedule.CanRun(maxDate))
			{
				yield return Convert(schedule);
			}
		}

		public static MoveCsv Convert(Schedule schedule)
		{
			return new(schedule.CreateMove());
		}

		private Guid guid { get; }

		public string Description { get; }
		public string Date { get; }
		public string Category { get; }
		public string Nature { get; }
		public string In { get; }
		public string Out { get; }
		public string Value { get; }
		public string Conversion { get; }
		public string Details { get; }

		public bool Equals(IMove other)
		{
			return guid == other.Guid;
		}
	}
}
