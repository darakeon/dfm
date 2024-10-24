using System;
using System.Collections.Generic;
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
		private readonly Action<Object> logOk;
		
		private readonly ServiceAccess service;
		private readonly S3Service wipeS3;
		private readonly S3Service exportS3;
		private readonly SQSService sqs;

		private readonly IDictionary<RobotTask, Func<Task>> actions;

		public Service(RobotTask task, Action<object> logOk)
		{
			this.task = task;
			this.logOk = logOk;

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

			actions = new Dictionary<RobotTask, Func<Task>>
			{
				{ RobotTask.Check, check },
				{ RobotTask.Schedules, schedules },
				{ RobotTask.Wipe, wipe },
				{ RobotTask.Import, import },
				{ RobotTask.Finish, finish },
				{ RobotTask.Requeue, requeue },
				{ RobotTask.Export, export },
				{ RobotTask.Expire, expire },
			};
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
			await actions[task]();
		}

		private async Task check()
		{
			logOk("Sunariom");
		}

		private async Task schedules()
		{
			var userErrors = service.Executor.RunSchedule();

			foreach (var (email, errors) in userErrors)
			{
				foreach (var error in errors)
				{
					error.TryLogHandled($"User: {email}");
				}
			}
		}

		private async Task wipe()
		{
			service.Executor.WipeUsers();
		}

		private async Task import()
		{
			for(var m = 0; m < 100; m++)
			{
				var result = await service.Executor.MakeMoveFromImported();

				if (result == null)
					return;

				if (!result.Success)
					result.Error.TryLogHandled($"User: {result.User.Email}");
			}
		}

		private async Task finish()
		{
			service.Executor.FinishArchives();
		}

		private async Task requeue()
		{
			await service.Executor.RequeueLines();
		}

		private async Task export()
		{
			var result = service.Executor.ExportOrder();

			if (!result.Success)
				result.Error.TryLogHandled($"User: {result.User.Email}");
		}

		private async Task expire()
		{
			var results = service.Executor.DeleteExpiredOrders();

			foreach (var result in results)
			{
				if (!result.Success)
					result.Error.TryLogHandled($"User: {result.User.Email}");
			}
		}

		public void Dispose()
		{
			wipeS3?.Dispose();
			exportS3?.Dispose();
			sqs?.Dispose();
		}
	}
}
