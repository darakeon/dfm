using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using DFM.Entities;

namespace DFM.Exchange
{
	public class CSV
	{
		/*
		 * | Description | Date | Category | Nature | In | Out | Value | Details |
		 */

		private readonly List<MoveCsv> moves = new();
		private readonly List<Schedule> schedules = new();

		public void Add(IEnumerable<Move> movesDB)
		{
			foreach (var move in movesDB)
			{
				if (moves.Any(m => m.Equals(move)))
					continue;

				moves.Add(MoveCsv.Convert(move));
			}
		}

		public void Add(IEnumerable<Schedule> schedulesDB)
		{
			foreach (var schedule in schedulesDB)
			{
				if (moves.Any(m => m.Equals(schedule)))
					continue;

				schedules.Add(schedule);
			}
		}

		public void Create(User user)
		{
			if (!moves.Any() && !schedules.Any())
				return;

			var path = user.Email.Replace("@", "_") + ".csv";

			using TextWriter writer = new StreamWriter(path);

			var csv = new CsvWriter(writer, CultureInfo.CurrentCulture);

			if (schedules.Any())
			{
				var maxDate = schedules
					.Where(s => !s.Boundless)
					.Max(s => s.LastDateShouldRun());

				moves.AddRange(
					schedules.SelectMany(
						s => MoveCsv.Convert(s, maxDate)
					)
				);
			}

			csv.WriteRecords(moves.OrderBy(s => s.Date));
		}
	}
}
