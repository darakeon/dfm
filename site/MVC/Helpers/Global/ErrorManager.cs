using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DFM.BusinessLogic;
using Keon.MVC.Cookies;
using DFM.Email;
using DfM.Logs;
using DFM.MVC.Helpers.Extensions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using DFM.MVC.Starters.Routes;
using Microsoft.AspNetCore.Http.Extensions;

namespace DFM.MVC.Helpers.Global
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
			try
			{
				var exception = context.Features
					.Get<IExceptionHandlerFeature>()
					.Error;

				exception.TryLog();

				sendEmail(exception);
			}
			catch (Exception e)
			{
				e.TryLog();
			}

			redirect();
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

			EmailSent = Error.SendReport(
				exception,
				request.GetDisplayUrl(),
				urlReferrer,
				request.Method,
				parameters,
				identification
			);
		}

		private void redirect()
		{
			var error500Url = Route.GetUrl<Default.Main>("Ops", "Code", 500);
			if (context.Request.Path != error500Url)
				context.Response.Redirect(error500Url);
		}

		/// <summary>
		/// When its value is gotten, its emptied
		/// </summary>
		public Error.Status EmailSent
		{
			get
			{
				if (!errors.ContainsKey(key))
					return Error.Status.Empty;

				var result = errors[key];

				errors[key] = Error.Status.Empty;

				return result;
			}
			private set
			{
				if (!errors.ContainsKey(key))
					errors.Add(key, value);
				else
					errors[key] = value;
			}
		}
	}
}
