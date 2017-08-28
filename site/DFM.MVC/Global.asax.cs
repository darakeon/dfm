using System;
using System.Globalization;
using System.IO;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using DK.Generic.Exceptions;
using DFM.Authentication;
using DFM.BusinessLogic;
using DFM.Email;
using DFM.Entities;
using DFM.Generic;
using DFM.MVC.Controllers;
using DFM.MVC.Helpers;
using DFM.MVC.Helpers.Global;
using DFM.Repositories.Mappings;
using DK.MVC.Cookies;
using DK.NHibernate.Base;
using log4net.Config;

namespace DFM.MVC
{
	public class MvcApplication : HttpApplication
	{
		private readonly Current current = Service.Current;
		private readonly ServiceAccess access = Service.Access;

		protected void Application_Start()
		{
			AreaRegistration.RegisterAllAreas();
			RouteConfig.RegisterRoutes(RouteTable.Routes);

			Directory.SetCurrentDirectory(Server.MapPath("~"));

			SessionFactoryManager.Initialize<UserMap, User>();
			MultiLanguage.Initialize();

			XmlConfigurator.Configure();

			if (isLocal)
				IP.SaveCurrent();
		}



		// ReSharper disable InconsistentNaming
		protected void Application_BeginRequest()
		// ReSharper restore InconsistentNaming
		{
			if (isAsset) return;

			if (isLocal)
				MultiLanguage.Initialize();

			SessionManager.Init(BrowserId.Get);
		}



		// ReSharper disable InconsistentNaming
		protected void Application_AuthenticateRequest()
		// ReSharper restore InconsistentNaming
		{
			if (isAsset) return;

			if (isElmah && !current.IsAdm)
				Response.Redirect("/");
		}



		// ReSharper disable InconsistentNaming
		protected void Application_AcquireRequestState()
		// ReSharper restore InconsistentNaming
		{
			if (isAsset || !current.IsAuthenticated)
				return;

			Thread.CurrentThread.CurrentUICulture = 
				new CultureInfo(Service.Current.Language);

			if (access.Safe.IsLastContractAccepted())
				runSchedule();
		}

		private void runSchedule()
		{
			var emailsStatus = access.Robot.RunSchedule();

			if (emailsStatus.IsWrong())
				setErrorMessage(emailsStatus);
		}

		private static void setErrorMessage(EmailStatus emailsStatus)
		{
			var message = MultiLanguage.Dictionary["ScheduleRun"];
			var error = MultiLanguage.Dictionary[emailsStatus].ToLower();
			var final = String.Format(message, error);

			ErrorAlert.AddTranslated(final);
		}



		// ReSharper disable InconsistentNaming
		protected void Application_Error()
		// ReSharper restore InconsistentNaming
		{
			if (!isLocal)
				ErrorManager.SendEmail();

			closeSession();
		}


		// ReSharper disable InconsistentNaming
		protected void Application_PostRequestHandlerExecute(object sender, EventArgs e)
		// ReSharper restore InconsistentNaming
		{
			if (isAsset) return;

			closeSession();

			if (Request["Error"] == "Force")
				throw new Exception("Forced error.");
		}



		private static void closeSession()
		{
			try
			{
				SessionManager.Close();
			}
			catch (DKException e)
			{
				if (e.Message != "Restart the Application.")
					throw;
			}
		}


		// ReSharper disable InconsistentNaming
		protected void Application_End()
		// ReSharper restore InconsistentNaming
		{
			SessionFactoryManager.End();

			IP.SaveOnline();
		}



		private static Uri url => HttpContext.Current.Request.Url;
		private static String path => url.AbsolutePath.ToLowerInvariant();

		private static Boolean isAsset =>
			path.StartsWith("/assets/")
			|| path.StartsWith("/favicon.ico")
			|| path.StartsWith("/robots.txt");

		private static Boolean isLocal => Cfg.IsLocal;

		private static readonly String elmahTestAction = getName(oc => oc.TestElmahLog);
		private static Boolean isElmah => path.Contains("elmah.axd")
			|| (!String.IsNullOrEmpty(elmahTestAction) && path.Contains(elmahTestAction));

		private delegate ActionResult testElmah();
		private static String getName(Expression<Func<OpsController, testElmah>> expression)
		{
			var body = (UnaryExpression)expression.Body;
			var operand = (MethodCallExpression)body.Operand;
			var @object = (ConstantExpression)operand.Object;
			var value = (_MethodInfo)@object?.Value;
			return value?.Name?.ToLowerInvariant();
		}

	}
}
