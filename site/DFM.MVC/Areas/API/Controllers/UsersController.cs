using System;
using System.Web.Mvc;
using DFM.BusinessLogic.Exceptions;
using DFM.MVC.Areas.API.Models;
using DFM.MVC.Helpers.Authorize;
using DFM.MVC.Helpers.Global;
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

				model.LogOn();

				return new {ticket = Current.Ticket.Key};
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
			try
			{
				model.Save();

				return JsonPostSuccess();
			}
			catch (DFMCoreException e)
			{
				return JsonPostError(MultiLanguage.Dictionary[e]);
			}
		}



	}
}
