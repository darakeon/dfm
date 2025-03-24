using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DFM.API.Helpers.Extensions;
using DFM.Email;
using DfM.Logs;
using Keon.MVC.Cookies;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Newtonsoft.Json;
using DFM.Logs;

namespace DFM.API.Helpers.Global
{
	public class ErrorManager(HttpContext context, ILogService logService)
	{
		public static Task Process(HttpContext context, ILogService logService)
		{
			var manager = new ErrorManager(context, logService);
			return Task.Run(manager.process);
		}

		private String key => BrowserId.Get(() => context);

		private static readonly
			IDictionary<String, Error.Status> errors
				= new Dictionary<String, Error.Status>();

		private void process()
		{
			var errorStatus = Error.Status.Empty;

			try
			{
				var exception = context.Features
					.Get<IExceptionHandlerFeature>()
					.Error;

				logService.Log(exception);

				errorStatus = sendEmail(exception);
			}
			catch (Exception e)
			{
				logService.Log(e);
			}

			makeResponse(errorStatus);
		}

		private Error.Status sendEmail(Exception exception)
		{
			var user = context.User.Identity;
			var request = context.Request;

			var parameters = request.GetSafeFields();

			var urlReferrer = request.Headers["Referer"].ToString();

			var authenticated = user?.IsAuthenticated ?? false;

			return Error.SendReport(
				logService,
				exception,
				request.GetDisplayUrl(),
				urlReferrer,
				request.Method,
				parameters,
				authenticated ? user.Name : "Off"
			);
		}

		private void makeResponse(Error.Status errorStatus)
		{
			context.Response.StatusCode = 500;

			var response = new
			{
				emailSent = errorStatus == Error.Status.Sent
			};

			var json = JsonConvert.SerializeObject(response);

			context.Response.WriteAsJsonAsync(response);
		}
	}
}
