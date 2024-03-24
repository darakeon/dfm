using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace DFM.MVC.Helpers.Extensions
{
	public static class RequestX
	{
		public static IDictionary<String, String> GetSafeFields(this HttpRequest request)
		{
			return request.getFields()
				.Where(
					p => !p.Key.Contains("Password")
							&& !Decimal.TryParse(p.Value, out _)
				)
				.ToDictionary(p => p.Key, p => p.Value.ToString());
		}

		private static IEnumerable<KeyValuePair<String, StringValues>> getFields(this HttpRequest request)
		{
			switch (request.Method)
			{
				case WebRequestMethods.Http.Get:
					return request.Query;

				case WebRequestMethods.Http.Post:
					return request.Form;

				default:
					return new KeyValuePair<String, StringValues>[0];
			}
		}

		public static Boolean IsAsset(this HttpContext context)
		{
			var path = context.Request.Path.Value;
			return path != null && path.StartsWith("/Assets");
		}

		public static void InitLog(this HttpContext context)
		{
			context.Items.Add("Logs", new List<String>());
		}

		public static void AddLog(this HttpContext context, String log)
		{
			GetLogs(context).Add(log);
		}

		public static List<String> GetLogs(this HttpContext context)
		{
			return context.Items["Logs"] as List<String>;
		}
	}
}
