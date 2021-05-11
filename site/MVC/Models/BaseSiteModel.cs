using System;
using System.Linq.Expressions;
using DFM.Generic;
using DFM.MVC.Controllers;
using Keon.Util.Collection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace DFM.MVC.Models
{
	[ValidateNever]
	public class BaseSiteModel : BaseModel
	{
		public Boolean IsAuthenticated => current.IsAuthenticated;
		public Boolean IsLastContractAccepted => safe.IsLastContractAccepted();

		public Boolean IsUsingCategories => isUsingCategories;

		public Theme Theme => theme;
		public ThemeColor Color => theme.Color();
		public ThemeBrightness Brightness => theme.Brightness();
		public String Language => language;

		public Boolean ShowWizard => wizard;

		public DateTime Now => current.Now;

		public String ActionName => route?["action"];
		public String ControllerName => route?["controller"];

		public Boolean IsExternal
		{
			get
			{
				var usersControllerUrl = getControllerUrl<UsersController>();
				var tokensControllerUrl = getControllerUrl<TokensController>();
				var opsControllerUrl = getControllerUrl<OpsController>();

				return ControllerName.IsIn(usersControllerUrl, tokensControllerUrl, opsControllerUrl);
			}
		}

		private static String getControllerUrl<T>()
			where T : Controller
		{
			var name = typeof(T).Name;
			return name[..^10];
		}

		protected String getName<T>(Expression<Func<T, Object>> prop)
		{
			return prop.Body.ToString().Substring(2);
		}
	}
}
