using System;
using System.Web.Mvc;
using DFM.MVC.Areas.API.Models;
using DFM.MVC.Helpers.Authorize;
using DFM.MVC.Helpers.Controllers;
using DFM.MVC.Models;

namespace DFM.MVC.Areas.API.Controllers
{
	public class UsersController : BaseJsonController
	{
		[HttpPost]
		public ActionResult Login(String email, String password)
		{
			return json(() =>
			{
				var model =
					new UsersLoginModel
					{
						Email = email,
						Password = password,
					};

				var ticket = model.LogOn();

				return new { ticket };
			});
		}

		[HttpPost]
		public ActionResult Logout()
		{
			return json(() =>
			{
				new SafeModel().LogOff();
			});
		}

		[HttpGetAndHead, ApiAuth]
		public ActionResult Config()
		{
			return json(() => new UserConfigModel());
		}

		[HttpPost, ApiAuth]
		public ActionResult Config(UserConfigModel model)
		{
			return json(model.Save);
		}

		[HttpPost, ApiAuth(false)]
		public ActionResult TFA(UserTFAModel model)
		{
			return json(model.Validate);
		}
	}
}
