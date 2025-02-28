using Keon.TwoFactorAuth;
using System;

namespace DFM.BusinessLogic.Tests.Helpers
{
	internal static class StringExtension
	{
		public static String ForScenario(this String text, String scenarioCode)
		{
			return text.Replace("{scenarioCode}", scenarioCode);
		}

		public static String GenerateTFA(this String? text, String? secret)
		{
			if (text == null || secret == null)
				return text;

			var code = CodeGenerator.Generate(secret);
			return text.Replace("{generated}", code);
		}
	}
}
