using System;
using System.Web;
using System.Web.Mvc;
using DFM.Authentication;
using DFM.BusinessLogic.Helpers;
using DFM.BusinessLogic.Services;
using DFM.Entities.Enums;
using DFM.MVC.Controllers;
using DFM.MVC.Helpers;
using DK.Generic.Collection;
using DK.MVC.Route;

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

		public static UrlHelper Url => new UrlHelper(HttpContext.Current.Request.RequestContext);


		public Boolean IsAuthenticated => Current.IsAuthenticated;
		public Boolean IsLastContractAccepted => Safe.IsLastContractAccepted();
		

		public Boolean IsExternal
		{
			get
			{
				var usersControllerUrl = getControllerUrl<UsersController>();
				var tokensControllerUrl = getControllerUrl<TokensController>();
				var opsControllerUrl = getControllerUrl<OpsController>();

				var controller = RouteInfo.Current["controller"];

				return controller.IsIn(usersControllerUrl, tokensControllerUrl, opsControllerUrl);
			}
		}

		private static string getControllerUrl<T>()
			where T : Controller
		{
			var name = typeof (T).Name;
			return name.Substring(0, name.Length - 10);
		}


		public Boolean UseCategories => Current.User?.Config?.UseCategories ?? false;

		public DateTime Today => Current.User?.Now().Date ?? DateTime.UtcNow;

		public BootstrapTheme UserTheme => Current.User?.Config?.Theme ?? Defaults.DEFAULT_THEME;

		public Boolean ShowWizard => Current.User?.Config?.Wizard ?? false;

	}
}
