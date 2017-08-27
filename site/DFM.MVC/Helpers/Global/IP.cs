using System;
using System.IO;
using System.Net;

namespace DFM.MVC.Helpers.Global
{
    public class IP
    {
        const string relativePath = @"../../android/src/com/dontflymoney/api/Site.java";

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
            save(Get());
        }

        internal static void SaveOnline()
        {
            save("beta.dontflymoney.com");
        }

        private static void save(String address)
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
                if (lines[l].StartsWith(domainDeclaration))
                {
                    lines[l] = domainDeclaration + address + "\";";
                    break;
                }
            }

            File.WriteAllLines(path, lines);
        }

        const string domainDeclaration = "	public static final String Domain = \"";



    }
}