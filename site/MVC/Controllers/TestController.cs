using System;
using System.Linq;
using DFM.Email;
using DFM.Entities;
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
	[Auth(AuthParams.Admin), Wizard.Avoid]
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

			var languages = PlainText.AcceptedLanguages();

			var fakeUsers =
				from language in languages
				from theme in themes
				select fakeUser(language, theme);

			var result =
				from fakeUser in fakeUsers
				select getLayout(Format.MoveNotification(fakeUser))
					+ getLayout(Format.SecurityAction(fakeUser, SecurityAction.PasswordReset))
					+ getLayout(Format.SecurityAction(fakeUser, SecurityAction.UserVerification))
					+ getLayout(Format.UserRemoval(fakeUser, RemovalReason.NoInteraction))
					+ getLayout(Format.UserRemoval(fakeUser, RemovalReason.NotSignedContract))
					+ getLayout(Format.WipeNotice(fakeUser, RemovalReason.NoInteraction))
					+ getLayout(Format.WipeNotice(fakeUser, RemovalReason.NotSignedContract))
					+ getLayout(Format.WipeNotice(fakeUser, RemovalReason.PersonAsked))
					;

			return View(result);
		}

		private static User fakeUser(String language, Theme theme)
		{
			return new()
			{
				Config = new Config
				{
					Language = language,
					Theme = theme,
				},
				Control = new Control
				{
					MiscDna = Misc.RandomDNA()
				},
			};
		}

		private String getLayout(Format format)
		{
			return format.Layout.Replace(
				"{{Url}}",
				Request.Scheme + "://" + Request.Host
			);
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

		[HttpGetAndHead]
		public IActionResult Error()
		{
			throw new Exception("Logging right!");
		}
	}
}
