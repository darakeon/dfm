using System;
using System.Web.Mvc;
using DFM.BusinessLogic.Exceptions;
using DFM.MVC.Areas.API.Models;
using DFM.MVC.Helpers.Controllers;
using DFM.MVC.Helpers.Global;

namespace DFM.MVC.Areas.API.Controllers
{
	public class BaseJsonController : BaseController
	{
		protected JsonResult json(Action action)
		{
			return json(() =>
			{
				action();
				return new { success = true };
			});
		}

		protected JsonResult json<T>(Func<T> action)
			where T : class
		{
			try
			{
				var model = action();

				var result = makeResult(model);

				return makeMvcActionResponse(result);
			}
			catch (DFMCoreException e)
			{
				return error(e.Type);
			}
		}

		private object makeResult(object model)
		{
			if (model is BaseApiModel apiModel)
			{
				return new
				{
					data = apiModel,
					environment = apiModel.Environment
				};
			}

			return new { data = model };
		}

		public JsonResult Uninvited()
		{
			return error(ExceptionPossibilities.Uninvited);
		}

		public JsonResult AcceptOnlineContract()
		{
			return error(ExceptionPossibilities.NotSignedLastContract);
		}

		public JsonResult OpenTFA()
		{
			return error(ExceptionPossibilities.TFANotVerified);
		}

		private JsonResult error(ExceptionPossibilities error)
		{
			var result = new
			{
				error = MultiLanguage.Dictionary[error],
				code = (Int32) error
			};

			return makeMvcActionResponse(result);
		}

		private JsonResult makeMvcActionResponse(object result)
		{
			return Json(result, JsonRequestBehavior.AllowGet);
		}
	}
}
