using System;
using System.IO;
using TechTalk.SpecFlow;

namespace DFM.Tests.Helpers
{
	public class ContextHelper
	{
		protected static T Get<T>(String key)
		{
			return ScenarioContext.Current.ContainsKey(key)
				? (T)ScenarioContext.Current[key]
				: default(T);
		}

		protected static void Set(String key, object value)
		{
			ScenarioContext.Current[key] = value;
		}

		protected static String RunPath
		{
			get
			{
				var assembly = typeof(ContextHelper).Assembly;

				var uri = new UriBuilder(assembly.CodeBase);
				var path = Uri.UnescapeDataString(uri.Path);

				return Path.GetDirectoryName(path);
			}
		}
	}
}
