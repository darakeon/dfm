using System;
using System.Collections.Generic;
using System.Linq;

namespace DFM.Generic.Pages
{
	public class Pages<T>
	{
		public Boolean HasMoreBack { get; }
		private OperatorBox<T> operatorMoreBack { get; }
		public T MoreBack => operatorMoreBack.Item;

		public Boolean HasMoreForward { get; }
		private OperatorBox<T> operatorMoreForward { get; }
		public T MoreForward => operatorMoreForward.Item;

		private IList<OperatorBox<T>> operatorList { get; }
		public IList<T> List => operatorList.Select(p => p.Item).ToList();

		public T Current { get; }

		public Pages(T minimum, T maximum, T current, Int32 rangeSize)
		{
			Current = current;

			var operatorMinimum = new OperatorBox<T>(minimum);
			var operatorMaximum = new OperatorBox<T>(maximum);
			var operatorCurrent = new OperatorBox<T>(current);

			var first = operatorCurrent - (rangeSize/2);
			var last = operatorCurrent + (rangeSize/2);

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

			operatorList = new List<OperatorBox<T>>();

			for (var page = first; page <= last; page++)
			{
				operatorList.Add(page);
			}

			operatorMoreBack = operatorList.First() - 1;
			HasMoreBack = operatorMoreBack >= operatorMinimum;

			operatorMoreForward = operatorList.Last() + 1;
			HasMoreForward = operatorMoreForward <= operatorMaximum;
		}

	}
}
