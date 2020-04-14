using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Keon.MVC.Cookies;
using DFM.Email;
using DFM.Generic;
using DFM.MVC.Helpers.Extensions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using DFM.MVC.Starters.Routes;

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
		private Exception exception { get; set; }
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
				sendEmail();
			}
			catch (Exception e)
			{
				exception?.TryLog();
				e.TryLog();
			}

			redirect();
		}

		private void sendEmail()
		{
			exception = context.Features
				.Get<IExceptionHandlerFeature>()
				.Error;

			var user = context.User.Identity;
			var request = context.Request;

			var parameters = request.GetSafeFields();

			var urlReferrer = request.Headers["Referer"].ToString();

			EmailSent = Error.SendReport(
				exception,
				request.Host.ToString(),
				urlReferrer,
				request.Method,
				parameters,
				user.IsAuthenticated ? user.Name : "Off"
			);
		}

		private void redirect()
		{
			context.Response.Redirect(
				Route.GetUrl<RouteDefault>
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
