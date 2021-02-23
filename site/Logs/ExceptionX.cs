using System;
using System.IO;
using DFM.Generic;

namespace DfM.Logs
{
	public static class ExceptionX
	{
		public static void TryLog(this ErrorLog error)
		{
			var textError = error.ToString();

			try
			{
				var path = String.Format(Cfg.LogPathErrors, error.ID);
				File.WriteAllText(path, textError);
			}
			catch (Exception e)
			{
				Console.WriteLine($"Error while recording log: {e}");
			}
			finally
			{
				Console.WriteLine($"Error that happened: {textError.Replace("\\n", "\n")}");
			}
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
