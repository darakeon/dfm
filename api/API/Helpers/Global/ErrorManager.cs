using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DFM.API.Helpers.Extensions;
using DFM.API.Starters.Routes;
using DFM.Email;
using DfM.Logs;
using Keon.MVC.Cookies;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;

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
			var user = context.User.Identity;
			var request = context.Request;

			var parameters = request.GetSafeFields();

			var urlReferrer = request.Headers["Referer"].ToString();

			var authenticated = user?.IsAuthenticated ?? false;

			EmailSent = Error.SendReport(
				exception,
				request.GetDisplayUrl(),
				urlReferrer,
				request.Method,
				parameters,
				authenticated ? user.Name : "Off"
			);
		}

		private void redirect()
		{
			context.Response.Redirect(
				Route.GetUrl<Apis.Main>
					("Ops", "Code", 500)
			);
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
