﻿using System;
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

			using TextReader reader = new StringReader(content);

			var config = new CsvConfiguration(CultureInfo.CurrentCulture)
			{
				MissingFieldFound = null,
				HeaderValidated = null,
			};

			using var csv = new CsvReader(reader, config);

			List<MoveCsv> moves = new List<MoveCsv>();

			try
			{
				moves = csv.GetRecords<MoveCsv>().ToList();
			}
			catch (TypeConverterException exception)
			{
				Error = handleFieldError(exception);
				return;
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

		public ImporterError? Error { get; }
		public IList<MoveCsv> MoveList { get; }
	}
}