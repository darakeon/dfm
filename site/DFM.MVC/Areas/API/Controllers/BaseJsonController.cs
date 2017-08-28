using System;
using System.Web.Mvc;
using DFM.BusinessLogic.Exceptions;
using DFM.MVC.Helpers.Controllers;
using DFM.MVC.Helpers.Global;

namespace DFM.MVC.Areas.API.Controllers
{
	public class BaseJsonController : BaseController
	{
		protected JsonResult JsonGet(Func<object> action)
		{
			try
			{
				var model = action();
				
				return Json(
					new { data = model }, 
					JsonRequestBehavior.AllowGet
				);
			}
			catch (DFMCoreException e)
			{
				return Json(
					new { error = MultiLanguage.Dictionary[e] }, 
					JsonRequestBehavior.AllowGet
				);
			}
		}



		public JsonResult Uninvited()
		{
			return Json(
				new { error = MultiLanguage.Dictionary[ExceptionPossibilities.Uninvited] }, 
				JsonRequestBehavior.AllowGet
			);
		}



		protected JsonResult JsonPost(Action action)
		{
			try
			{
				action();

				return Json(new { success = true });
			}
			catch (DFMCoreException e)
			{
				return Json(new { error = MultiLanguage.Dictionary[e] });
			}
		}


	}
}
