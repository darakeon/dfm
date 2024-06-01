using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

namespace DFM.Exchange.Importer
{
	public class CSVImporter
	{
		public CSVImporter(String content)
		{
			MoveList = new List<MoveCsv>();
			ErrorList = new Dictionary<Int16, ImporterError>();

			using TextReader reader = new StringReader(content);

			var config = new CsvConfiguration(CultureInfo.CurrentCulture)
			{
				MissingFieldFound = null,
				HeaderValidated = null,
			};

			using var csv = new CsvReader(reader, config);

			Int16 line = 0;

			while (csv.Read())
			{
				try
				{
					line++;

					var move = csv.GetRecord<MoveCsv>();

					if (move != null)
					{
						move.Position = line;
						MoveList.Add(move);
					}
				}
				catch (TypeConverterException exception)
				{
					ErrorList.Add(line, handleFieldError(exception));
				}
			}

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
				ErrorList.Add(0, ImporterError.Header);
			}

			if (!MoveList.Any() && !ErrorList.Any())
			{
				ErrorList.Add(0, ImporterError.Empty);
			}
		}

		private ImporterError handleFieldError(TypeConverterException exception)
		{
			var name = exception.MemberMapData.Member.Name;
			var value = exception.Text;

			switch (name)
			{
				case "Date":
					return value == ""
						? ImporterError.DateRequired
						: ImporterError.DateInvalid;

				case "Nature":
					return value == ""
						? ImporterError.NatureRequired
						: ImporterError.NatureInvalid;

				case "Value":
					return ImporterError.ValueInvalid;

				default:

					if (name.StartsWith("Value"))
						return value == ""
							? ImporterError.DetailValueRequired
							: ImporterError.DetailValueInvalid;

					if (name.StartsWith("Amount"))
						return value == ""
							? ImporterError.DetailAmountRequired
							: ImporterError.DetailAmountInvalid;

					if (name.StartsWith("Conversion"))
						return ImporterError.DetailConversionInvalid;

					throw exception;
			}
		}

		public IDictionary<Int16, ImporterError> ErrorList { get; }
		public IList<MoveCsv> MoveList { get; }
	}
}
