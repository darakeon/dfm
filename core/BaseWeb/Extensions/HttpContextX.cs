using System;
using System.Collections.Generic;
using System.Linq;
using DFM.BaseWeb.BusinessLogic;
using DFM.BaseWeb.Languages;
using DFM.BusinessLogic.Exceptions;
using DFM.Language.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace DFM.BaseWeb.Extensions
{
	public static class HttpContextX
	{
		private static readonly ContextDic<Service> services =
			new(c => new Service(() => c));

		public static Service GetService(this HttpContext context)
		{
			return services[context];
		}

		private static readonly ContextDic<Translator> translators =
			new(c => new Translator(c));

		public static void StartTranslator(this HttpContext context)
		{
			context.GetTranslator();
		}

		public static Translator GetTranslator(this HttpContext context)
		{
			return translators[context];
		}

		public static String Translate(this HttpContext context, Error error)
		{
			return context.GetTranslator()[error];
		}

		public static String Translate(this HttpContext context, params String[] text)
		{
			return context.GetTranslator()[text];
		}

		public static IList<String> TranslateList(this HttpContext context, String text)
		{
			return context.GetTranslator().List(text);
		}

		public static String TryTranslate(this HttpContext context, params String[] text)
		{
			try
			{
				return context.Translate(text);
			}
			catch (DicException)
			{
				return null;
			}
		}

		public static String Translate(this HttpContext context, CoreError error)
		{
			return context.GetTranslator()[error];
		}

		public static String GetMonthName(this HttpContext context, Int32 month)
		{
			return context.GetTranslator().GetMonthName(month);
		}

		public static Dictionary<String, Object> GetRoute(this HttpContext context)
		{
			return context.GetRouteData().Values
				.ToDictionary(v => v.Key, v => v.Value);
		}

		public static Dictionary<String, String> GetRouteText(this HttpContext context)
		{
			var result = context.GetRoute()
				.ToDictionary(v => v.Key, v => v.Value.ToString());

			if (!result.ContainsKey("area"))
				result.Add("area", null);

			return result;
		}
	}
}
