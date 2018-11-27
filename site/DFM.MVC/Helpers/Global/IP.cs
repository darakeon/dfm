using System;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;

namespace DFM.MVC.Helpers.Global
{
	public class IP
	{
		private const string relativePath = @"..\..\android\DFM\src\main\res\values\localaddress.xml";

		internal static void SaveCurrent()
		{
			var currentPath = Directory.GetCurrentDirectory();

			var path = Path.Combine(currentPath, relativePath);
			var info = new FileInfo(path);
			path = info.FullName;

			if (!info.Exists)
				return;

			var lines = File.ReadAllLines(path);

			for (var l = 0; l < lines.Length; l++)
			{
				var regex = new Regex(@"\d+\.\d+\.\d+\.\d+");
				if (regex.IsMatch(lines[l]))
				{
					lines[l] = regex.Replace(lines[l], currentIP);
					break;
				}
			}

			File.WriteAllLines(path, lines);
		}

		private static String currentIP
		{
			get
			{
				var name = Dns.GetHostName();
				var ipInfo = Dns.GetHostAddresses(name);

				var ip = ipInfo[1].ToString();

				if (ip.Length > 15)
					ip = ipInfo[2].ToString();

				return ip;
			}
		}
	}
}