using System;
using System.IO;
using Newtonsoft.Json;

namespace DFM.Generic
{
	public class ApiDebug
	{
		private const String filename = "../../pseudo-debug.txt";

		public static void Log(object obj)
		{
			File.AppendAllText(filename, "\n" + JsonConvert.SerializeObject(obj));
		}

	}
}