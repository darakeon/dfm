using System;
using System.Linq;
using DFM.Email;
using DFM.Entities.Enums;
using DFM.Generic;
using DFM.Language;
using DFM.MVC.Helpers.Authorize;
using DFM.MVC.Helpers.Controllers;
using DFM.MVC.Models;
using Keon.Util.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace DFM.MVC.Controllers
{
	[Auth(true)]
	public class TestController : Controller
	{
		[HttpGetAndHead]
		public IActionResult Language()
		{
			return View(
				"AnalyzeDictionary",
				new TestAnalyzeDictionaryModel()
			);
		}

		[HttpGetAndHead]
		public IActionResult EmailLayout()
		{
			var themes = EnumX.AllValues<Theme>();

			var languages = PlainText.AcceptedLanguage();

			var result =
				from language in languages
				from theme in themes
				select getLayout(Format.MoveNotification(language, theme))
				       + getLayout(Format.SecurityAction(language, theme, SecurityAction.PasswordReset))
				       + getLayout(Format.SecurityAction(language, theme, SecurityAction.UserVerification));

			return View(result);
		}

		[HttpGetAndHead]
		public IActionResult SendEmail()
		{
			var message = "The e-mail sending is working!";

			try
			{
				new Sender()
					.Subject("Test")
					.Body(message)
					.To("darakeon@gmail.com")
					.Send();
			}
			catch (Exception e)
			{
				e = e.MostInner();
				message = e.Message + "<br />"
					+ e.StackTrace?.Replace("\n", "<br />");
			}

			return Content(message, "text/html");
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
