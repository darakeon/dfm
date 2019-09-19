using System;
using System.Collections.Generic;
using DFM.Entities.Enums;
using Keon.Util.DB;

namespace DFM.BusinessLogic.Response
{
	public interface IMoveInfo : IEntityLong
	{
		String Description { get; set; }
		DateTime Date { get; set; }
		MoveNature Nature { get; set; }

		Decimal? Value { get; set; }
		IList<DetailInfo> DetailList { get; set; }

		String CategoryName { get; set; }
		String InUrl { get; set; }
		String OutUrl { get; set; }
	}
}
