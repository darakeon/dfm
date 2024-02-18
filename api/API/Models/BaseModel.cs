using System;
using DFM.API.Helpers.Extensions;
using DFM.API.Helpers.Global;
using DFM.API.Starters;
using DFM.BusinessLogic;
using DFM.BusinessLogic.Services;
using DFM.Generic;
using Microsoft.AspNetCore.Http;

namespace DFM.API.Models
{
	public abstract class BaseModel
	{
		public static Boolean IsDev { get; set; }

		private static HttpContext context => Context.Accessor.HttpContext;

		protected Translator translator => context.GetTranslator();

		protected static ServiceAccess service => context.GetService().Access;

		protected AuthService auth => service.Auth;
		protected LawService law => service.Law;
		protected AdminService admin => service.Admin;
		protected ClipService clip => service.Clip;
		protected MoneyService money => service.Money;
		protected ReportService report => service.Report;
		protected RobotService robot => service.Robot;
		protected OutsideService outside => service.Outside;

		protected Current current => service.Current;

		protected DateTime now => current.Now;
		protected Theme theme => current.Theme;
		protected String language => translator.Language;

		protected Boolean isUsingCategories => current.UseCategories;
		protected Boolean isUsingAccountsSigns => current.UseAccountsSigns;
		protected Boolean moveCheckingEnabled => current.MoveCheck;

		protected void logout()
		{
			current.Clear();
		}
	}
}
