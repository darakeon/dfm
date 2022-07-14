using System;
using DFM.MVC.Areas.Api.Models;
using DFM.MVC.Helpers.Authorize;
using DFM.MVC.Helpers.Controllers;
using DFM.MVC.Models;
using DFM.MVC.Starters.Routes;
using Microsoft.AspNetCore.Mvc;

namespace DFM.MVC.Areas.Api.Controllers
{
	[Area(Route.ApiArea)]
	public class UsersController : BaseJsonController
	{
		[HttpPost]
		public IActionResult Login(String email, String password)
		{
			var model =
				new UsersLoginModel
				{
					Email = email,
					Password = password,
				};

			return json(() => new { ticket = model.LogOn() });
		}

		[HttpPost]
		public IActionResult Logout()
		{
			return json(() =>
			{
				new SafeModel().LogOff();
			});
		}

		[HttpGetAndHead, ApiAuth]
		public IActionResult GetSettings()
		{
			return json(() => new UserSettingsModel());
		}

		[HttpPost, ApiAuth]
		public IActionResult SaveSettings()
		{
			var model = getFromBody<UserSettingsModel>();
			return json(model.Save);
		}

		[HttpPost, ApiAuth(AuthParams.IgnoreTFA)]
		public IActionResult TFA(String code)
		{
			var model = new UserTFAModel(code);
			return json(model.Validate);
		}
	}
}
