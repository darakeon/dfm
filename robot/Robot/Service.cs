using System;
using DFM.Authentication;
using DFM.BusinessLogic;
using DFM.BusinessLogic.Exceptions;
using DFM.Entities.Enums;
using DFM.Exchange;
using DFM.Generic;
using DFM.Generic.Datetime;
using DfM.Logs;

namespace DFM.Robot
{
	internal class Service : IDisposable
	{
		private readonly Task task;
		private readonly ServiceAccess service;
		private readonly S3 s3;

		public Service(String task)
		{
			this.task = EnumX.Parse<Task>(task);

			TZ.Init(false);

			s3 = this.task == Task.Wipe ? new S3() : null;

			service = new ServiceAccess(getTicket, getSite, s3);

			if (this.task != Task.Check)
				service.Current.Set(Cfg.RobotEmail, Cfg.RobotPassword, false);
		}

		private static ClientTicket getTicket(Boolean remember)
		{
			return new("ROBOT", TicketType.Local);
		}

		private static String getSite()
		{
			return "https://dontflymoney.com";
		}

		public void Execute()
		{
			switch (task)
			{
				case Task.Check:
					Console.WriteLine("Sunariom");
					break;

				case Task.Schedules:
					var userErrors = service.Robot.RunSchedule();
					handleScheduleErrors(userErrors);
					break;

				case Task.Wipe:
					service.Robot.WipeUsers();
					break;
			}
		}

		private void handleScheduleErrors(DicList<CoreError> userErrors)
		{
			foreach (var (email, errors) in userErrors)
			{
				foreach (var error in errors)
				{
					error.TryLogHandled($"User: {email}");
				}
			}
		}

		public void Dispose()
		{
			s3?.Dispose();
		}
	}
}