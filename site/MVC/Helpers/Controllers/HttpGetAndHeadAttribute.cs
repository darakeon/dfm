using System.Reflection;
using System.Web.Mvc;

namespace DFM.MVC.Helpers.Controllers
{
	public class HttpGetAndHeadAttribute : ActionMethodSelectorAttribute
	{
		public HttpGetAndHeadAttribute()
		{
			get = new HttpGetAttribute();
			head = new HttpHeadAttribute();
		}

		private readonly HttpGetAttribute get;
		private readonly HttpHeadAttribute head;

		public override bool IsValidForRequest(ControllerContext controllerContext, MethodInfo methodInfo)
		{
			return get.IsValidForRequest(controllerContext, methodInfo)
			       || head.IsValidForRequest(controllerContext, methodInfo);
		}
	}
}
