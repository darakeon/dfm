using System;
using System.Collections.Generic;
using System.Linq;
using DFM.Generic;
using Newtonsoft.Json;

namespace DFM.Language.Emails
{
	public class ThemeColorize
	{
		public class Brightnesses : Dictionary<ThemeBrightness, Brightness> { }

		public class Brightness
		{
			public Values Values { get; set; }
			public Colors Colors { get; set; }
		}

		public class Colors : Dictionary<ThemeColor, Values> { }
		public class Values : Dictionary<String, String> { }

		public static IDictionary<String, String> Get(String json, Theme theme)
		{
			var themes = JsonConvert.DeserializeObject<Brightnesses>(json);

			var brightness = theme.Brightness();
			var color = theme.Color();

			return themes[brightness].Values
				.Union(themes[brightness].Colors[color])
				.ToDictionary(t => t.Key, t => t.Value);
		}
	}
}
