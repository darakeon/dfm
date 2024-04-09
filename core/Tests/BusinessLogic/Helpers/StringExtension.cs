using System;

namespace DFM.BusinessLogic.Tests.Helpers
{
	internal static class StringExtension
	{
		public static String ForScenario(this String text, String scenarioCode)
		{
			return text.Replace("{scenarioCode}", scenarioCode);
		}
	}
}
