using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using DFM.BusinessLogic.Exceptions;
using DFM.MVC.Helpers.Global;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace DFM.MVC.Helpers.Extensions
{
	public static class HttpContextX
	{
		private static readonly ContextDic<Service> services =
			new(c => new Service(() => c));

		public static Service GetService(this HttpContext context)
		{
			return services[context];
		}

		private static readonly ContextDic<ErrorAlert> errorAlerts =
			new(c => new ErrorAlert(translators[c]));

		public static ErrorAlert GetErrorAlert(this HttpContext context)
		{
			return errorAlerts[context];
		}

		private static readonly ContextDic<Translator> translators =
			new(c => new Translator(c));

		public static void StartTranslator(this HttpContext context)
		{
			context.GetTranslator();
		}

		internal static Translator GetTranslator(this HttpContext context)
		{
			return translators[context];
		}

		public static String Translate(this HttpContext context, params String[] text)
		{
			return context.GetTranslator()[text];
		}

		public static String Translate(this HttpContext context, Error error)
		{
			return context.GetTranslator()[error];
		}

		public static String Translate(this HttpContext context, CoreError error)
		{
			return context.GetTranslator()[error];
		}

		public static String GetMonthName(this HttpContext context, Int32 month)
		{
			return context.GetTranslator().GetMonthName(month);
		}

		public static Int32 CountTranslations(this HttpContext context, String sectionToCount, String phrasePrefix)
		{
			return context.GetTranslator().CountTranslations(sectionToCount, phrasePrefix);
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

		class ContextDic<T>
		{
			private readonly Func<HttpContext, T> create;

			public ContextDic(Func<HttpContext, T> create)
			{
				this.create = create;
			}

			private readonly IDictionary<HttpContext, T> dic =
				new ConcurrentDictionary<HttpContext, T>();

			public T this[HttpContext context]
			{
				get
				{
					if (!dic.ContainsKey(context))
						dic.Add(context, create(context));

					return dic[context];
				}
			}
		}
	}
}
