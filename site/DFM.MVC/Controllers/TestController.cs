using System;
using System.Web.Mvc;
using DFM.Email;
using DFM.Entities.Enums;
using DFM.MVC.Helpers.Authorize;
using DFM.MVC.Models;

namespace DFM.MVC.Controllers
{
	[DFMAuthorize(true)]
	public class TestController : Controller
	{
		public ActionResult Index()
		{
			return View("AnalyzeDictionary", new TestAnalyzeDictionary());
		}

		public ActionResult Email(String id)
		{
			var paramList = id.Split('_');
			var language = paramList[0];
			//var theme = paramList[1];

			var result = 
				Format.MoveNotification(language).Layout
				+ Format.SecurityAction(language, SecurityAction.PasswordReset).Layout
				+ Format.SecurityAction(language, SecurityAction.UserVerification).Layout;

			return View((object)result);
		}

	}
}
