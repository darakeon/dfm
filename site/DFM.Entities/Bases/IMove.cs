using System;
using System.Collections.Generic;
using Keon.Util.DB;
using DFM.Entities.Enums;

namespace DFM.Entities.Bases
{
	public interface IMove : IEntityLong
	{
		String Description { get; set; }
		DateTime Date { get; set; }
		MoveNature Nature { get; set; }
		Decimal? Value { get; set; }
		Category Category { get; set; }
		IList<Detail> DetailList { get; set; }

		void AddDetail(Detail detail);
		Decimal Total();
		Account In { get; }
		Account Out { get; }
	}
}
