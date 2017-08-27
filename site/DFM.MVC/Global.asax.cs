using System;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Ak.Generic.Exceptions;
using DFM.BusinessLogic;
using DFM.BusinessLogic.Exceptions;
using DFM.Authentication;
using DFM.MVC.Helpers;
using DFM.MVC.Helpers.Authorize;
using DFM.PageLog;
using DFM.Repositories;
using log4net.Config;

namespace DFM.MVC
{
    public class MvcApplication : HttpApplication
    {
        private readonly Current current = Auth.Current;
        private readonly ServiceAccess access = new ServiceAccess(new Connector());


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

            IP.Save();
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

            if (isElmah && !current.IsAdm)
                Response.Redirect("/");

            if (current.IsAuthenticated)
                access.Robot.RunSchedule();
        }


        // ReSharper disable InconsistentNaming
        protected void Application_Error()
        // ReSharper restore InconsistentNaming
        {
            if (!isLocal)
                ErrorManager.SendEmail();

            error();
        }


        // ReSharper disable InconsistentNaming
        protected void Application_EndRequest(object sender, EventArgs e)
        // ReSharper restore InconsistentNaming
        {
            if (isAsset) return;

            if (DFMCoreException.ErrorCounter > 0)
                error();

            NHManager.Close();

            PageLogger.Record(Context, access.Safe);

            if (Request["Error"] == "Force")
                throw new Exception("Forced error.");
        }

        
        
        private static void error()
        {
            try
            {
                NHManager.Error();
            }
            catch (AkException e)
            {
                if (e.Message != "Restart the Application.")
                    throw;
            }
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