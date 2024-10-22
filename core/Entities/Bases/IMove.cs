﻿using System;
using System.Collections.Generic;
using DFM.Entities.Enums;
using Keon.Util.DB;

namespace DFM.Entities.Bases
{
	public interface IMove : IEntityLong, IDate
	{
		Guid Guid { get; set; }

		String Description { get; set; }
		MoveNature Nature { get; set; }

		Decimal Value { get; set; }
		Decimal? Conversion { get; set; }
		IList<Detail> DetailList { get; set; }

		Category Category { get; set; }
		Account In { get; }
		Account Out { get; }
	}
}
