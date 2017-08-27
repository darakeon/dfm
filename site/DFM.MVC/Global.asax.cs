using System;
using System.Configuration;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Ak.Generic.Exceptions;
using Ak.NHibernate;
using DFM.Authentication;
using DFM.BusinessLogic;
using DFM.BusinessLogic.Exceptions;
using DFM.Email;
using DFM.Entities;
using DFM.Generic;
using DFM.MVC.Helpers.Authorize;
using DFM.MVC.Helpers.Global;
using DFM.Repositories;
using DFM.Repositories.Mappings;
using log4net.Config;

namespace DFM.MVC
{
    public class MvcApplication : HttpApplication
    {
        private readonly Current current = Auth.Current;
        private readonly ServiceAccess access = new ServiceAccess();

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            
            Directory.SetCurrentDirectory(Server.MapPath("~"));

            NHManager.Start<UserMap, User>();
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

            NHManager.Open();
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
            if (isAsset) return;

            if (current.IsAuthenticated)
            {
                var emailsStatus = access.Robot.RunSchedule();

                if (emailsStatus.IsWrong())
                {
                    var message = MultiLanguage.Dictionary["ScheduleRun"];
                    var error = MultiLanguage.Dictionary[emailsStatus].ToLower();
                    var final = String.Format(message, error);

                    ErrorAlert.AddTranslated(final);
                }
            }
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

            IP.SaveOnline();
        }



        private static Uri url { get { return HttpContext.Current.Request.Url; } }
        private static Boolean isAsset { get { return url.AbsolutePath.ToLowerInvariant().StartsWith("/assets/"); } }
        private static Boolean isLocal { get { return Cfg.IsLocal; } }
        private static Boolean isElmah { get { return url.AbsolutePath.ToLowerInvariant().Contains("elmah.axd"); } }

    }
}
