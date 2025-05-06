using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace DFM.BaseWeb.Helpers.Extensions;

public class ContextDic<T>
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
