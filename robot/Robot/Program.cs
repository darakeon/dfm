using System;
using DFM.Authentication;
using DFM.BusinessLogic;
using DFM.Entities.Enums;

namespace DFM.Robot
{
	class Program
	{
		public static void Main()
		{
			Connection.Run(() =>
			{
				var services = new ServiceAccess(getTicket, getSite);

				services.Robot.RunSchedule();

				Console.WriteLine("Hello World!");
			});
		}

		private static ClientTicket getTicket(Boolean remember)
		{
			return new ClientTicket("ROBOT", TicketType.Local);
		}

		private static string getSite()
		{
			return "https://dontflymoney.com";
		}
	}
}
