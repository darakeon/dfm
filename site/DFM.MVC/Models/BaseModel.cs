using System;
using System.Web;
using System.Web.Mvc;
using DFM.Authentication;
using DFM.BusinessLogic;
using DFM.BusinessLogic.Services;

namespace DFM.MVC.Models
{
	public class BaseModel
	{
		private static ServiceAccess access;

		private static ServiceAccess getOrCreateAccess()
		{
			return access ?? (access = new ServiceAccess());
		}

		protected static AdminService Admin => getOrCreateAccess().Admin;
		protected static MoneyService Money => getOrCreateAccess().Money;
		protected static ReportService Report => getOrCreateAccess().Report;
		protected static RobotService Robot => getOrCreateAccess().Robot;
		protected static SafeService Safe => getOrCreateAccess().Safe;

		protected static Current Current => getOrCreateAccess().Current;

		public DateTime Today => 
			Current.User?.Now().Date ?? DateTime.UtcNow;

		public static UrlHelper Url => new UrlHelper(HttpContext.Current.Request.RequestContext);
	}
}