using System;
using DFM.BusinessLogic;
using DFM.Exchange;
using DFM.Generic;
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

			Access = new ServiceAccess(
				new Session(getContext).GetTicket,
				getUrl,
				fileService
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
