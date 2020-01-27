using System;
using DFM.Authentication;
using DFM.BusinessLogic;
using DFM.BusinessLogic.Helpers;
using DFM.MVC.Starters.Routes;
using Keon.MVC.Cookies;
using Microsoft.AspNetCore.Http;
using TicketType = DFM.Entities.Enums.TicketType;

namespace DFM.MVC.Helpers
{
	public class Service
	{
		public Service(GetContext getContext)
		{
			this.getContext = getContext;
			Access = new ServiceAccess(getTicket, getPath, getUrl);
		}

		private readonly GetContext getContext;
		private HttpContext context => getContext();
		private HttpRequest request => context.Request;

		public ServiceAccess Access;
		public Current Current => Access?.Current;

		private ClientTicket getTicket(Boolean remember)
		{
			var type = request.Path.Value
				.ToLower().StartsWith("/api")
				? TicketType.Mobile
				: TicketType.Browser;

			var key = getKey(type, remember);

			return new ClientTicket(key, type);
		}

		private String getKey(TicketType type, Boolean remember)
		{
			return type switch
			{
				TicketType.Browser => BrowserId.Get(() => context, remember),
				TicketType.Mobile => request.Headers["ticket"],
				TicketType.Local => throw new NotImplementedException(),
				_ => throw new NotImplementedException()
			};
		}

		private String getPath(PathType pathType)
		{
			return Route.GetUrl<RouteDefault>
				("Tokens", getAction(pathType));
		}

		private String getAction(PathType pathType)
		{
			return pathType switch
			{
				PathType.PasswordReset => "PasswordReset",
				PathType.UserVerification => "UserVerification",
				PathType.DisableToken => "Disable",
				_ => throw new NotImplementedException()
			};
		}

		private String getUrl()
		{
			return $"{request.Scheme}://{request.Host}";
		}
	}
}
