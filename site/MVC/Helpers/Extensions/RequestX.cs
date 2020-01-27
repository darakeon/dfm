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
	}
}
