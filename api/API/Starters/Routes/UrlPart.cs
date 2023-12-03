using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace DFM.API.Starters.Routes
{
	class UrlPart
	{
		public UrlPart(Match match)
		{
			name = match.Groups[1].Value;

			if (name.EndsWith("?"))
			{
				optional = true;
				name = name[..^1];
			}

			name = name.ToLower();

			if (match.Groups.Count > 2)
			{
				value = match.Groups[2].Value;
			}
		}

		public UrlPart(String part)
		{
			value = part;
		}

		private readonly Boolean optional;
		private readonly String name;
		private readonly String value;

		public String Translate(Dictionary<String, String> values)
		{
			if (name == null)
				return value;

			if (values.ContainsKey(name))
				return values[name];

			if (!optional)
				return value;

			return String.Empty;
		}
	}
}
