using System.Reflection;
using System.Web.Mvc;

namespace DFM.MVC.Helpers.Authorize
{
	class HttpShouldBePostAttribute : ActionMethodSelectorAttribute
	{
		private readonly HttpGetAttribute get;

		public HttpShouldBePostAttribute()
		{
			get = new HttpGetAttribute();
		}

		public override bool IsValidForRequest(ControllerContext controllerContext, MethodInfo methodInfo)
		{
			return get.IsValidForRequest(controllerContext, methodInfo);
		}
	}
}