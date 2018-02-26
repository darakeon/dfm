using System;
using System.Web.Mvc;
using DFM.MVC.Areas.API.Models;
using DFM.MVC.Helpers.Authorize;
using DFM.MVC.Models;

namespace DFM.MVC.Areas.API.Controllers
{
	public class UsersController : BaseJsonController
	{
		public ActionResult Login(String email, String password)
		{
			return JsonGet(() =>
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
			return JsonGet(() =>
			{
				var model = new SafeModel();

				model.LogOff();

				return new {success = true};
			});
		}



		[DFMApiAuthorize, HttpGet]
		public ActionResult GetConfig()
		{
			return JsonGet(() => new UserGetConfigModel());
		}

		[DFMApiAuthorize, HttpPost]
		public ActionResult SaveConfig(UserSaveConfigModel model)
		{
			return JsonPost(model.Save);
		}

		[DFMApiAuthorize(false), HttpPost]
		public ActionResult TFA(UserTFAModel model)
		{
			return JsonPost(model.Validate);
		}
	}
}
