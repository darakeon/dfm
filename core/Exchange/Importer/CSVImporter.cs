using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using DFM.Entities.Bases;

namespace DFM.Exchange.Importer
{
	public class CSVImporter
	{
		public CSVImporter(String content)
		{
			if (content.Length > MaxLen.ArchiveSize)
			{
				ErrorList.Add(0, ImporterError.Size);
				return;
			}

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

					readMove(csv, line);
				}
				catch (TypeConverterException exception)
				{
					ErrorList.Add(line, handleFieldError(exception));
				}
				catch (ReaderException exception)
				{
					ErrorList.Add(line, handleFieldError(exception));
				}
			}

			if (line > MaxLen.ArchiveLines)
			{
				ErrorList.Add(0, ImporterError.Lines);
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

		private void readMove(CsvReader csv, Int16 line)
		{
			var move = csv.GetRecord<MoveCsv>();

			if (move == null) return;

			var noNature = move.Nature == null;

			var noAccounts =
				String.IsNullOrEmpty(move.In)
			        && String.IsNullOrEmpty(move.Out);

			if (noNature && noAccounts)
			{
				ErrorList.Add(line, ImporterError.NatureRequired);
				return;
			}

			move.Position = line;
			MoveList.Add(move);
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

		private ImporterError handleFieldError(ReaderException exception)
		{
			if (exception.Data.Count == 0)
				return ImporterError.Empty;

			throw exception;
		}

		public IDictionary<Int16, ImporterError> ErrorList { get; }
			= new Dictionary<Int16, ImporterError>();

		public IList<MoveCsv> MoveList { get; }
			= new List<MoveCsv>();
	}
}
