using System;
using System.Web;
using Elmah;

namespace DFM.Generic
{
	public static class Elmah
	{
		public static Exception TryLog(this Exception exception)
		{
			try
			{
				var error = new Error(exception);
				var elmah = ErrorLog.GetDefault(HttpContext.Current);
				elmah.Log(error);

				return null;
			}
			catch (Exception e)
			{
				return e;
			}
		}
	}
}
