using System;
using System.IO;
using Newtonsoft.Json;

namespace DFM.Generic
{
	public static class ExceptionX
	{
		public static void TryLog(this Exception exception)
		{
			try
			{
				var date = DateTime.Now.ToString("yyyyMMddHHmmssffffff");
				var path = String.Format(Cfg.LogPathErrors, date);

				var json = JsonConvert.SerializeObject(exception, Formatting.Indented);

				File.WriteAllText(path, json);
			}
			catch { /* ignored, nothing can be done anymore */ }
		}
	}
}
