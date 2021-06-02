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
				var item = get(name);

				if (item == null)
					throw new DicException($"No {typeof (T)} {name}");

				return item;
			}
		}

		private T get(String name)
		{
			return this.FirstOrDefault(t => t.Name
				.Equals(name, StringComparison.InvariantCultureIgnoreCase));
		}

		private Boolean available(T other)
		{
			return get(other.Name) == null;
		}

		public DicList<T> Union(DicList<T> other)
		{
			var union = new DicList<T>();
			union.AddRange(this);
			union.AddRange(other.Where(available));
			return union;
		}
	}
}
