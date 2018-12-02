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

		[HttpPost]
		public ActionResult Logout()
		{
			return json(() =>
			{
				new SafeModel().LogOff();
			});
		}

		[HttpGet, DFMApiAuthorize]
		public ActionResult Config()
		{
			return json(() => new UserConfigModel());
		}

		[HttpPost, DFMApiAuthorize]
		public ActionResult Config(UserConfigModel model)
		{
			return json(model.Save);
		}

		[HttpPost, DFMApiAuthorize(false)]
		public ActionResult TFA(UserTFAModel model)
		{
			return json(model.Validate);
		}
	}
}
