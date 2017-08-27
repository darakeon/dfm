using System;
using System.IO;
using Newtonsoft.Json;

namespace DFM.MVC.Areas.API.Helpers
{
    public class Debug
    {
        private const String filename = "../../pseudo-debug.txt";

        internal static void Log(object obj)
        {
            File.AppendAllText(filename, "\n" + JsonConvert.SerializeObject(obj));
        }

    }
}