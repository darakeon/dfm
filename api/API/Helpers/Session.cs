using System;
using DFM.Authentication;
using DFM.Entities.Enums;
using Keon.MVC.Cookies;
using Microsoft.AspNetCore.Http;

namespace DFM.API.Helpers
{
	public class Session
	{
		private readonly GetContext getContext;
		private HttpContext context => getContext();
		private HttpRequest request => context.Request;

		public Session(GetContext getContext)
		{
			this.getContext = getContext;
		}

		public static String GetKey(GetContext getContext)
		{
			return new Session(getContext).GetTicket().Key;
		}

		public ClientTicket GetTicket(Boolean remember = false)
		{
			var type = TicketType.Mobile;
			var key = request.Headers["ticket"];
			return new ClientTicket(key, type);
		}
	}
}
