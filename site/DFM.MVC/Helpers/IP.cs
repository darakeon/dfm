using System;
using System.IO;
using System.Net;

namespace DFM.MVC.Helpers
{
    public class IP
    {
        const string relativePath = @"../../android/DontFlyMoney/src/com/dontflymoney/site/HttpHelper.java";

        internal static String Get()
        {
            var nome = Dns.GetHostName();
            var ipInfo = Dns.GetHostAddresses(nome);

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
            save("http://beta.dontflymoney.com");
        }

        private static void save(String address)
        {
            var currentPath = Directory.GetCurrentDirectory();

            var path = Path.Combine(currentPath, relativePath);
            var info = new FileInfo(path);
            path = info.FullName;

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

        const string domainDeclaration = "	private static String domain = \"";



    }
}