using System;
using System.Collections.Generic;
using DFM.Entities.Bases;
using DFM.Entities.Enums;

namespace DFM.BusinessLogic.Response
{
	public interface IMoveInfo : IDate
	{
		Guid Guid { get; set; }

		String Description { get; set; }
		MoveNature Nature { get; set; }

		Decimal Value { get; set; }
		Decimal? Conversion { get; set; }
		IList<DetailInfo> DetailList { get; set; }

		String CategoryName { get; set; }
		String InUrl { get; set; }
		String OutUrl { get; set; }
	}
}
