using System;
using System.Linq;
using System.Web.Mvc;
using DFM.Email;
using DFM.Entities.Enums;
using DFM.Multilanguage;
using DFM.Multilanguage.Emails;
using DFM.MVC.Helpers.Authorize;
using DFM.MVC.Models;

namespace DFM.MVC.Controllers
{
	[DFMAuthorize(true)]
	public class TestController : Controller
	{
		[HttpGet]
		public ActionResult Index()
		{
			return View("AnalyzeDictionary", new TestAnalyzeDictionary());
		}

		[HttpGet]
		public ActionResult Email()
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
				Request.Url?.GetComponents(
					UriComponents.Scheme | UriComponents.HostAndPort, UriFormat.UriEscaped
				)
			);
		}
	}
}
