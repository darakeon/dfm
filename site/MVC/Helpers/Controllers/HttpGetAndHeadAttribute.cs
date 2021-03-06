﻿using System.Net;
using Microsoft.AspNetCore.Mvc.Routing;

namespace DFM.MVC.Helpers.Controllers
{
	public class HttpGetAndHeadAttribute : HttpMethodAttribute
	{
		public HttpGetAndHeadAttribute()
			: base(
				new[]
				{
					WebRequestMethods.Http.Get,
					WebRequestMethods.Http.Head
				}
			) { }
	}
}
