using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;

namespace DFM.MVC.Helpers.Global
{
	public class IP
	{
		private const string relativePath =
			@"..\..\android\DFM\src\main\res\values\localaddress.xml";

		private static readonly Regex ip =
			new Regex(@"\d+\.\d+\.\d+\.\d+");

		internal static void SaveCurrent()
		{
			var currentPath = Directory.GetCurrentDirectory();

			var path = Path.Combine(currentPath, relativePath);
			var info = new FileInfo(path);
			path = info.FullName;

			if (!info.Exists)
				return;

			var content = File.ReadAllText(path);
			content = ip.Replace(content, currentIP);
			File.WriteAllText(path, content);
		}

		private static String currentIP
		{
			get
			{
				var name = Dns.GetHostName();
				var ipInfo = Dns.GetHostAddresses(name);

				return ipInfo
					.Select(i => i.ToString())
					.LastOrDefault(ip.IsMatch);
			}
		}
	}
}
