using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using JetBrains.Annotations;

[assembly: InternalsVisibleTo("DFM.API.Tests")]
namespace DFM.API.Starters.Routes
{
	class Url
	{
		public Url(String pattern)
		{
			var parts = pattern.Split("/");

			foreach (var part in parts)
			{
				var child =
					getValue("\\{(.+)\\=(.+)\\}", part)
						?? getValue("\\{(.+)\\}", part)
						?? new UrlPart(part);

				children.Add(child);
			}
		}

		private static UrlPart getValue([RegexPattern] String pattern, String part)
		{
			var regex = new Regex(pattern);

			if (!regex.IsMatch(part))
				return null;

			var match = regex.Matches(part)[0];

			return new UrlPart(match);
		}

		private readonly IList<UrlPart> children =
			new List<UrlPart>();

		public String Translate(Object parameters)
		{
			var values = parameters.GetType()
				.GetProperties()
				.ToDictionary(
					m => m.Name.ToLower(),
					m => m.GetValue(parameters)?.ToString()
				);

			return children
				.Select(p => p.Translate(values))
				.Where(p => !String.IsNullOrEmpty(p))
				.Select(p => "/" + p)
				.Aggregate((all, part) => all + part);
		}
	}
}
