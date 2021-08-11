using System.Collections.Generic;
using System.Linq;
using DFM.Entities;
using DFM.Entities.Enums;

namespace DFM.BusinessLogic.Response
{
	public class CategoryReport
	{
		public CategoryReport(SummaryNature nature, IList<Summary> summaryList)
		{
			Nature = nature;
			List = summaryList
				.Select(s => new CategoryValue(s))
				.OrderBy(i => i.Category)
				.ToList();
		}

		public IList<CategoryValue> List { get; }
		public SummaryNature Nature { get; }
	}
}
