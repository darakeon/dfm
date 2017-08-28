using System;
using System.Configuration;

namespace DFM.Generic
{
    public class Cfg
    {
        public static String EmailSender
        {
            get { return ConfigurationManager.AppSettings["EmailSender"]; }
            set { ConfigurationManager.AppSettings["EmailSender"] = value; }
        }

        public static String Email => ConfigurationManager.AppSettings["Email"];

	    public static String Version => typeof(Cfg).Assembly.GetName().Version.ToString();

	    public static Boolean IsLocal => ConfigurationManager.AppSettings["IsLocal"] == "1";

	    public static Int32 PasswordErrorLimit => Int32.Parse(ConfigurationManager.AppSettings["PasswordErrorLimit"]);

	    public static String Get(String key)
        {
            return ConfigurationManager.AppSettings[key];
        }
    }
}
