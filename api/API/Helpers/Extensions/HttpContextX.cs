using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using DFM.API.Helpers.Global;
using DFM.BusinessLogic.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace DFM.API.Helpers.Extensions
{
	public static class HttpContextX
	{
		private static readonly ContextDic<Service> services =
			new(c => new Service(() => c));

		internal static Service GetService(this HttpContext context)
		{
			return services[context];
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

		public static String Translate(this HttpContext context, Error error)
		{
			return context.GetTranslator()[error];
		}

		public static Dictionary<String, Object> GetRoute(this HttpContext context)
		{
			return context.GetRouteData().Values
				.ToDictionary(v => v.Key, v => v.Value);
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
