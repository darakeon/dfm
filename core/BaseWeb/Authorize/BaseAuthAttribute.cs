using System;
using DFM.BaseWeb.Extensions;
using DFM.BusinessLogic.Exceptions;
using DFM.Entities.Enums;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DFM.BaseWeb.Authorize
{
	public abstract class BaseAuthAttribute(AuthParams mandatory)
		: Attribute, IAuthorizationFilter
	{
		public void OnAuthorization(AuthorizationFilterContext context)
		{
			var service = context.HttpContext.GetService();
			var current = service.Current;
			var access = service.Access;
			var isAuthenticated = current.IsAuthenticated;


			var denyByAdmin =
				isAuthenticated
				&& mandatory.HasFlag(AuthParams.Admin)
				&& !current.IsAdm;

			var denyByContract =
				isAuthenticated
				&& !mandatory.HasFlag(AuthParams.IgnoreContract)
				&& !access.Law.IsLastContractAccepted();

			var denyByTFA =
				isAuthenticated
				&& !mandatory.HasFlag(AuthParams.IgnoreTFA)
				&& !access.Auth.VerifyTicketTFA();

			var denyByMobile =
				isAuthenticated
				&& mandatory.HasFlag(AuthParams.Mobile)
				&& !access.Auth.VerifyTicketType(TicketType.Mobile);


			if (!isAuthenticated)
				makeResult(context, Error.LoginRequested);

			else if (denyByTFA)
				makeResult(context, Error.TFANotVerified);

			else if (denyByContract)
				makeResult(context, Error.NotSignedLastContract);

			else if (denyByAdmin || denyByMobile)
				makeResult(context, Error.Uninvited);
		}

		protected abstract void makeResult(
			AuthorizationFilterContext context,
			Error error
		);
	}
}
