using DFM.BaseWeb.Helpers.Extensions;
using DFM.MVC.Helpers.Global;
using Microsoft.AspNetCore.Http;

namespace DFM.MVC.Helpers.Extensions
{
	public static class HttpContextX
	{
		private static readonly ContextDic<ErrorAlert> errorAlerts =
			new(c => new ErrorAlert(c.GetTranslator()));

		public static ErrorAlert GetErrorAlert(this HttpContext context)
		{
			return errorAlerts[context];
		}
	}
}
