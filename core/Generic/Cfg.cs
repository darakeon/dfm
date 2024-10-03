using System;
using System.Collections.Immutable;
using System.IO;
using System.Text;
using DFM.Generic.Settings;
using Microsoft.Extensions.Configuration;

namespace DFM.Generic
{
	public class Cfg
	{
		private static readonly ImmutableList<String> configTypes =
			ImmutableList.Create("db", "smtp", "login", "s3", "sqs");

		public static void Init(String environment = null)
		{
			var builder = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory());

			fromDefaultFiles(builder);

			fromDifferentEnvironment(builder, environment);

			fromBase64EnvVars(builder);

			dic = builder.Build();
		}

		private static void fromDefaultFiles(IConfigurationBuilder builder)
		{
			builder
				.AddJsonFile("appSettings.json", true)
				.AddJsonFile("tips.json", true);

			configTypes.ForEach(
				ct => builder.AddJsonFile($"{ct}.json", true)
			);
		}

		private static void fromDifferentEnvironment(IConfigurationBuilder builder, String? environment)
		{
			if (environment == null) return;

			configTypes.ForEach(
				ct => builder.AddJsonFile($"{ct}.{environment}.json", true)
			);
		}
		
		private static void fromBase64EnvVars(IConfigurationBuilder builder)
		{
			foreach (var configType in configTypes)
			{
				var envVarName = $"CFG_{configType.ToUpper()}";
				var envVar = Environment.GetEnvironmentVariable(envVarName);

				if (envVar == null)
					continue;

				var bytes = Convert.FromBase64String(envVar);
				var json = Encoding.UTF8.GetString(bytes);

				var filePath = $"/tmp/{configType}.envVar.json";
				File.WriteAllText(filePath, json);

				builder.AddJsonFile(filePath, true);
			}
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
		public static SQS SQS => new(dic.GetSection("SQS"));

		public static String LanguagePath { get; set; }

		public static String LogErrorsPath =>
			Path.Combine(dic["LogErrorsPath"].Split(","));

		public static String LogErrorsFile(String name) =>
			Path.Combine(LogErrorsPath, $"{name}.log");

		private static IConfiguration robot => dic.GetSection("Robot");
		public static String RobotEmail => robot["Email"];
		public static String RobotPassword => robot["Password"];

		public static Tips Tips => new Tips(dic.GetSection("Tips"));

		public const String EmailContact = "dfm@dontflymoney.com";
		public const String BuyMeACoffee = "https://buymeacoffee.com/darakeon";
	}
}
