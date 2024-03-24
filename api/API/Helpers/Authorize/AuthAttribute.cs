using System;
using DFM.API.Controllers;
using DFM.API.Helpers.Extensions;
using DFM.BusinessLogic;
using DFM.BusinessLogic.Exceptions;
using DFM.Entities.Enums;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DFM.API.Helpers.Authorize
{
	public class AuthAttribute : Attribute, IAuthorizationFilter
	{
		private readonly AuthParams mandatory;

		public AuthAttribute(AuthParams mandatory = AuthParams.None)
		{
			this.mandatory = mandatory | AuthParams.Mobile;
		}

		private Service service;
		private Current current => service.Current;
		private ServiceAccess access => service.Access;
		private Boolean isAuthenticated => current.IsAuthenticated;

		private Boolean denyByAdmin =>
			mandatory.HasFlag(AuthParams.Admin)
				&& !current.IsAdm;

		private Boolean denyByContract =>
			!mandatory.HasFlag(AuthParams.IgnoreContract)
				&& !access.Law.IsLastContractAccepted();
		
		private Boolean denyByTFA =>
			!mandatory.HasFlag(AuthParams.IgnoreTFA)
				&& !access.Auth.VerifyTicketTFA();
		
		private Boolean denyByMobile =>
			!access.Auth.VerifyTicketType(TicketType.Mobile);

		public void OnAuthorization(AuthorizationFilterContext context)
		{
			service = context.HttpContext.GetService();

			if (!isAuthenticated)
				makeResult(context, Error.LoginRequested);

			else if (denyByTFA)
				makeResult(context, Error.TFANotVerified);

			else if (denyByContract)
				makeResult(context, Error.NotSignedLastContract);

			else if (denyByAdmin || denyByMobile)
				makeResult(context, Error.Uninvited);
		}

		protected void makeResult(AuthorizationFilterContext filterContext, Error error)
		{
			filterContext.Result = BaseApiController.ApiError(
				filterContext.HttpContext, error
			);
		}
	}
}
