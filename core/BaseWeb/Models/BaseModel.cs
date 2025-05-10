using System;
using System.Collections.Generic;
using DFM.BaseWeb.Extensions;
using DFM.BaseWeb.Languages;
using DFM.BaseWeb.Starters;
using DFM.BusinessLogic;
using DFM.BusinessLogic.Services;
using DFM.Generic;
using DFM.Logs;
using Microsoft.AspNetCore.Http;
using Service = DFM.BaseWeb.BusinessLogic.Service;

namespace DFM.BaseWeb.Models
{
	public abstract class BaseModel
	{
		public static Boolean IsDev => Cfg.IsDev;

		protected static HttpContext context => Context.Accessor.HttpContext;

		protected Translator translator => context.GetTranslator();
		protected Dictionary<String, String> route => context.GetRouteText();

		private Service contextService => context.GetService();
		private ServiceAccess service => contextService.Access;

		protected AuthService auth => service.Auth;
		protected LawService law => service.Law;
		protected AdminService admin => service.Admin;
		protected ClipService clip => service.Clip;
		protected MoneyService money => service.Money;
		protected ReportService report => service.Report;
		protected OutsideService outside => service.Outside;
		protected AttendantService attendant => service.Attendant;

		protected ILogService logService => contextService.LogService;

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
		protected Boolean tfaForgottenWarning => current.TFAForgottenWarning;

		protected void logout()
		{
			current.Clear();
		}
	}
}
