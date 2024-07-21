using System;
using System.Collections.Generic;
using System.Linq;
using DFM.Entities;
using DFM.Entities.Enums;

namespace DFM.BusinessLogic.Response;

public class LineInfo
{
	public static LineInfo Convert(Line line)
	{
		return new()
		{
			Position = line.Position,
			Description = line.Description,
			Date = line.Date,
			Category = line.Category,
			Nature = line.Nature,
			In = line.In,
			Out = line.Out,
			Value = line.Value,
			Conversion = line.Conversion,
			Scheduled = line.Scheduled,
			Status = line.Status,

			DetailList = line.DetailList
				.Select(DetailInfo.Convert).ToList(),
		};
	}

	public Int16 Position { get; set; }

	public String Description { get; set; }
	public DateTime Date { get; set; }
	public String Category { get; set; }
	public MoveNature? Nature { get; set; }
	public String In { get; set; }
	public String Out { get; set; }
	public Decimal? Value { get; set; }
	public Decimal? Conversion { get; set; }

	public DateTime Scheduled { get; set; }
	public ImportStatus Status { get; set; }

	public IList<DetailInfo> DetailList { get; set; }
		= new List<DetailInfo>();
}
