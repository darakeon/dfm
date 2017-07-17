using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Ak.DataAccess.NHibernate;
using DFM.Core.Database;
using DFM.Core.Entities;
using DFM.Core.Mappings;
using DFM.MVC.Authentication;
using DFM.MVC.Helpers;

// ReSharper disable InconsistentNaming
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


            var mapInfo = new AutoMappingInfo<UserMap, User>();

            SessionBuilder.Start(mapInfo);
        }

        protected void Application_BeginRequest()
        {
            SessionBuilder.Open();
        }

        protected void Application_EndRequest()
        {
            SessionBuilder.Close();
        }

        protected void Application_End()
        {
            SessionBuilder.End();
        }
    
    }
}