using System;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using DFM.BusinessLogic.Exceptions;
using DFM.MVC.Authentication;
using DFM.MVC.Helpers;
using DFM.Repositories;
using DFM.Robot;
using log4net.Config;

namespace DFM.MVC
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : HttpApplication
    {
        // ReSharper disable InconsistentNaming
        protected void Application_Start()
        // ReSharper restore InconsistentNaming
        {
            AreaRegistration.RegisterAllAreas();

            GeneralAreaRegistration.RegisterGlobalFilters(GlobalFilters.Filters);
            GeneralAreaRegistration.RegisterRoutes(RouteTable.Routes);

            Directory.SetCurrentDirectory(Server.MapPath("~"));

            NHManager.Start();
            MultiLanguage.Initialize();

            XmlConfigurator.Configure();
        }


        // ReSharper disable InconsistentNaming
        protected void Application_BeginRequest()
        // ReSharper restore InconsistentNaming
        {
            if (isAsset) return;

            if (isLocal)
                MultiLanguage.Initialize();

        }

        // ReSharper disable InconsistentNaming
        protected void Application_AuthenticateRequest()
        // ReSharper restore InconsistentNaming
        {
            if (isAsset) return;

            if (isElmah && !Current.IsAdm)
                Response.Redirect("/");

            if (Current.IsAuthenticated)
                MainRobot.Run(Current.User, Services.Robot);
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

            if (DFMCoreException.ErrorCounter > 0)
                NHManager.Error();

            NHManager.Close();
        }


        // ReSharper disable InconsistentNaming
        protected void Application_End()
        // ReSharper restore InconsistentNaming
        {
            NHManager.End();
        }



        private static Uri url { get { return HttpContext.Current.Request.Url; } }
        private static Boolean isAsset { get { return url.AbsolutePath.ToLowerInvariant().StartsWith("/assets/"); } }
        private static Boolean isLocal { get { return url.Host == "localhost"; } }
        private static Boolean isElmah { get { return url.AbsolutePath.ToLowerInvariant().Contains("elmah.axd"); } }

    }
}