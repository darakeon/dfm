using System;
using System.Buffers;
using System.Text;
using DFM.BusinessLogic.Exceptions;
using DFM.MVC.Areas.Api.Models;
using DFM.MVC.Helpers.Controllers;
using DFM.MVC.Helpers.Extensions;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DFM.MVC.Areas.Api.Controllers
{
	[NoWizard]
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
			catch (CoreError e)
			{
				return error(e.Type);
			}
		}

		private Object makeResult(Object model)
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

		[HttpGetAndHead]
		public JsonResult Uninvited()
		{
			return error(Error.Uninvited);
		}

		[HttpGetAndHead]
		public JsonResult AcceptOnlineContract()
		{
			return error(Error.NotSignedLastContract);
		}

		[HttpGetAndHead]
		public JsonResult OpenTFA()
		{
			return error(Error.TFANotVerified);
		}

		private JsonResult error(Error error)
		{
			var result = new
			{
				error = HttpContext.Translate(error),
				code = (Int32) error
			};

			return makeMvcActionResponse(result);
		}

		private JsonResult makeMvcActionResponse(Object result)
		{
			return Json(result);
		}

		protected T getFromBody<T>()
		{
			var body = Request.BodyReader.ReadAsync();
			var json = Encoding.UTF8.GetString(body.Result.Buffer.ToArray());

			return JsonConvert.DeserializeObject<T>(json);
		}
	}
}
