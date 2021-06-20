using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using DFM.Language;
using NUnit.Framework;

namespace DFM.MVC.Tests
{
	class TranslationsTest
	{
		public TranslationsTest()
		{
			PlainText.Initialize();
		}

		[Test]
		public void HaveAllTranslations()
		{
			var up = "..";
			var path = Path.Combine(
				Directory.GetCurrentDirectory(),
				up, up, up, up, up,
				"MVC"
			);

			var exceptions = scanDir(
				Path.GetFullPath(path)
			).ToList();

			if (exceptions.Any())
			{
				throw new AggregateException(exceptions);
			}
		}

		private static IEnumerable<Exception> scanDir(String path)
		{
			var htmlFiles = Directory
				.GetFiles(path, "*.cshtml");

			foreach (var file in htmlFiles)
			{
				var exceptions = scanFile(file);
				foreach (var exception in exceptions)
				{
					yield return exception;
				}
			}

			var children = Directory
				.GetDirectories(path)
				.Where(notGenerated);

			foreach (var child in children)
			{
				var exceptions = scanDir(child);
				foreach (var exception in exceptions)
				{
					yield return exception;
				}
			}
		}

		private static IEnumerable<Exception> scanFile(String file)
		{
			var html = File.ReadAllText(file);

			var matches = findTranslations(file, html);
			if (matches.Count == 0) yield break;

			var dir = Path.GetDirectoryName(file);
			if (dir == null) yield break;
			var section = new DirectoryInfo(dir).Name;

			if (section == "Shared")
				section = "general";

			foreach (Match match in matches)
			{
				var phrases = match.Groups[1].Value
					.Split("\"")
					.Where((_,i) => i % 2 == 1);

				foreach (var phrase in phrases)
				{
					var exception = checkPhrase(phrase, section);
					if (exception != null) yield return exception;
				}
			}
		}

		private static MatchCollection findTranslations(String file, String html)
		{
			var count = html.Split("Translate").Length - 1;

			var regex = new Regex(@"Translate\(([^\)]+)\)");
			var matches = regex.Matches(html);

			Assert.AreEqual(
				count, matches.Count,
				$"Problem at {file}, regex ignored {count - matches.Count} occurrences"
			);

			return matches;
		}

		private static Exception checkPhrase(String phrase, String section)
		{
			// this means a variable is being used inside the phrase
			if (phrase.Contains("{"))
				return null;

			try
			{
				var translation = PlainText.Site[section, "pt-BR", phrase];

				Assert.NotNull(translation);
				Assert.IsNotEmpty(translation);

				return null;
			}
			catch (Exception e)
			{
				return new Exception($"Error on {phrase}: {e.Message}", e);
			}
		}

		private static Boolean notGenerated(String c)
		{
			return !c.EndsWith("bin") && !c.EndsWith("obj");
		}
	}
}
