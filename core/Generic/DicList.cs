using System;
using System.Collections.Generic;

namespace DFM.Generic
{
	public class DicList<V> : DicList<String, V> { }

	public class DicList<K, V> : Dictionary<K, List<V>>
	{
		public void Add(K key, V value)
		{
			if (!ContainsKey(key))
				Add(key, new List<V>());

			this[key].Add(value);
		}
	}
}
