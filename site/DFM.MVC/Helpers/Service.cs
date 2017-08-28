using System;
using System.Web;
using DFM.Authentication;
using DFM.BusinessLogic;
using DK.MVC.Cookies;
using DK.MVC.Route;
using TicketType = DFM.Entities.Enums.TicketType;

namespace DFM.MVC.Helpers
{
	public class Service
	{
		public static ServiceAccess Access = new ServiceAccess(getTicket);
		public static Current Current => Access?.Current;


		private static TypedTicket getTicket(Boolean? remember = null)
		{
			var url = HttpContext.Current.Request.Url;

			var type = url.AbsolutePath.StartsWith("/Api")
				? TicketType.Mobile
				: TicketType.Browser;

			return new TypedTicket(getKey(type, remember), type);
		}


		private static String getKey(TicketType type, Boolean? remember)
		{
			switch (type)
			{
				case TicketType.Browser:
					return BrowserId.Get(remember);
				
				case TicketType.Mobile:
					return RouteInfo.Current["ticket"];

				default:
					throw new NotImplementedException();
			}
		}


	}

}