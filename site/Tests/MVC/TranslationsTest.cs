using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using DFM.Generic;
using DFM.Language;
using DFM.Language.Entities;
using DFM.Language.Extensions;
using DFM.MVC.Helpers.Controllers;
using Microsoft.AspNetCore.Mvc;
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

			var regex = new Regex(@"Translate(?:List)?\(([^\)]+)\)");
			var matches = regex.Matches(html);

			Assert.That(
				matches.Count, Is.EqualTo(count),
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
				var translation = PlainText.Site[section, "pt-BR", phrase]?.Text;

				Assert.That(translation, Is.Not.Null);
				Assert.That(translation, Is.Not.Empty);

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

		[Test]
		public void AllActionsHaveWizard()
		{
			var asm = typeof(Program).Assembly;

			var controllers = asm.GetTypes()
				.Where(shouldHaveWizard);

			var issues = new List<Exception>();

			foreach (var controller in controllers)
			{
				var controllerName = 
					controller.Name.Replace("Controller", "");

				var isAction = BindingFlags.Instance
					| BindingFlags.DeclaredOnly
					| BindingFlags.Public;

				var actionNames = controller
					.GetMethods(isAction)
					.Where(a => a.ShouldHaveWizard())
					.Select(a => a.Name)
					.Distinct();

				foreach (var actionName in actionNames)
				{
					var errorMessage =
						$"Could not find wizard for '{controllerName}/{actionName}'";

					if (issues.Any(e => e.Message == errorMessage))
						continue;

					var phrase = $"Wizard_{controllerName}{actionName}";

					var translation = tryGetWizardPtBr(phrase);

					if (translation?.Texts != null)
						continue;

					var translationEmpty = tryGetWizardPtBr(phrase + "Empty");
					var translationFilled = tryGetWizardPtBr(phrase + "Filled");

					if (
						translationEmpty?.Texts != null
						&& translationFilled?.Texts != null
					)
						continue;

					issues.Add(new Exception(errorMessage));
				}
			}

			if (issues.Any())
			{
				throw new AggregateException($"Problems: {issues.Count}", issues);
			}
		}

		private Boolean shouldHaveWizard(Type type)
		{
			return typeof(Controller).IsAssignableFrom(type)
					&& !type.IsAbstract
					&& type.ShouldHaveWizard();
		}

		private static Phrase tryGetWizardPtBr(String phrase)
		{
			try
			{
				return PlainText.Site["wizard", "pt-BR", phrase];
			}
			catch (DicException)
			{
				return null;
			}
		}


		[Test]
		public void AllWizardPlusHaveTranslations()
		{
			var siteDir = Directory
				.GetParent(FilePath.Get())!
				.Parent!.Parent!.FullName;

			var mvcDir = Path.Combine(siteDir, "MVC");
			var viewsParents = new[] {mvcDir}.ToList();

			var areasDir = Path.Combine(mvcDir, "Areas");
			var areas = Directory.GetDirectories(areasDir);

			foreach (var area in areas)
			{
				viewsParents.Add(area);
			}

			var views = new List<String>();

			foreach (var viewsParent in viewsParents)
			{
				var viewsFolder = Path.Combine(viewsParent, "Views");

				if (Directory.Exists(viewsFolder))
				{
					views.AddRange(getViews(viewsFolder));
				}
			}

			var wizardPlusList = new List<String>();

			foreach (var view in views)
			{
				var name = view.Replace(mvcDir, "");

				var isWizardPlus = 
					new Regex("WizardPlus =(?: \"([^\"]+)\")?;(?: // (.+))?");

				var wizardPlusLines = File
					.ReadAllLines(view)
					.Where(l => isWizardPlus.IsMatch(l))
					.Select(l => isWizardPlus.Match(l).Groups);

				foreach (var groups in wizardPlusLines)
				{
					Assert.That(
						groups.Count, Is.GreaterThanOrEqualTo(2),
						$"Wizard Plus values not found for {name}"
					);

					var plus = groups[1].Value;
					var actionName = groups[2].Value;

					if (String.IsNullOrEmpty(actionName))
					{
						var wizardPlusParts = view
							.Split(Path.DirectorySeparatorChar, '.')
							.ToArray()
							[^3..^1];

						var regexWords = new Regex("([A-Z][a-z]+)");
						var words = regexWords.Matches(wizardPlusParts[1]);

						foreach (Match word in words)
						{
							var wizardPlus = wizardPlusParts[0] + word + plus;

							if (!wizardPlusList.Contains(wizardPlus))
								wizardPlusList.Add(wizardPlus);
						}
					}
					else
					{
						var actions = actionName.Split(',');

						foreach (var action in actions)
						{
							var wizardPlus = action + plus;

							if (!wizardPlusList.Contains(wizardPlus))
								wizardPlusList.Add(wizardPlus);
						}
					}

				}
			}

			var issues = new List<Exception>();

			foreach (var wizardPlus in wizardPlusList)
			{
				var errorMessage =
					$"Could not find wizard for '{wizardPlus}'";

				var phrase = $"Wizard_{wizardPlus}";

				var translation = tryGetWizardPtBr(phrase);

				if (translation?.Texts != null)
					continue;

				var translationEmpty = tryGetWizardPtBr(phrase + "Empty");
				var translationFilled = tryGetWizardPtBr(phrase + "Filled");

				if (
					translationEmpty?.Texts != null
					&& translationFilled?.Texts != null
				)
					continue;

				issues.Add(new Exception(errorMessage));
			}

			if (issues.Any())
			{
				throw new AggregateException($"Problems: {issues.Count}", issues);
			}
		}

		private IList<String> getViews(String viewsFolder)
		{
			var views = Directory
				.GetFiles(viewsFolder, "*.cshtml")
				.ToList();

			foreach (var viewsSubfolder in Directory.GetDirectories(viewsFolder))
			{
				views.AddRange(getViews(viewsSubfolder));
			}

			return views;
		}
	}
}
