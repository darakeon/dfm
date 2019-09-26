using System;
using System.Web.Mvc;
using DFM.MVC.Areas.Account.Controllers;
using DFM.MVC.Helpers.Global;

namespace DFM.MVC.Areas.Account
{
	// ReSharper disable once UnusedMember.Global
	public class AccountAreaRegistration : AreaRegistration
	{
		public override String AreaName => RouteNames.Account;

		public override void RegisterArea(AreaRegistrationContext context)
		{
			context.MapRoute(
				RouteNames.Account,
				"Account/{accountUrl}/{controller}/{action}/{id}",
				new { controller = "Reports", action = "Index", id = UrlParameter.Optional },
				new[] { typeof(ReportsController).Namespace }
			);
		}
	}
}
