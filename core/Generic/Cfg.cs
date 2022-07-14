using System;
using System.IO;
using DFM.Generic.Settings;
using Microsoft.Extensions.Configuration;

namespace DFM.Generic
{
	public class Cfg
	{
		public static void Init(String environment = null)
		{
			var builder = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appSettings.json", true)
				.AddJsonFile("db.json", true)
				.AddJsonFile("smtp.json", true)
				.AddJsonFile("login.json", true)
				.AddJsonFile("s3.json", true)
				.AddJsonFile("tips.json", true)
				;

			if (environment != null)
			{
				builder
					.AddJsonFile($"db.{environment}.json", true)
					.AddJsonFile($"smtp.{environment}.json", true)
					.AddJsonFile($"login.{environment}.json", true)
					.AddJsonFile($"s3.{environment}.json", true)
					.AddJsonFile($"tips.{environment}.json", true)
					;
			}

			dic = builder.Build();
		}

		private static IConfiguration dic;

		public static IConfiguration DB => dic.GetSection("DB");

		public static String Server => DB["Server"];
		public static String DataBase => DB["DataBase"];
		public static String Login => DB["Login"];
		public static String Password => DB["Password"];

		public static Boolean ForceEmailError;

		public static String Email => dic["Email"];

		public static String Version => typeof(Cfg).Assembly.GetName().Version?.ToString();

		public static Int32 PasswordErrorLimit => Int32.Parse(dic["PasswordErrorLimit"]);

		public static String GooglePlay => dic["GooglePlay"];

		public static Int32 VersionCount => Int32.Parse(dic["VersionCount"] ?? "0");

		public static Smtp Smtp => new(dic.GetSection("Smtp"));
		public static Rewrite Rewrites => new("rewrites.json");
		public static S3 S3 => new(dic.GetSection("S3"));

		public static String LanguagePath { get; set; }

		public static String LogErrorsPath =>
			Path.Combine(dic["LogErrorsPath"].Split(","));

		public static String LogErrorsFile(String name) =>
			Path.Combine(LogErrorsPath, $"{name}.log");

		private static IConfiguration robot => dic.GetSection("Robot");
		public static String RobotEmail => robot["Email"];
		public static String RobotPassword => robot["Password"];

		public static Tips Tips => new Tips(dic.GetSection("Tips"));
	}
}
