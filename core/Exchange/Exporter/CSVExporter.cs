using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using DFM.Entities;
using DFM.Generic;
using DFM.Generic.Datetime;

namespace DFM.Exchange.Exporter
{
	public class CSVExporter : IDisposable
	{
		private readonly List<MoveCsv> moves = new();
		private readonly List<Schedule> schedules = new();

		public String Path { get; private set; }

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

		public void Create(Wipe wipe)
		{
			if (!moves.Any() && !schedules.Any())
				return;

			if (schedules.Any())
				addSchedules();

			if (moves.Any())
				write(wipe);
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

		private void write(Wipe wipe)
		{
			var hashedEmail = wipe.HashedEmail.ToBase64();
			var now = DateTime.UtcNow.UntilSecond();
			Path = $"{hashedEmail}_{now}.csv";

			write();
		}

		private void write()
		{
			using var writer = new StreamWriter(Path);

			using var csv = new CsvWriter(writer, CultureInfo.CurrentCulture);
			csv.WriteRecords(moves.OrderBy(s => s.Date));
		}

		public void Create(Order order)
		{
			if (!moves.Any())
				return;

			var now = DateTime.UtcNow.UntilSecond();
			Path = $"{order.Guid}_{now}.csv";

			write();
		}

		public void Dispose()
		{
			try { File.Delete(Path); }
			catch { /* ignored */ }
		}
	}
}
