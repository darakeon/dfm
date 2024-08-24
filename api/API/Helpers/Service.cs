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

			IFileService fileService =
				Cfg.S3.Local
					? new LocalFileService()
					: new S3Service();

			IQueueService queueService =
				Cfg.SQS.Local
					? new LocalQueueService()
					: new SQSService();

			Access = new ServiceAccess(
				new Session(getContext).GetTicket,
				getUrl,
				fileService,
				queueService
			);
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
