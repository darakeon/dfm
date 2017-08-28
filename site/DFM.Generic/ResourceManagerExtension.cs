using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Resources;

namespace DFM.Generic
{
	public static class ResourceManagerExtension
	{
		public static Dictionary<String, String> ToDictionary(this ResourceManager resourceManager, CultureInfo cultureInfo = null)
		{
			return resourceManager
				.GetResourceSet(cultureInfo ?? CultureInfo.CurrentUICulture, true, true)
				.Cast<DictionaryEntry>()
				.ToDictionary(i => i.Key.ToString(), i => i.Value.ToString());
		}

		public static Dictionary<String, String> ToDictionary(this IEnumerable<ResourceManager> resourceManagerList, CultureInfo cultureInfo = null)
		{
			var dic = new Dictionary<String, String>();

			foreach (var resourceManager in resourceManagerList)
			{
				resourceManager.ToDictionary(cultureInfo)
					.ToList().ForEach(i => dic.Add(i.Key, i.Value));
			}

			return dic;
		}

	}
}
