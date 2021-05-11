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

		protected static void set(String key, Object value)
		{
			context[key] = value;
		}

		protected static String runPath =>
			Path.GetDirectoryName(typeof(ContextHelper).Assembly.Location);

		#pragma warning disable 618
		protected static ScenarioContext context => ScenarioContext.Current;
		#pragma warning restore 618

		protected static String scenarioTitle => context?.ScenarioInfo?.Title;
		protected String scenarioCode => scenarioTitle?.ToLower().Substring(0, 4);

		protected Boolean isCurrent(ScenarioBlock block)
		{
			return context.CurrentScenarioBlock == block;
		}
	}
}
