using System;
using DFM.Authentication;
using DFM.BusinessLogic;
using DFM.Entities.Enums;
using DFM.Generic;
using DFM.Generic.Datetime;
using DfM.Logs;

namespace DFM.Robot
{
	class Program
	{
		public static void Main()
		{
			Connection.Run(execute);
		}

		private static void execute()
		{
			TZ.Init(false);

			var services = new ServiceAccess(getTicket, getSite);

			services.Current.Set(Cfg.RobotEmail, Cfg.RobotPassword, false);

			var userErrors = services.Robot.RunSchedule();

			foreach (var (email, errors) in userErrors)
			{
				foreach (var error in errors)
				{
					error.TryLogHandled($"User: {email}");
				}
			}
		}

		private static ClientTicket getTicket(Boolean remember)
		{
			return new("ROBOT", TicketType.Local);
		}

		private static string getSite()
		{
			return "https://dontflymoney.com";
		}
	}
}
