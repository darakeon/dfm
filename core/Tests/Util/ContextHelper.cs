using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
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

		protected static IEnumerable<T> getList<T>(String key)
		{
			return get<IEnumerable<T>>(key) ?? Array.Empty<T>();
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

		protected static Boolean match(String text, String pattern)
		{
			var match = Regex.IsMatch(text, pattern);

			if (match)
				return true;

			for (var l = 1; l < pattern.Length; l++)
			{
				try
				{
					var subPattern = pattern.Substring(0, l);

					if (subPattern.EndsWith("{"))
						continue;

					if (!Regex.IsMatch(text, subPattern))
					{
						throw new Exception($"Wrong match of \n\t{subPattern}\n\tinto\n\t{text}");
					}
				}
				catch (RegexParseException) { }
			}

			return false;
		}
	}
}
