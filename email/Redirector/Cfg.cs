using System;
using Amazon;

namespace Redirector
{
	static class Cfg
	{
		public class Smtp
		{
			public static readonly String From = env("FROM");
			public static readonly String FromName = env("NAME");

			public static readonly String To = env("TO");

			public static readonly String Username = env("UN");
			public static readonly String Password = env("PW");

			public static readonly Int32 Port = Int32.Parse(env("PORT"));

			private static String env(String key) => envByType("SES", key);
		}

		public class File
		{
			public static readonly String Username = env("UN");
			public static readonly String Password = env("PW");
			public static readonly String Bucket = env("BUCKET");

			private static String env(String key) => envByType("S3", key);
		}

		public static readonly RegionEndpoint Region = RegionEndpoint.USWest2;

		private static String envByType(String type, String key)
		{
			var value = Environment.GetEnvironmentVariable($"AWS_{type}_{key}");
			Console.WriteLine($"{type} {key} empty: {String.IsNullOrEmpty(value)}");
			return value;
		}
	}
}
