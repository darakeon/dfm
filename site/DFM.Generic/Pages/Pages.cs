using System;
using System.Collections.Generic;
using System.Linq;

namespace DFM.Generic.Pages
{
	public class Pages
	{
		public IList<IPage> List { get; }

		public IPage Current { get; }

		public Boolean HasMoreBack { get; }
		public IPage MoreBack { get; }

		public Boolean HasMoreForward { get; }
		public IPage MoreForward { get; }

		public Pages(IPage minimum, IPage maximum, IPage current, Int32 rangeSize)
		{
			Current = current;

			var operatorMinimum = new OperatorBox(minimum);
			var operatorMaximum = new OperatorBox(maximum);
			var operatorCurrent = new OperatorBox(current);

			var first = operatorCurrent - rangeSize/2;
			var last = operatorCurrent + rangeSize/2;

			while (first < operatorMinimum)
			{
				first++;

				if (last < operatorMaximum)
				{ last++; }
			}

			while (last > operatorMaximum)
			{
				if (first > operatorMinimum)
				{ first--; }

				last--;
			}

			var operatorList = new List<OperatorBox>();

			for (var page = first; page <= last; page++)
			{
				operatorList.Add(page);
			}

			List = operatorList.Select(p => p.Item).ToList();

			var operatorMoreBack = operatorList.First() - 1;
			HasMoreBack = operatorMoreBack >= operatorMinimum;
			MoreBack = operatorMoreBack.Item;			

			var operatorMoreForward = operatorList.Last() + 1;
			HasMoreForward = operatorMoreForward <= operatorMaximum;
			MoreForward = operatorMoreForward.Item;
		}
	}
}
