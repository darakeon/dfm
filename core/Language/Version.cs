using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DFM.Generic;
using Newtonsoft.Json;

namespace DFM.Language
{
	public class Version
	{
		private Version(KeyValuePair<String, List<String>> jsonContent)
		{
			Name = jsonContent.Key;
			Updates = jsonContent.Value;
		}

		public String Name { get; }
		public IList<String> Updates { get; }

		static Version()
		{
			foreach (var language in PlainText.AcceptedLanguages())
			{
				var versionsPath = Path.Combine(
					PlainText.VersionPath,
					$"{language}.json"
				);

				var versionsJson = File.ReadAllText(versionsPath);

				var versionsContent = JsonConvert.DeserializeObject
					<Dictionary<String, List<String>>>(versionsJson)
					.Take(Cfg.VersionCount)
					.Select(v => new Version(v))
					.ToList();

				versions.Add(language, versionsContent);
			}
		}

		private static readonly IDictionary<String, IList<Version>> versions
			= new Dictionary<String, IList<Version>>();

		public static IList<Version> Get(String language)
		{
			return versions[language.ToLower()];
		}
	}
}
