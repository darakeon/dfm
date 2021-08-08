using System.Collections.Generic;
using System.Linq;
using DFM.Entities;

namespace DFM.BusinessLogic.Response
{
	public class CategoryReport
	{
		public CategoryReport()
		{
			List = new List<CategoryValue>();
		}

		public IList<CategoryValue> List { get; set; }

		public void Add(IList<Summary> summaryList)
		{
			List = summaryList
				.Select(s => new CategoryValue(s))
				.OrderBy(i => i.Category)
				.ToList();
		}
	}
}
