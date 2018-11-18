using System;
using System.Web;
using System.Web.Mvc;
using DFM.Entities.Enums;
using DFM.MVC.Controllers;
using DK.Generic.Collection;
using DK.MVC.Route;

namespace DFM.MVC.Models
{
	public class BaseSiteModel : BaseModel
	{
		public Boolean IsAuthenticated => current.IsAuthenticated;
		public Boolean IsLastContractAccepted => safe.IsLastContractAccepted();

		public Boolean IsUsingCategories => isUsingCategories;

		public BootstrapTheme Theme => theme;
		public String Language => language;

		public Boolean ShowWizard => config?.Wizard ?? false;

		public static UrlHelper Url => new UrlHelper(HttpContext.Current.Request.RequestContext);

		public String ActionName => RouteInfo.Current["action"] ?? String.Empty;
		public String ControllerName => RouteInfo.Current["controller"] ?? String.Empty;

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
			var name = typeof(T).Name;
			return name.Substring(0, name.Length - 10);
		}
	}
}
