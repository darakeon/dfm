using System;
using System.Globalization;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using DFM.Core.Database.Base;
using DFM.Core.Robots;
using DFM.MVC.Authentication;
using DFM.MVC.Helpers;
using DFM.MVC.MultiLanguage;
using DFM.MVC.MultiLanguage.Helpers;
using log4net.Config;

namespace DFM.MVC
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("elmah.axd");

            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                RouteNames.Default, // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "User", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );
        }



        // ReSharper disable InconsistentNaming
        protected void Application_Start()
        // ReSharper restore InconsistentNaming
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            NHManager.Start();
            PlainText.Initialize();

            XmlConfigurator.Configure();
        }


        // ReSharper disable InconsistentNaming
        protected void Application_BeginRequest()
        // ReSharper restore InconsistentNaming
        {
            if (isAsset) return;

            NHManager.Open();

            if (isLocal)
                PlainText.Initialize();

        }

        // ReSharper disable InconsistentNaming
        protected void Application_AuthenticateRequest()
        // ReSharper restore InconsistentNaming
        {
            if (isAsset) return;

            if (isElmah && !Current.IsAdm)
                Response.Redirect("/");

            if (Current.IsAuthenticated)
                MainRobot.Run(Current.User, EmailFormats.GetForMove);

            specifyLanguage();
        }


        // ReSharper disable InconsistentNaming
        protected void Application_Error()
        // ReSharper restore InconsistentNaming
        {
            if (isAsset) return;

            NHManager.Error();
        }


        // ReSharper disable InconsistentNaming
        protected void Application_EndRequest(object sender, EventArgs e)
        // ReSharper restore InconsistentNaming
        {
            if (isAsset) return;

            NHManager.Close();
        }


        // ReSharper disable InconsistentNaming
        protected void Application_End()
        // ReSharper restore InconsistentNaming
        {
            NHManager.End();
        }




        private void specifyLanguage()
        {
            var browserLanguage =
                Request.UserLanguages != null && Request.UserLanguages.Length > 0
                    ? Request.UserLanguages[0]
                    : null;

            var language = Current.Language ?? browserLanguage;

            if (language == null || !PlainText.AcceptLanguage(language))
                language = "en-US";

            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(language);
        }



        private static Uri url { get { return HttpContext.Current.Request.Url; } }
        private static Boolean isAsset { get { return url.AbsolutePath.ToLowerInvariant().StartsWith("/Assets/"); } }
        private static Boolean isLocal { get { return url.Host == "localhost"; } }
        private static Boolean isElmah { get { return url.AbsolutePath.ToLowerInvariant().Contains("elmah.axd"); } }

    }
}