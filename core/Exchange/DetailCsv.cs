using System;
using System.Collections.Generic;
using System.Linq;
using DFM.Entities;
using DFM.Exchange.Exporter;

namespace DFM.Exchange;

public class DetailCsv
{
	private DetailCsv(Detail detail)
	{
		Description = detail.Description;
		Amount = detail.Amount.ToString();
		Value = detail.Value.ToCsv();
		Conversion = detail.Conversion.ToCsv();
	}

	public String Description { get; set; }
	public String Amount { get; set; }
	public String Value { get; set; }
	public String Conversion { get; set; }

	public static IList<DetailCsv> Convert(IList<Detail> details)
	{
		return details
			.Select(d => new DetailCsv(d))
			.ToList();
	}
}
