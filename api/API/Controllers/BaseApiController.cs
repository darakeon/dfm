using System;
using System.Net;
using DFM.API.Helpers.Extensions;
using DFM.API.Models;
using DFM.BusinessLogic.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DFM.API.Controllers
{
	[ApiController]
	public class BaseApiController : Controller
	{
		protected JsonResult json(Action action)
		{
			return json(() =>
			{
				action();
				return (BaseApiModel) null;
			});
		}

		protected JsonResult jsonNonBaseApi<T>(Func<T> action)
		{
			try
			{
				var model = action();

				var result = model != null
					? new ResponseModel(model, null)
					: new ResponseModel();

				return Json(result);
			}
			catch (CoreError e)
			{
				return error(e.Type);
			}
		}

		protected JsonResult json<T>(Func<T> action)
			where T : BaseApiModel
		{
			try
			{
				var model = action();

				var result = model != null
					? new ResponseModel(model, model.Environment)
					: new ResponseModel();

				return Json(result);
			}
			catch (CoreError e)
			{
				return error(e.Type);
			}
		}

		protected JsonResult error(Error error)
		{
			return ApiError(HttpContext, error);
		}

		public static JsonResult ApiError(HttpContext context, Error error)
		{
			var result = new ResponseModel(
				(Int32)error,
				context.Translate(error)
			);

			context.Response.StatusCode = (Int32)getStatusCode(error);
			return new JsonResult(result);
		}

		private static HttpStatusCode getStatusCode(Error error)
		{
			switch (error)
			{
				case Error.LoginRequested:
					return HttpStatusCode.Unauthorized;
				case Error.Uninvited:
					return HttpStatusCode.Forbidden;
				case Error.LogNotFound:
				case Error.TermsNotFound:
				case Error.AccountNotFound:
				case Error.MoveNotFound:
					return HttpStatusCode.NotFound;
				default:
					return HttpStatusCode.BadRequest;
			}
		}
	}
}
