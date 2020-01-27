using System;
using DFM.Email;
using DFM.MVC.Helpers;
using DFM.MVC.Helpers.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;

namespace DFM.MVC.Starters
{
	class Robot
	{
		public static void Run(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime life)
		{
			app.Use(async (context, next) =>
			{
				var service = context.GetService();
				var current = service.Current;

				if (current.IsAuthenticated)
					runUserSchedules(service, context);
	
				await next();
			});
		}

		private static void runUserSchedules(Service service, HttpContext context)
		{
			var access = service.Access;

			var canRunSchedule =
				access.Safe.IsLastContractAccepted()
				&& access.Safe.VerifyTicketTFA();

			if (canRunSchedule)
			{
				var emailsStatus = access.Robot.RunSchedule();

				if (emailsStatus.IsWrong())
					addError(context, emailsStatus);
			}
		}

		private static void addError(HttpContext context, EmailStatus emailsStatus)
		{
			var translator = context.GetTranslator();

			var message = translator["ScheduleRun"];
			var error = translator[emailsStatus].ToLower();
			var final = String.Format(message, error);

			var errorAlert = context.GetErrorAlert();
			errorAlert.AddTranslated(final);
		}
	}
}
