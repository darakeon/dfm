using System;
using DFM.Entities;

namespace DFM.BusinessLogic.Response
{
	public class CategoryValue
	{
		public CategoryValue(Summary summary)
		{
			Category = summary.Category.Name;
			OutCents = summary.OutCents;
			InCents = summary.InCents;
		}

		public String Category { get; }
		public Int32 OutCents { get; }
		public Int32 InCents { get; }

		public override String ToString()
		{
			return $"{Category}: {InCents}-{OutCents}";
		}
	}
}
