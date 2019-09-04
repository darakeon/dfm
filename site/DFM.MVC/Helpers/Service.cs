using System;
using System.Web;
using DFM.Authentication;
using DFM.BusinessLogic;
using DFM.BusinessLogic.Helpers;
using DFM.MVC.Helpers.Global;
using DFM.MVC.Models;
using Keon.MVC.Cookies;
using TicketType = DFM.Entities.Enums.TicketType;

namespace DFM.MVC.Helpers
{
	public class Service
	{
		public static ServiceAccess Access =
			new ServiceAccess(getTicket, getPath);

		public static Current Current => Access?.Current;

		private static HttpRequest request => HttpContext.Current.Request;

		private static String getPath(PathType pathType)
		{
			switch (pathType)
			{
				case PathType.PasswordReset:
					return BaseSiteModel.Url.RouteUrl(
						RouteNames.DEFAULT,
						new
						{
							action = "PasswordReset",
							controller = "Tokens"
						}
					);

				case PathType.UserVerification:
					return BaseSiteModel.Url.RouteUrl(
						RouteNames.DEFAULT,
						new
						{
							action = "UserVerification",
							controller = "Tokens"
						}
					);

				case PathType.DisableToken:
					return BaseSiteModel.Url.RouteUrl(
						RouteNames.DEFAULT,
						new
						{
							action = "Disable",
							controller = "Tokens"
						}
					);

				default:
					throw new NotImplementedException();
			}
		}

		private static ClientTicket getTicket(Boolean remember)
		{
			var type = request.Url.AbsolutePath
					.ToLower().StartsWith("/api")
				? TicketType.Mobile
				: TicketType.Browser;

			var key = getKey(type, remember);

			return new ClientTicket(key, type);
		}

		private static String getKey(TicketType type, Boolean remember)
		{
			switch (type)
			{
				case TicketType.Browser:
					return BrowserId.Get(remember);

				case TicketType.Mobile:
					return request.Headers["ticket"];

				default:
					throw new NotImplementedException();
			}
		}
	}
}
