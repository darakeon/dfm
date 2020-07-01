using System;
using System.IO;
using TechTalk.SpecFlow;

namespace DFM.Tests.Util
{
	public class ContextHelper
	{
		protected static T get<T>(String key)
		{
			return exists(key)
				? (T)context[key]
				: default;
		}

		protected static Boolean exists(String key)
		{
			return context.ContainsKey(key);
		}

		protected static void set(String key, object value)
		{
			context[key] = value;
		}

		protected static String runPath
		{
			get
			{
				var assembly = typeof(ContextHelper).Assembly;

				var uri = new UriBuilder(assembly.CodeBase);
				var path = Uri.UnescapeDataString(uri.Path);

				return Path.GetDirectoryName(path);
			}
		}

		#pragma warning disable 618
		protected static ScenarioContext context => ScenarioContext.Current;
		#pragma warning restore 618
	}
}
