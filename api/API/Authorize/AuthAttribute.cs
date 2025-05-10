using DFM.API.Controllers;
using DFM.BaseWeb.Helpers.Authorize;
using DFM.BusinessLogic.Exceptions;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DFM.API.Authorize
{
	public class AuthAttribute : BaseAuthAttribute
	{
		public AuthAttribute(AuthParams mandatory = AuthParams.None)
			: base(mandatory | AuthParams.Mobile) { }

		protected override void makeResult(
			AuthorizationFilterContext context, Error error
		)
		{
			context.Result = BaseApiController.ApiError(
				context.HttpContext, error
			);
		}
	}
}
