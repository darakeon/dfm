﻿using System;
using System.Collections.Generic;
using System.Linq;
using DFM.Entities;
using DFM.Entities.Bases;
using DFM.Entities.Enums;

namespace DFM.Exchange.Importer;

public class MoveCsv : Line
{
	public MoveCsv()
	{
		DetailList = new List<Detail>();
	}

	public String Description1 { set => setDescription(0, value); }
	public Int16? Amount1 { set => setAmount(0, value); }
	public Decimal? Value1 { set => setValue(0, value); }
	public Decimal? Conversion1 { set => setConversion(0, value); }
	public String Description2 { set => setDescription(1, value); }
	public Int16? Amount2 { set => setAmount(1, value); }
	public Decimal? Value2 { set => setValue(1, value); }
	public Decimal? Conversion2 { set => setConversion(1, value); }
	public String Description3 { set => setDescription(2, value); }
	public Int16? Amount3 { set => setAmount(2, value); }
	public Decimal? Value3 { set => setValue(2, value); }
	public Decimal? Conversion3 { set => setConversion(2, value); }
	public String Description4 { set => setDescription(3, value); }
	public Int16? Amount4 { set => setAmount(3, value); }
	public Decimal? Value4 { set => setValue(3, value); }
	public Decimal? Conversion4 { set => setConversion(3, value); }
	public String Description5 { set => setDescription(4, value); }
	public Int16? Amount5 { set => setAmount(4, value); }
	public Decimal? Value5 { set => setValue(4, value); }
	public Decimal? Conversion5 { set => setConversion(4, value); }
	public String Description6 { set => setDescription(5, value); }
	public Int16? Amount6 { set => setAmount(5, value); }
	public Decimal? Value6 { set => setValue(5, value); }
	public Decimal? Conversion6 { set => setConversion(5, value); }
	public String Description7 { set => setDescription(6, value); }
	public Int16? Amount7 { set => setAmount(6, value); }
	public Decimal? Value7 { set => setValue(6, value); }
	public Decimal? Conversion7 { set => setConversion(6, value); }
	public String Description8 { set => setDescription(7, value); }
	public Int16? Amount8 { set => setAmount(7, value); }
	public Decimal? Value8 { set => setValue(7, value); }
	public Decimal? Conversion8 { set => setConversion(7, value); }
	public String Description9 { set => setDescription(8, value); }
	public Int16? Amount9 { set => setAmount(8, value); }
	public Decimal? Value9 { set => setValue(8, value); }
	public Decimal? Conversion9 { set => setConversion(8, value); }
	public String Description10 { set => setDescription(9, value); }
	public Int16? Amount10 { set => setAmount(9, value); }
	public Decimal? Value10 { set => setValue(9, value); }
	public Decimal? Conversion10 { set => setConversion(9, value); }
	public String Description11 { set => setDescription(10, value); }
	public Int16? Amount11 { set => setAmount(10, value); }
	public Decimal? Value11 { set => setValue(10, value); }
	public Decimal? Conversion11 { set => setConversion(10, value); }
	public String Description12 { set => setDescription(11, value); }
	public Int16? Amount12 { set => setAmount(11, value); }
	public Decimal? Value12 { set => setValue(11, value); }
	public Decimal? Conversion12 { set => setConversion(11, value); }
	public String Description13 { set => setDescription(12, value); }
	public Int16? Amount13 { set => setAmount(12, value); }
	public Decimal? Value13 { set => setValue(12, value); }
	public Decimal? Conversion13 { set => setConversion(12, value); }
	public String Description14 { set => setDescription(13, value); }
	public Int16? Amount14 { set => setAmount(13, value); }
	public Decimal? Value14 { set => setValue(13, value); }
	public Decimal? Conversion14 { set => setConversion(13, value); }
	public String Description15 { set => setDescription(14, value); }
	public Int16? Amount15 { set => setAmount(14, value); }
	public Decimal? Value15 { set => setValue(14, value); }
	public Decimal? Conversion15 { set => setConversion(14, value); }
	public String Description16 { set => setDescription(15, value); }
	public Int16? Amount16 { set => setAmount(15, value); }
	public Decimal? Value16 { set => setValue(15, value); }
	public Decimal? Conversion16 { set => setConversion(15, value); }
	public String Description17 { set => setDescription(16, value); }
	public Int16? Amount17 { set => setAmount(16, value); }
	public Decimal? Value17 { set => setValue(16, value); }
	public Decimal? Conversion17 { set => setConversion(16, value); }
	public String Description18 { set => setDescription(17, value); }
	public Int16? Amount18 { set => setAmount(17, value); }
	public Decimal? Value18 { set => setValue(17, value); }
	public Decimal? Conversion18 { set => setConversion(17, value); }
	public String Description19 { set => setDescription(18, value); }
	public Int16? Amount19 { set => setAmount(18, value); }
	public Decimal? Value19 { set => setValue(18, value); }
	public Decimal? Conversion19 { set => setConversion(18, value); }
	public String Description20 { set => setDescription(19, value); }
	public Int16? Amount20 { set => setAmount(19, value); }
	public Decimal? Value20 { set => setValue(19, value); }
	public Decimal? Conversion20 { set => setConversion(19, value); }

	private void setDescription(Int32 index, String value)
	{
		if (!String.IsNullOrEmpty(value))
		{
			addDetail(index);
			DetailList[index].Description = value;
		}
	}

	private void setAmount(Int32 index, Int16? value)
	{
		if (value.HasValue && value != 0)
		{
			addDetail(index);
			DetailList[index].Amount = value.Value;
		}
	}

	private void setValue(Int32 index, Decimal? value)
	{
		if (value.HasValue && value != 0)
		{
			addDetail(index);
			DetailList[index].Value = value.Value;
		}
	}

	private void setConversion(Int32 index, Decimal? value)
	{
		if (value != null)
		{
			addDetail(index);
			DetailList[index].Conversion = value;
		}
	}

	private void addDetail(Int32 index)
	{
		while (DetailList.Count <= index)
		{
			DetailList.Add(new Detail{Amount = 0});
		}
	}

	private Move move { get; set; }

	public Move ToMove(
		IDictionary<String, Account> accounts,
		IDictionary<String, Category> categories
	)
	{
		if (move == null)
		{
			var hasIn = !String.IsNullOrEmpty(In);
			var hasOut = !String.IsNullOrEmpty(Out);

			var hasCategory = !String.IsNullOrEmpty(Category);

			move = new Move
			{
				Description = Description,
				Nature = getNature(hasIn, hasOut),

				In = hasIn ? accounts[In] : null,
				Out = hasOut ? accounts[Out] : null,
				Category = hasCategory ? categories[Category] : null,

				Value = Value ?? 0,
				Conversion = Conversion,
				DetailList = DetailList
			};

			move.DetailList
				.ToList()
				.ForEach(d =>
				{
					d.Line = this;
					d.Guid = Guid.NewGuid();
				});

			move.SetDate(Date);
		}

		return move;
	}

	private MoveNature getNature(Boolean hasIn, Boolean hasOut)
	{
		if (Nature.HasValue)
			return Nature.Value;

		if (!hasIn)
			return MoveNature.Out;

		if (!hasOut)
			return MoveNature.In;

		return MoveNature.Transfer;
	}

	public Line ToLine(Archive archive)
	{
		Archive = archive;
		return this;
	}
}