using System;
using System.Collections.Generic;
using System.Linq;

namespace DFM.Language.Extensions
{
	public class DicList<T> : List<T>
		where T : class, INameable
	{
		public T this[String name]
		{
			get
			{
				var item = this.FirstOrDefault(t => t.Name
					.Equals(name, StringComparison.InvariantCultureIgnoreCase));

				if (item == null)
					throw new DicException($"No {typeof (T)} {name}");

				return item;
			}
		}
	}
}
