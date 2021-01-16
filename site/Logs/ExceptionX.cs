using System;
using System.IO;
using DFM.Generic;

namespace DfM.Logs
{
	public static class ExceptionX
	{
		public static void TryLog(this ErrorLog error)
		{
			try
			{
				var path = String.Format(Cfg.LogPathErrors, error.ID);
				File.WriteAllText(path, error.ToString());
			}
			catch { /* ignored, nothing can be done anymore */ }
		}

		public static void TryLog(this Exception exception)
		{
			new ErrorLog(exception, false).TryLog();
		}

		public static void TryLogHandled(this Exception exception, String message)
		{
			new ErrorLog(
				new SystemError(message, exception), true
			).TryLog();
		}
	}
}
