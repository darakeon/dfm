using System.Collections.Generic;

namespace DFM.Generic
{
	public static class DictionaryExtension
	{
		public static TValue TryGetValue<TKey, TValue>(
			this IDictionary<TKey, TValue> dictionary,
			TKey key, TValue defaultValue = default
		)
		{
			return dictionary.TryGetValue(key, out var value)
				? value
				: defaultValue;
		}
	}
}
