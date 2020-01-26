using System;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace DFM.Generic
{
	public class Cfg
	{
		static Cfg()
		{
			Dic = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appSettings.json")
				.AddJsonFile("smtp.json")
				.Build();
		}

		public static readonly IConfiguration Dic;

		public static String Server => Dic["Server"];
		public static String DataBase => Dic["DataBase"];
		public static String Login => Dic["Login"];
		public static String Password => Dic["Password"];

		public static String EmailSender
		{
			get => Dic["EmailSender"];
			set => Dic["EmailSender"] = value;
		}

		public static String Email => Dic["Email"];

		public static String Version => typeof(Cfg).Assembly.GetName().Version.ToString();

		public static Int32 PasswordErrorLimit => Int32.Parse(Dic["PasswordErrorLimit"]);

		public static String GooglePlay => Dic["GooglePlay"];

		public static Int32 VersionCount => Int32.Parse(Dic["VersionCount"]);

		public static Smtp Smtp => new Smtp(Dic.GetSection("Smtp"));

		public static String LanguagePath { get; set; }

		public static String LogPathErrors => Dic["LogPathErrors"];
	}
}
