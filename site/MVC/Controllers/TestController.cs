using System;
using System.Linq;
using DFM.Email;
using DFM.Entities.Enums;
using DFM.Language;
using DFM.Language.Emails;
using DFM.MVC.Helpers.Authorize;
using DFM.MVC.Helpers.Controllers;
using DFM.MVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace DFM.MVC.Controllers
{
	[Auth(true)]
	public class TestController : Controller
	{
		[HttpGetAndHead]
		public IActionResult Index()
		{
			return View(
				"AnalyzeDictionary",
				new TestAnalyzeDictionaryModel()
			);
		}

		[HttpGetAndHead]
		public IActionResult Email()
		{
			var themes = new[] {SimpleTheme.Dark, SimpleTheme.Light};
			var languages = PlainText.AcceptedLanguage();

			var result =
				from language in languages
				from theme in themes
				select getLayout(Format.MoveNotification(language, theme))
					 + getLayout(Format.SecurityAction(language, theme, SecurityAction.PasswordReset))
					 + getLayout(Format.SecurityAction(language, theme, SecurityAction.UserVerification));

			return View(result);
		}

		private String getLayout(Format format)
		{
			return format.Layout.Replace(
				"{{Url}}",
				Request.Scheme + "://" + Request.Host
			);
		}

		[HttpGetAndHead]
		public IActionResult Error()
		{
			throw new Exception("Logging right!");
		}
	}
}
