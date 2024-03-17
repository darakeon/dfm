using System;
using System.Buffers;
using System.Net;
using System.Text;
using DFM.API.Helpers.Controllers;
using DFM.API.Helpers.Extensions;
using DFM.API.Models;
using DFM.BusinessLogic.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DFM.API.Controllers
{
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

        [HttpGetAndHead]
        public JsonResult Uninvited()
        {
	        return error(Error.Uninvited);
        }

        [HttpGetAndHead]
        public JsonResult LoginRequested()
        {
	        return error(Error.LoginRequested);
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

        protected JsonResult error(Error error)
        {
            var result = new ResponseModel(
	            (Int32)error, 
	            HttpContext.Translate(error)
	        );

            Response.StatusCode = (Int32) getStatusCode(error);
            return Json(result);
        }

        private HttpStatusCode getStatusCode(Error error)
        {
	        switch (error)
	        {
		        case Error.LoginRequested:
			        return HttpStatusCode.Unauthorized;
		        case Error.Uninvited:
			        return HttpStatusCode.Forbidden;
				case Error.LogNotFound:
				case Error.TermsNotFound:
					return HttpStatusCode.NotFound;
		        default:
			        return HttpStatusCode.BadRequest;
	        }
	    }

        protected T getFromBody<T>()
        {
            var body = Request.BodyReader.ReadAsync();
            var json = Encoding.UTF8.GetString(body.Result.Buffer.ToArray());

            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
