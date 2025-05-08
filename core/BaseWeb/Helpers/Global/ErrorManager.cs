using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DFM.BaseWeb.Helpers.Extensions;
using DFM.Email;
using DFM.Logs;
using Keon.MVC.Cookies;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;

namespace DFM.BaseWeb.Helpers.Global
{
	public class ErrorManager
	{
		private ErrorManager(
			HttpContext context,
			ILogService logService,
			String? redirectUrl
		)
		{
			this.context = context;
			this.logService = logService;
			this.redirectUrl = redirectUrl;
		}


		public static Task Process(
			HttpContext context,
			ILogService logService,
			String? redirect = null
		)
		{
			var manager = new ErrorManager(context, logService, redirect);
			return Task.Run(manager.process);
		}


		private String key => getKey(context);

		private static String getKey(HttpContext context)
		{
			return BrowserId.Get(() => context);
		}


		private static readonly
			IDictionary<String, Error.Status> errors
				= new Dictionary<String, Error.Status>();

		private readonly HttpContext context;
		private readonly ILogService logService;
		private readonly String? redirectUrl;


		private void process()
		{
			try
			{
				var exception = context.Features
					.Get<IExceptionHandlerFeature>()
					.Error;

				logService.Log(exception);

				sendEmail(exception);
			}
			catch (Exception e)
			{
				logService.Log(e);
			}

			if (redirectUrl == null)
				makeResponse();
			else if (context.Request.Path != redirectUrl)
				context.Response.Redirect(redirectUrl);
		}


		private void sendEmail(Exception exception)
		{
			var request = context.Request;

			var parameters = request.GetSafeFields();

			var urlReferrer = request.Headers["Referer"].ToString();

			var services = new Service(() => context);
			var current = services.Current;
			var authenticated = current.IsAuthenticated;

			var identification =
				authenticated
					? current.SafeTicketKey
					: "Off";

			errors[key] = Error.SendReport(
				logService,
				exception,
				request.GetDisplayUrl(),
				urlReferrer,
				request.Method,
				parameters,
				identification
			);
		}


		private void makeResponse()
		{
			context.Response.StatusCode = 500;

			var response = new
			{
				emailSent = errors[key] == Error.Status.Sent
			};

			context.Response.WriteAsJsonAsync(response);
		}


		public static Error.Status GetEmailSent(HttpContext context)
		{
			var key = getKey(context);

			if (!errors.ContainsKey(key))
				return Error.Status.Empty;

			var result = errors[key];

			errors[key] = Error.Status.Empty;

			return result;
		}
	}
}
