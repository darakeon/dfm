using System;
using System.IO;
using System.Net;

namespace DFM.MVC.Helpers
{
    public class IP
    {
        internal static String Get()
        {
            var nome = Dns.GetHostName();
            var ipInfo = Dns.GetHostAddresses(nome);

            var ip = ipInfo[1].ToString();

            if (ip.Length > 15)
                ip = ipInfo[2].ToString();

            return ip;
        }

        internal static void Save()
        {
            var relativePath = @"../../android/DontFlyMoney/src/com/dontflymoney/site/IP";
            var currentPath = Directory.GetCurrentDirectory();

            var path = Path.Combine(currentPath, relativePath);
            var info = new FileInfo(path);

            path = info.FullName;

            var ip = Get();

            File.WriteAllText(path, ip);
        }


    }
}