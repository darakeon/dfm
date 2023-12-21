using System;
using DFM.Authentication;
using DFM.Entities.Enums;
using Keon.MVC.Cookies;
using Microsoft.AspNetCore.Http;

namespace DFM.MVC.Helpers
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
			var type = TicketType.Browser;
			var key = BrowserId.Get(() => context, remember);
			return new ClientTicket(key, type);
		}
	}
}
