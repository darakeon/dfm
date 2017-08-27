using System;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Configuration;
using System.Net.Mail;
using System.Runtime.InteropServices;
using Ak.XML;
using DFM.BusinessLogic;
using DFM.Email;
using NUnit.Framework;

namespace DFM.Tests.BusinessLogic.Helpers
{
    class ConfigHelper
    {
        private static readonly Node configFile = new Node(@"DFM.Tests.dll.config");
        private static String password;

        public static void BreakTheEmailSystem()
        {
            changePassword(String.Empty);
        }

        internal static void FixTheEmailSystem()
        {
            changePassword(password);
        }

        private static void changePassword(String newPassword)
        {
            var all = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var net = (SmtpSection)all.GetSection("system.net/mailSettings/smtp");
            var network = net.Network;

            if (newPassword == String.Empty)
                password = network.Password;
            
            network.Password = newPassword;

            all.Save();

            ConfigurationManager.RefreshSection("system.net/mailSettings/smtp");
        }



        internal static void ActivateEmailSystem()
        {
            ConfigurationManager.AppSettings["EmailSender"] = "";
        }

        internal static void DeactivateEmailSystem()
        {
            ConfigurationManager.AppSettings["EmailSender"] = "DontSend";
        }

        internal static void ActivateEmailForUser(ServiceAccess sa)
        {
            sa.Admin.UpdateConfig(null, null, true, null);
        }

        internal static void DeactivateEmailForUser(ServiceAccess sa)
        {
            sa.Admin.UpdateConfig(null, null, false, null);
        }

    }
}
