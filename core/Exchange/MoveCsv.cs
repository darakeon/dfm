using System;
using System.Collections.Generic;
using DFM.Entities;
using DFM.Entities.Bases;

namespace DFM.Exchange
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

		public String Description { get; }
		public String Date { get; }
		public String Category { get; }
		public String Nature { get; }
		public String In { get; }
		public String Out { get; }
		public String Value { get; }
		public String Conversion { get; }
		public String Details { get; }

		public Boolean Equals(IMove other)
		{
			return guid == other.Guid;
		}
	}
}
