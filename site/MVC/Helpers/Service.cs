using System;
using DFM.BusinessLogic;
using Keon.MVC.Cookies;
using Microsoft.AspNetCore.Http;

namespace DFM.MVC.Helpers
{
	public class Service
	{
		public Service(GetContext getContext)
		{
			this.getContext = getContext;
			Access = new ServiceAccess(
				new Session(getContext).GetTicket,
				getUrl
			);
		}

		private readonly GetContext getContext;
		private HttpContext context => getContext();
		private HttpRequest request => context.Request;

		public ServiceAccess Access;
		public Current Current => Access?.Current;


		private String getUrl()
		{
			return $"{request.Scheme}://{request.Host}";
		}
	}
}
