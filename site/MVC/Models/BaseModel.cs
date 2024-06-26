﻿using System;
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
		protected Boolean wizard => current.Wizard;

		protected Boolean isUsingCategories => current.UseCategories;
		protected Boolean isUsingAccountsSigns => current.UseAccountsSigns;
		protected Boolean moveCheckingEnabled => current.MoveCheck;
		protected Boolean isUsingCurrency => current.UseCurrency;

		protected void logout()
		{
			current.Clear();
		}
	}
}
