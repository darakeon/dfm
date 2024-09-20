using System;
using DFM.BusinessLogic;
using DFM.Files;
using DFM.Generic;
using DFM.Queue;
using Keon.MVC.Cookies;
using Microsoft.AspNetCore.Http;

namespace DFM.API.Helpers
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
				Cfg.SQS.Local
					? new LocalQueueService()
					: new SQSService();

			Access = new ServiceAccess(
				new Session(getContext).GetTicket,
				getUrl,
				wipeFileService,
				exportFileService,
				queueService
			);
		}

		private static IFileService createFileService(StoragePurpose purpose)
		{
			return Cfg.S3.Local
				? new LocalFileService(purpose)
				: new S3Service(purpose);
		}

		private readonly GetContext getContext;
		private HttpContext context => getContext();
		private HttpRequest request => context.Request;

		public ServiceAccess Access;
		public Current Current => Access?.Current;


		private String getUrl()
		{
			var requestHost = request.Host.Value.Replace("api.", "");
			return $"{request.Scheme}://{requestHost}";
		}
	}
}
