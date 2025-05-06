using System;
using DFM.Authentication;
using DFM.Generic;
using DFM.Generic.Settings;
using Keon.MVC.Cookies;
using Microsoft.AspNetCore.Http;

namespace DFM.BaseWeb.Helpers
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
			var type = Cfg.TicketType;

			var key = type == TicketType.Browser
				? BrowserId.Get(() => context, remember)
				: request.Headers["ticket"].ToString();

			return new ClientTicket(key, type);
		}
	}
}
