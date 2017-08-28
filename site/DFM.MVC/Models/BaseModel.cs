using System;
using System.Web;
using System.Web.Mvc;
using DFM.Authentication;
using DFM.BusinessLogic.Services;
using DFM.Entities.Enums;
using DFM.MVC.Helpers;

namespace DFM.MVC.Models
{
	public class BaseModel
	{
		protected AdminService Admin => Service.Access.Admin;
		protected MoneyService Money => Service.Access.Money;
		protected ReportService Report => Service.Access.Report;
		protected RobotService Robot => Service.Access.Robot;
		protected SafeService Safe => Service.Access.Safe;

		protected Current Current => Service.Access.Current;

		public DateTime Today => Current.User?.Now().Date ?? DateTime.UtcNow;

		public static UrlHelper Url => new UrlHelper(HttpContext.Current.Request.RequestContext);

		public BootstrapTheme Theme => BootstrapTheme.Cyborg;
	}
}