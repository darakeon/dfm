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
            if (isAsset) return;

            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            NHManager.Start();
            PlainText.Initialize();
        }


        // ReSharper disable InconsistentNaming
        protected void Application_BeginRequest()
        // ReSharper restore InconsistentNaming
        {
            if (isAsset) return;

            NHManager.Open();

            if (Request.Url.Host == "localhost")
                PlainText.Initialize();

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

            if (!NHManager.IsAssetCalling && Current.IsAuthenticated)
                ScheduleRunner.Run(Current.User);

            NHManager.Close();
        }


        // ReSharper disable InconsistentNaming
        protected void Application_End()
        // ReSharper restore InconsistentNaming
        {
            if (isAsset) return;

            NHManager.End();
        }




        private void specifyLanguage()
        {
            if (Request.UserLanguages == null || Request.UserLanguages.Length == 0)
                return;

            //var language = Request.UserLanguages[0];
            var language = "pt-BR";

            if (!PlainText.AcceptLanguage(language))
                language = "pt-BR";

            Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(language);
        }


        private static Boolean isAsset
        {
            get { return HttpContext.Current.Request.Url.AbsolutePath.StartsWith("/Assets/"); }
        }
    }
}