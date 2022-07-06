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
			var path = (request.Path.Value ?? "").ToLower();

			var type = path.StartsWith("/api")
				? TicketType.Mobile
				: TicketType.Browser;

			var key = getKey(type, remember);

			return new ClientTicket(key, type);
		}

		private String getKey(TicketType type, Boolean remember)
		{
			return type switch
			{
				TicketType.Browser =>
					BrowserId.Get(() => context, remember),

				TicketType.Mobile =>
					request.Headers["ticket"],

				_ => throw new NotImplementedException()
			};
		}
	}
}
