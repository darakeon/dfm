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

			DetailList = DetailCsv.Convert(move.DetailList);
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
		public IList<DetailCsv> DetailList { get; }

		public bool Equals(IMove other)
		{
			return guid == other.Guid;
		}

		public String Description1 => getDescription(0);
		public String Amount1 => getAmount(0);
		public String Value1 => getValue(0);
		public String Conversion1 => getConversion(0);
		public String Description2 => getDescription(1);
		public String Amount2 => getAmount(1);
		public String Value2 => getValue(1);
		public String Conversion2 => getConversion(1);
		public String Description3 => getDescription(2);
		public String Amount3 => getAmount(2);
		public String Value3 => getValue(2);
		public String Conversion3 => getConversion(2);
		public String Description4 => getDescription(3);
		public String Amount4 => getAmount(3);
		public String Value4 => getValue(3);
		public String Conversion4 => getConversion(3);
		public String Description5 => getDescription(4);
		public String Amount5 => getAmount(4);
		public String Value5 => getValue(4);
		public String Conversion5 => getConversion(4);
		public String Description6 => getDescription(5);
		public String Amount6 => getAmount(5);
		public String Value6 => getValue(5);
		public String Conversion6 => getConversion(5);
		public String Description7 => getDescription(6);
		public String Amount7 => getAmount(6);
		public String Value7 => getValue(6);
		public String Conversion7 => getConversion(6);
		public String Description8 => getDescription(7);
		public String Amount8 => getAmount(7);
		public String Value8 => getValue(7);
		public String Conversion8 => getConversion(7);
		public String Description9 => getDescription(8);
		public String Amount9 => getAmount(8);
		public String Value9 => getValue(8);
		public String Conversion9 => getConversion(8);
		public String Description10 => getDescription(9);
		public String Amount10 => getAmount(9);
		public String Value10 => getValue(9);
		public String Conversion10 => getConversion(9);
		public String Description11 => getDescription(10);
		public String Amount11 => getAmount(10);
		public String Value11 => getValue(10);
		public String Conversion11 => getConversion(10);
		public String Description12 => getDescription(11);
		public String Amount12 => getAmount(11);
		public String Value12 => getValue(11);
		public String Conversion12 => getConversion(11);
		public String Description13 => getDescription(12);
		public String Amount13 => getAmount(12);
		public String Value13 => getValue(12);
		public String Conversion13 => getConversion(12);
		public String Description14 => getDescription(13);
		public String Amount14 => getAmount(13);
		public String Value14 => getValue(13);
		public String Conversion14 => getConversion(13);
		public String Description15 => getDescription(14);
		public String Amount15 => getAmount(14);
		public String Value15 => getValue(14);
		public String Conversion15 => getConversion(14);
		public String Description16 => getDescription(15);
		public String Amount16 => getAmount(15);
		public String Value16 => getValue(15);
		public String Conversion16 => getConversion(15);
		public String Description17 => getDescription(16);
		public String Amount17 => getAmount(16);
		public String Value17 => getValue(16);
		public String Conversion17 => getConversion(16);
		public String Description18 => getDescription(17);
		public String Amount18 => getAmount(17);
		public String Value18 => getValue(17);
		public String Conversion18 => getConversion(17);
		public String Description19 => getDescription(18);
		public String Amount19 => getAmount(18);
		public String Value19 => getValue(18);
		public String Conversion19 => getConversion(18);
		public String Description20 => getDescription(19);
		public String Amount20 => getAmount(19);
		public String Value20 => getValue(19);
		public String Conversion20 => getConversion(19);
		public String Description21 => getDescription(20);
		public String Amount21 => getAmount(20);
		public String Value21 => getValue(20);
		public String Conversion21 => getConversion(20);
		public String Description22 => getDescription(21);
		public String Amount22 => getAmount(21);
		public String Value22 => getValue(21);
		public String Conversion22 => getConversion(21);
		public String Description23 => getDescription(22);
		public String Amount23 => getAmount(22);
		public String Value23 => getValue(22);
		public String Conversion23 => getConversion(22);
		public String Description24 => getDescription(23);
		public String Amount24 => getAmount(23);
		public String Value24 => getValue(23);
		public String Conversion24 => getConversion(23);
		public String Description25 => getDescription(24);
		public String Amount25 => getAmount(24);
		public String Value25 => getValue(24);
		public String Conversion25 => getConversion(24);
		public String Description26 => getDescription(25);
		public String Amount26 => getAmount(25);
		public String Value26 => getValue(25);
		public String Conversion26 => getConversion(25);
		public String Description27 => getDescription(26);
		public String Amount27 => getAmount(26);
		public String Value27 => getValue(26);
		public String Conversion27 => getConversion(26);
		public String Description28 => getDescription(27);
		public String Amount28 => getAmount(27);
		public String Value28 => getValue(27);
		public String Conversion28 => getConversion(27);
		public String Description29 => getDescription(28);
		public String Amount29 => getAmount(28);
		public String Value29 => getValue(28);
		public String Conversion29 => getConversion(28);
		public String Description30 => getDescription(29);
		public String Amount30 => getAmount(29);
		public String Value30 => getValue(29);
		public String Conversion30 => getConversion(29);

		private String getDescription(Int32 index)
		{
			return getDetail(index)?.Description;
		}

		private String getAmount(Int32 index)
		{
			return getDetail(index)?.Amount;
		}

		private String getValue(Int32 index)
		{
			return getDetail(index)?.Value;
		}

		private String getConversion(Int32 index)
		{
			return getDetail(index)?.Conversion;
		}

		private DetailCsv getDetail(Int32 index)
		{
			return DetailList.Count > index
				? DetailList[index]
				: null;
		}

	}
}
