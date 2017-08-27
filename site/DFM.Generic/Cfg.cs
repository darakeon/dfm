using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFM.Generic
{
    public class Cfg
    {
        public static String EmailSender
        {
            get { return ConfigurationManager.AppSettings["EmailSender"]; }
            set { ConfigurationManager.AppSettings["EmailSender"] = value; }
        }

        public static String Email
        {
            get { return ConfigurationManager.AppSettings["Email"]; }
        }

        public static String Version
        {
            get { return ConfigurationManager.AppSettings["Version"]; }
        }

        public static Boolean IsLocal
        {
            get { return ConfigurationManager.AppSettings["IsLocal"] == "1"; }
        }

        public static Int32 PasswordErrorLimit
        {
            get { return Int32.Parse(ConfigurationManager.AppSettings["PasswordErrorLimit"]); }
        }

        public static String Get(String key)
        {
            return ConfigurationManager.AppSettings[key];
        }
    }
}
