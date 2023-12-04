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

namespace DFM.API.Helpers.Global
{
	public class ErrorManager
	{
		public static Task Process(HttpContext context)
		{
			var manager = new ErrorManager(context);
			return Task.Run(manager.process);
		}

		private readonly HttpContext context;
		private String key => BrowserId.Get(() => context);

		private static readonly
			IDictionary<String, Error.Status> errors
				= new Dictionary<String, Error.Status>();

		public ErrorManager(HttpContext context)
		{
			this.context = context;
		}

		private void process()
		{
			var errorStatus = Error.Status.Empty;

			try
			{
				var exception = context.Features
					.Get<IExceptionHandlerFeature>()
					.Error;

				exception.TryLog();

				errorStatus = sendEmail(exception);
			}
			catch (Exception e)
			{
				e.TryLog();
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
