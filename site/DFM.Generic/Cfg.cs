using System;
using System.Collections.Specialized;
using System.Configuration;

namespace DFM.Generic
{
	public class Cfg
	{
		private static NameValueCollection appSettings => ConfigurationManager.AppSettings;

		public static String Server = appSettings["Server"];
		public static String DataBase = appSettings["DataBase"];
		public static String Login = appSettings["Login"];
		public static String Password = appSettings["Password"];

		public static String EmailSender
		{
			get { return appSettings["EmailSender"]; }
			set { appSettings["EmailSender"] = value; }
		}

		public static String Email => appSettings["Email"];

		public static String Version => typeof(Cfg).Assembly.GetName().Version.ToString();

		public static Boolean IsLocal => appSettings["IsLocal"] == "1";

		public static Int32 PasswordErrorLimit => Int32.Parse(appSettings["PasswordErrorLimit"]);

		public static String GooglePlay => appSettings["GooglePlay"];
	}
}
