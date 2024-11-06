using System;
using System.Collections.Generic;
using DFM.BusinessLogic;
using DFM.BusinessLogic.Services;
using DFM.Generic;
using DFM.MVC.Helpers.Extensions;
using DFM.MVC.Helpers.Global;
using DFM.MVC.Starters;
using Microsoft.AspNetCore.Http;

namespace DFM.MVC.Models
{
	public abstract class BaseModel
	{
		public static Boolean IsDev { get; set; }

		private static HttpContext context => Context.Accessor.HttpContext;

		protected Translator translator => context.GetTranslator();
		protected ErrorAlert errorAlert => context.GetErrorAlert();
		protected Dictionary<String, String> route => context.GetRouteText();

		private static ServiceAccess service => context.GetService().Access;

		protected static AuthService auth => service.Auth;
		protected static LawService law => service.Law;
		protected static AdminService admin => service.Admin;
		protected static ClipService clip => service.Clip;
		protected static MoneyService money => service.Money;
		protected static ReportService report => service.Report;
		protected static OutsideService outside => service.Outside;
		protected static AttendantService attendant => service.Attendant;

		protected Current current => service.Current;

		protected DateTime now => current.Now;
		protected Theme theme => current.Theme;
		protected String language => translator.Language;
		protected Boolean wizard => current.Wizard;

		protected Boolean isUsingCategories => current.UseCategories;
		protected Boolean isUsingAccountsSigns => current.UseAccountsSigns;
		protected Boolean moveCheckingEnabled => current.MoveCheck;
		protected Boolean isUsingCurrency => current.UseCurrency;
		
		protected Int32 planLimitMoveDetail => current.PlanLimitMoveDetail;

		protected void logout()
		{
			current.Clear();
		}
	}
}
