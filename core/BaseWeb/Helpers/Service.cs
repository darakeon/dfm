using System;
using DFM.BusinessLogic;
using DFM.Files;
using DFM.Generic;
using DFM.Logs;
using DFM.Queue;
using Keon.MVC.Cookies;
using Microsoft.AspNetCore.Http;

namespace DFM.BaseWeb.Helpers
{
	public class Service
	{
		public Service(GetContext getContext)
		{
			this.getContext = getContext;

			var wipeFileService =
				createFileService(StoragePurpose.Wipe);

			var exportFileService =
				createFileService(StoragePurpose.Export);

			IQueueService queueService =
				Cfg.Queue.Local
					? new LocalQueueService()
					: new SQSService();

			LogService = LogFactory.Service;

			Access = new ServiceAccess(
				new Session(getContext).GetTicket,
				getUrl,
				LogService,
				wipeFileService,
				exportFileService,
				queueService
			);
		}

		private static IFileService createFileService(StoragePurpose purpose)
		{
			return Cfg.Storage.Local
				? new LocalFileService(purpose)
				: new S3Service(purpose);
		}

		private readonly GetContext getContext;
		private HttpContext context => getContext();
		private HttpRequest request => context.Request;

		public readonly ServiceAccess Access;
		public readonly ILogService LogService;
		public Current Current => Access?.Current;


		private String getUrl()
		{
			var requestHost = request.Host.Value.Replace("api.", "");
			return $"{request.Scheme}://{requestHost}";
		}
	}
}
