using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using DFM.Generic;
using TechTalk.SpecFlow;

namespace DFM.Tests.Util
{
	public abstract class ContextHelper
	{
		protected readonly ScenarioContext context;

		protected ContextHelper(ScenarioContext context)
		{
			this.context = context;
		}

		protected T get<T>(String key)
		{
			return exists(key)
				? (T)context[key]
				: default;
		}

		protected IEnumerable<T> getList<T>(String key)
		{
			return get<IEnumerable<T>>(key) ?? Array.Empty<T>();
		}

		protected Boolean exists(String key)
		{
			return context.ContainsKey(key);
		}

		protected void set(String key, Object value)
		{
			context[key] = value;
		}

		protected static String runPath =>
			Path.GetDirectoryName(typeof(ContextHelper).Assembly.Location);

		protected String scenarioTitle => context?.ScenarioInfo?.Title;
		protected String scenarioCode => scenarioTitle?.ToLower()[..4];

		protected Boolean isCurrent(ScenarioBlock block)
		{
			return context.CurrentScenarioBlock == block;
		}

		protected static Boolean match(String text, String pattern)
		{
			var match = Regex.IsMatch(text, pattern);

			if (match)
				return true;

			var patternOut = pattern
				.ReplaceRegex(@"\\([\/\}\{])", "$1");

			var textOut = text
				.ReplaceRegex(@"style=""[^\""]+""", @"style=""[^\""]+""")
				.ReplaceRegex("[0-9A-F]{32}", @"[0-9A-F]{32}");

			for (var l = 1; l < patternOut.Length; l++)
			{
				if (patternOut[l] != textOut[l])
				{
					throw new Exception(
						@$"
							Wrong match of
							{interval(patternOut, l, 20)}
							into
							{interval(textOut, l, 20)}
						".ReplaceRegex(@"\t{8}", "\t")
					);
				}
			}

			return false;
		}

		private static String interval(String text, Int32 position, Int32 around)
		{
			return text[
				Math.Max(position - around, 0)
				..
				Math.Min(position + around, text.Length)
			];
		}
	}
}
