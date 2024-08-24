using System;
using System.Threading.Tasks;
using DFM.Authentication;
using DFM.BusinessLogic;
using DFM.BusinessLogic.Exceptions;
using DFM.BusinessLogic.Response;
using DFM.Entities.Enums;
using DFM.Files;
using DFM.Generic;
using DFM.Generic.Datetime;
using DfM.Logs;
using DFM.Queue;

namespace DFM.Robot
{
	internal class Service : IDisposable
	{
		private readonly RobotTask task;
		private readonly ServiceAccess service;
		private readonly S3Service s3;
		private readonly SQSService sqs;

		public Service(RobotTask task)
		{
			this.task = task;

			TZ.Init(false);

			switch (this.task)
			{
				case RobotTask.Wipe:
					s3 = new S3Service();
					break;
				case RobotTask.Import:
				case RobotTask.Requeue:
					sqs = new SQSService();
					break;
			}

			service = new ServiceAccess(getTicket, getSite, s3, sqs);

			if (this.task != RobotTask.Check)
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

		public async Task Execute()
		{
			switch (task)
			{
				case RobotTask.Check:
					Console.WriteLine("Sunariom");
					break;

				case RobotTask.Schedules:
					var userErrors = service.Robot.RunSchedule();
					handleScheduleErrors(userErrors);
					break;

				case RobotTask.Wipe:
					service.Robot.WipeUsers();
					break;

				case RobotTask.Import:
					MoveResult result;
					Int32 count = 0;
					do
					{
						result = await service.Robot.MakeMoveFromImported();
						count++;
					} while (result != null && count < 10);

					break;

				case RobotTask.Finish:
					service.Robot.FinishArchives();
					break;

				case RobotTask.Requeue:
					await service.Robot.RequeueLines();
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
			sqs?.Dispose();
		}
	}
}
