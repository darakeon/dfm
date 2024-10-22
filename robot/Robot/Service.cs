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
		private readonly S3Service wipeS3;
		private readonly S3Service exportS3;
		private readonly SQSService sqs;

		public Service(RobotTask task)
		{
			this.task = task;

			TZ.Init(false);

			switch (this.task)
			{
				case RobotTask.Wipe:
				case RobotTask.Export:
				case RobotTask.Expire:
					wipeS3 = new S3Service(StoragePurpose.Wipe);
					exportS3 = new S3Service(StoragePurpose.Export);
					break;
				case RobotTask.Import:
				case RobotTask.Requeue:
					sqs = new SQSService();
					break;
			}

			service = new ServiceAccess(
				getTicket, getSite, wipeS3, exportS3, sqs
			);

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

		public async Task Execute(Action<object> logOk)
		{
			switch (task)
			{
				case RobotTask.Check:
					logOk("Sunariom");
					break;

				case RobotTask.Schedules:
					var userErrors = service.Executor.RunSchedule();
					handleScheduleErrors(userErrors);
					break;

				case RobotTask.Wipe:
					service.Executor.WipeUsers();
					break;

				case RobotTask.Import:
					MoveImportResult result;
					Int32 count = 0;
					do
					{
						result = await service.Executor.MakeMoveFromImported();
						handleImportErrors(result);
						count++;
					} while (result != null && count < 100);

					break;

				case RobotTask.Finish:
					service.Executor.FinishArchives();
					break;

				case RobotTask.Requeue:
					await service.Executor.RequeueLines();
					break;

				case RobotTask.Export:
					service.Executor.ExportOrder();
					break;

				case RobotTask.Expire:
					service.Executor.DeleteExpiredOrders();
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

		private void handleImportErrors(MoveImportResult result)
		{
			if (!result.Success)
				result.Error.TryLogHandled($"User: {result.User.Email}");
		}

		public void Dispose()
		{
			wipeS3?.Dispose();
			exportS3?.Dispose();
			sqs?.Dispose();
		}
	}
}
