using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Builder;

namespace DFM.MVC.Starters
{
	class Mobile
	{
		private static readonly Regex ipRegex = new Regex(@"\d+\.\d+\.\d+\.\d+");

		private static readonly String androidConfigPath =
			@"..\..\android\DFM\src\main\res\values\localaddress.xml";

		private static readonly String iisExpressConfigPath =
			@"..\.vs\DFM\config\applicationhost.config";

		private static readonly String bindingToAdd =
			"<binding protocol=\"http\" bindingInformation=\":80:*\" />";

		private static readonly String bindingAdded =
			"<binding protocol=\"http\" bindingInformation=\"*:80:localhost\" />";

		internal static void ConfigureIP()
		{
			configAtAndroid();
			configAtIISExpress();
		}

		private static void configAtAndroid()
		{
			var path = getPath(androidConfigPath);

			if (path != null)
				setIpAtAndroid(path);
		}

		private static void setIpAtAndroid(string path)
		{
			var content = File.ReadAllText(path);
			content = ipRegex.Replace(content, getCurrentIP());
			File.WriteAllText(path, content);
		}

		private static String getCurrentIP()
		{
			var name = Dns.GetHostName();
			var ipInfo = Dns.GetHostAddresses(name);

			return ipInfo
				.Select(i => i.ToString())
				.LastOrDefault(ipRegex.IsMatch);
		}

		private static void configAtIISExpress()
		{
			var path = getPath(iisExpressConfigPath);

			if (path != null)
				setAll80Listening(path);
		}

		private static void setAll80Listening(String path)
		{
			var lines = File.ReadAllLines(path).ToList();

			var initialSize = bindingAdded.Length;
			var bindingBefore = lines.Single(
				l => l.EndsWith(bindingAdded)
			);
			var newSize = bindingBefore.Length;

			var spaces = bindingBefore
				.Substring(0, newSize - initialSize);

			var toAdd = spaces + bindingToAdd;

			var lineIndex = lines.IndexOf(bindingBefore);
			var nextLineIndex = lineIndex + 1;

			if (lines[nextLineIndex] == toAdd)
				return;

			var lineCounts = lineIndex + 1;

			var before = lines.Take(lineCounts);
			var after = lines.Skip(lineCounts);

			var newLines = new List<String>();
			newLines.AddRange(before);
			newLines.Add(toAdd);
			newLines.AddRange(after);

			File.WriteAllLines(path, newLines);
		}

		private static String getPath(String relativePath)
		{
			var currentPath = Directory.GetCurrentDirectory();
			var path = Path.Combine(currentPath, relativePath);
			var info = new FileInfo(path);
			return info.Exists ? info.FullName : null;
		}
	}
}
