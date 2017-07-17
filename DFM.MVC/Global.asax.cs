using System.Globalization;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using DFM.Core.Database.Base;
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
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );
        }



        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            NHManager.Start();
            PlainText.Initialize();
        }

        protected void Application_BeginRequest()
        {
            NHManager.Open();
            
            if (Request.Url.Host == "localhost")
                PlainText.Initialize();

            specifyLanguage();
        }

        protected void Application_Error()
        {
            NHManager.Error();
        }

        protected void Application_EndRequest()
        {
            NHManager.Close();
        }

        protected void Application_End()
        {
            NHManager.End();
        }




        private void specifyLanguage()
        {
            if (Request.UserLanguages == null || Request.UserLanguages.Length == 0)
                return;

            var language = "pt-BR"; //Request.UserLanguages[0];

            if (!PlainText.AcceptedLanguages.Contains(language))
                language = "pt-BR";

            Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(language);
        }

    }
}