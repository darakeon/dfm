using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using CsvHelper.Configuration;

namespace DFM.Exchange.Importer
{
	public class CSVImporter
	{
		public CSVImporter(String content)
		{
			using TextReader reader = new StringReader(content);

			IReaderConfiguration config =
				new CsvConfiguration(CultureInfo.CurrentCulture)
				{
					MissingFieldFound = null,
					HeaderValidated = null,
				};

			using var csv = new CsvReader(reader, config);

			var moves = csv.GetRecords<MoveCsv>().ToList();

			var validHeaders =
				typeof(MoveCsv)
					.GetProperties()
					.Select(p => p.Name)
					.ToList();

			var hasNonValidHeaders =
				csv.HeaderRecord
					.Any(h => !validHeaders.Contains(h));

			if (hasNonValidHeaders)
			{
				Error = ImporterError.Header;
				return;
			}

			if (moves.Count == 0)
			{
				Error = ImporterError.Empty;
				return;
			}

			MoveList = moves;
		}

		public ImporterError? Error { get; }
		public IList<MoveCsv> MoveList { get; } = new List<MoveCsv>();
	}
}
