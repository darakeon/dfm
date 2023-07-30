using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using DFM.Entities;
using DFM.Language.Emails;
using DFM.Language.Entities;
using DFM.Language.Extensions;
using Keon.Util.Extensions;
using Newtonsoft.Json;

namespace DFM.Language
{
	public class PlainText
	{
		internal static String CurrentPath;

		private static String mainPath => Path.Combine(CurrentPath, "Language");

		private static readonly String siteName = "Site";
		private static String sitePath => Path.Combine(mainPath, siteName);

		private static readonly String emailName = "Email";
		private static String emailPath => Path.Combine(mainPath, emailName);

		private static readonly String versionName = "Version";
		internal static String VersionPath => Path.Combine(mainPath, versionName);

		public static Html Html { get; private set; }
		public static PlainText Site { get; private set; }
		public static PlainText Email { get; private set; }

		private String name { get; }
		public DicList<Section> SectionList { get; }

		private static List<String> acceptedLanguages;

		internal PlainText(String name, String path)
		{
			this.name = name;
			SectionList = new DicList<Section>();
			acceptedLanguages = new List<String>();

			Directory.GetFiles(path, "*.json")
				.ToList()
				.ForEach(addSection);
		}

		private void addSection(String fileName)
		{
			var sectionName = new FileInfo(fileName)
				.Name.Replace(".json", "").ToLower();

			var jsonText = File.ReadAllText(fileName);

			var jsonObject = JsonConvert
				.DeserializeObject<JsonDictionary>(jsonText);

			var section = new Section(sectionName, jsonObject);

			SectionList.Add(section);
		}

		public static void Initialize(String path = "")
		{
			if (CurrentPath != null)
				return;

			CurrentPath = path;

			Site = new PlainText(siteName, sitePath);
			Email = new PlainText(emailName, emailPath);

			Html = new Html();

			setAcceptedLanguages();
		}

		private static void setAcceptedLanguages()
		{
			var dictionaries = Site.SectionList
				.Union(Email.SectionList);

			acceptedLanguages = dictionaries
				.SelectMany(s => s.LanguageList)
				.Select(l => l.Name.ToLower())
				.Distinct()
				.ToList();
		}

		public static Boolean AcceptLanguage(String chosenLanguage)
		{
			return acceptedLanguages.Contains(chosenLanguage?.ToLower());
		}

		public static IList<String> AcceptedLanguages()
		{
			return acceptedLanguages.ToList();
		}

		public DicList<Phrase> this[String section, String language]
			=> this[new [] {section}, language];

		public DicList<Phrase> this[String[] sections, String language]
		{
			get
			{
				var result = SectionList["general"][language].PhraseList;

				foreach (var section in sections.Distinct())
				{
					result = result.Union(
						SectionList[section][language].PhraseList
					);
				}

				return result;
			}
		}

		public String this[String section, String language, params String[] phrase]
		{
			get
			{
				if (phrase.Length == 0)
					throw new ArgumentException("Need at least one phrase.");

				return String.Join(
					" ",
					phrase.ToList().Select(
						p => this[section, language, p]
					)
				);
			}
		}

		public Phrase this[String section, String language, String phrase]
		{
			get
			{
				phrase = Phrase.RemoveWrongCharacters(phrase);

				var result = tryGetText("general", language, phrase)
					?? tryGetText(section, language, phrase);

				if (result == null && name == siteName)
				{
					result = tryGetText("error", language, phrase)
						?? tryGetText("wizard", language, phrase)
						?? tryGetText("email", language, phrase)
						?? tryGetText("tips", language, phrase);
				}

				return result ?? notFound(section, language, phrase);
			}
		}

		private Phrase tryGetText(String section, String language, String phrase)
		{
			try { return SectionList[section][language][phrase]; }
			catch (DicException) { return null; }
		}

		private Phrase notFound(String section, String language, String phrase)
		{
			throw new DicException(
				$"P: {name} /// S: {section} /// L: {language} /// P: {phrase}"
			);
		}

		public static String GetMonthName(Int32 month, String language)
		{
			if (month < 1 || month > 12)
				return "---";

			return CultureInfo.GetCultureInfo(language)
				.DateTimeFormat.GetMonthName(month).Capitalize();
		}

		public static NumberFormatInfo GetNumberFormat(String language)
		{
			return CultureInfo.GetCultureInfo(language).NumberFormat;
		}

		public static String GetMiscText(Misc misc, String language)
		{
			var parts = new[]
			{
				"Start",
				misc.Colors
					? $"Color{misc.Color}"
					: $"Stain{misc.Color}",
				"Comma",
				"Middle",
				$"Eyes{misc.Eye}",
				"Comma",
				$"Arms{misc.Arm}",
				"Comma",
				$"Legs{misc.Leg}",
				"And",
				$"Antenna{misc.Antenna}"
			};

			return String.Join(
				"",
				parts.Select(part => Email["misc", language, part])
			);
		}
	}
}
