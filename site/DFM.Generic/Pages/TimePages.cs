using System;
using System.Collections.Generic;
using System.Linq;

namespace DFM.Generic.Pages
{
	public class TimePages<T>
	{
		public Boolean HasMoreBack { get; }
		private OperatorBox<T> operatorMoreBack { get; }
		public T MoreBack => operatorMoreBack.Item;

		public Boolean HasMoreFoward { get; }
		private OperatorBox<T> operatorMoreFoward { get; }
		public T MoreFoward => operatorMoreFoward.Item;

		private IList<OperatorBox<T>> operatorPages { get; }
		public IList<T> Pages => operatorPages.Select(p => p.Item).ToList();
		
		public T Current { get; }

		public TimePages(T minimum, T maximum, T current, Int32 rangeSize)
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

			operatorPages = new List<OperatorBox<T>>();

			for (var page = first; page <= last; page++)
			{
				operatorPages.Add(page);
			}

			operatorMoreBack = operatorPages.First() - 1;
			HasMoreBack = operatorMoreBack >= operatorMinimum;

			operatorMoreFoward = operatorPages.Last() + 1;
			HasMoreFoward = operatorMoreFoward <= operatorMaximum;
		}

	}
}