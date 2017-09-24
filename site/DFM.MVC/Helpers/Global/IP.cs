using System;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;

namespace DFM.MVC.Helpers.Global
{
	public class IP
	{
		private const string relative_path = @"../../android/DFM/src/main/res/values/localaddress.xml";

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
				var regex = new Regex(@"\d*\.\d*\.\d*\.\d*");
				if (regex.IsMatch(lines[l]))
				{
					lines[l] = regex.Replace(lines[l], Get());
					break;
				}
			}

			File.WriteAllLines(path, lines);
		}

	}
}