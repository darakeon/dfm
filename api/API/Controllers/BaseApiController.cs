﻿using System;
using System.Buffers;
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
                code = (int)error
            };

            Response.StatusCode = 400;
            return makeMvcActionResponse(result);
        }

        private JsonResult makeMvcActionResponse(object result)
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