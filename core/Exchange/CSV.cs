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
				if (schedules.Any(s => s.Guid == schedule.Guid))
					continue;

				schedules.Add(schedule);
			}
		}

		public void Create(User user)
		{
			if (!moves.Any() && !schedules.Any())
				return;

			if (schedules.Any())
				addSchedules();

			if (moves.Any())
				write(user);
		}

		private void addSchedules()
		{
			var bounded = schedules
				.Where(s => !s.Boundless)
				.ToList();

			if (bounded.Any())
			{
				var maxDate = bounded.Max(
					s => s.LastDateShouldRun()
				);

				var movesFromSchedules = schedules
					.SelectMany(
						s => MoveCsv.Convert(s, maxDate)
					);

				moves.AddRange(movesFromSchedules);
			}


			schedules.Clear();
		}

		private void write(User user)
		{
			var path = user.Email.Replace("@", "_") + ".csv";
			using var writer = new StreamWriter(path);
			using var csv = new CsvWriter(writer, CultureInfo.CurrentCulture);
			csv.WriteRecords(moves.OrderBy(s => s.Date));
		}
	}
}
