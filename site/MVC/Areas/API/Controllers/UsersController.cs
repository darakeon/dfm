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

			var ticket = model.LogOn();

			return json(() => new { ticket });
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
		public IActionResult GetConfig()
		{
			return json(() => new UserConfigModel());
		}

		[HttpPost, ApiAuth]
		public IActionResult SaveConfig()
		{
			var model = getFromBody<UserConfigModel>();
			return json(model.Save);
		}

		[HttpPost, ApiAuth(false)]
		public IActionResult TFA(String code)
		{
			var model = new UserTFAModel(code);
			return json(model.Validate);
		}
	}
}
