using System;
using System.Configuration;
using DFM.BusinessLogic;

namespace DFM.Tests.BusinessLogic.Helpers
{
    class ConfigHelper
    {
        private static String oldEmailConfig;

        public static void BreakTheEmailSystem()
        {
            oldEmailConfig = ConfigurationManager.AppSettings["EmailSender"];
            ConfigurationManager.AppSettings["EmailSender"] = "MakeError";
        }

        internal static void FixTheEmailSystem()
        {
            ConfigurationManager.AppSettings["EmailSender"] = oldEmailConfig;
            oldEmailConfig = "";
        }



        internal static void ActivateEmailSystem()
        {
            ConfigurationManager.AppSettings["EmailSender"] = "";
        }

        internal static void DeactivateEmailSystem()
        {
            ConfigurationManager.AppSettings["EmailSender"] = "DontSend";
        }



        internal static void ActivateMoveEmailForUser(ServiceAccess sa)
        {
            sa.Admin.UpdateConfig(null, null, true, null);
        }

        internal static void DeactivateMoveEmailForUser(ServiceAccess sa)
        {
            sa.Admin.UpdateConfig(null, null, false, null);
        }

        internal static void ActivateCategoriesUseForUser(ServiceAccess sa)
        {
            sa.Admin.UpdateConfig(null, null, null, true);
        }

        internal static void DeactivateCategoriesUseForUser(ServiceAccess sa)
        {
            sa.Admin.UpdateConfig(null, null, null, false);
        }



    }
}
