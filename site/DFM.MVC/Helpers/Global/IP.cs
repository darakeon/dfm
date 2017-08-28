using System;
using System.IO;
using System.Net;

namespace DFM.MVC.Helpers.Global
{
	public class IP
	{
		private const string relative_path = @"../../android/DFM/src/main/kotlin/com/darakeon/dfm/api/Site.kt";
		private const string domain_declaration = "	internal val Domain = ";

		internal static String Get()
		{
			var name = Dns.GetHostName();
			var ipInfo = Dns.GetHostAddresses(name);

			var ip = ipInfo[1].ToString();

			if (ip.Length > 15)
				ip = ipInfo[2].ToString();

			return ip;
		}

		internal static void SaveCurrent()
		{
			save("\"" + Get() + "\"");
		}

		internal static void SaveOnline()
		{
			save("publicDomain");
		}

		private static void save(String address)
		{
			var currentPath = Directory.GetCurrentDirectory();

			var path = Path.Combine(currentPath, relative_path);
			var info = new FileInfo(path);
			path = info.FullName;

			if (!info.Exists)
				return;

			var lines = File.ReadAllLines(path);

			for (var l = 0; l < lines.Length; l++)
			{
				if (lines[l].StartsWith(domain_declaration))
				{
					lines[l] = domain_declaration + address;
					break;
				}
			}

			File.WriteAllLines(path, lines);
		}




	}
}