using System;
using System.Web.Mvc;
using DFM.MVC.Areas.API.Models;
using DFM.MVC.Helpers.Authorize;
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

		public ActionResult Logout()
		{
			return json(() =>
			{
				new SafeModel().LogOff();
			});
		}

		[DFMApiAuthorize, HttpGet]
		public ActionResult Config()
		{
			return json(() => new UserConfigModel());
		}

		[DFMApiAuthorize, HttpPost]
		public ActionResult Config(UserConfigModel model)
		{
			return json(model.Save);
		}

		[DFMApiAuthorize(false), HttpPost]
		public ActionResult TFA(UserTFAModel model)
		{
			return json(model.Validate);
		}
	}
}
