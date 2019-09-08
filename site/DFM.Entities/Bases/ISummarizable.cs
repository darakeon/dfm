using System;
using System.Collections.Generic;
using Keon.Util.DB;

namespace DFM.Entities.Bases
{
	public interface ISummarizable : IEntityLong
	{
		IList<Summary> SummaryList { get; set; }
		Int16 Time { get; set; }

		Summary AddSummary(Category category);

		Summary this[String categoryName] { get; }
	}
}
