using System;
using DFM.BusinessLogic.Exceptions;
using DFM.Email;
using DfM.Logs;
using DFM.MVC.Helpers;
using DFM.MVC.Helpers.Extensions;
using DFM.MVC.Helpers.Global;
using error = DFM.BusinessLogic.Exceptions.Error;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace DFM.MVC.Starters
{
	class Robot
	{
		public static void Run(IApplicationBuilder app)
		{
			app.Use<Robot>(async (context, next) =>
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

			if (!canRunSchedule) return;

			try
			{
				var emailsStatus = access.Robot.RunSchedule();

				if (emailsStatus.IsWrong())
				{
					addError(context, translator =>
					{
						var message = translator["ScheduleRun"];
						var error = translator[emailsStatus].ToLower();
						return String.Format(message, error);
					});
				}
			}
			catch (CoreError e)
			{
				addError(context, t => t[e.Type]);
			}
			catch (Exception e)
			{
				e.TryLogHandled("Error on running robot");
				addError(context, t => t[error.Unknown]);
			}
		}

		private static void addError(HttpContext context, Func<Translator, String> mainError)
		{
			var translator = context.GetTranslator();
			var final = mainError(translator);

			var errorAlert = context.GetErrorAlert();
			errorAlert.AddTranslated(final);
		}
	}
}
