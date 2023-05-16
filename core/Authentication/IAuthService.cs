using System;

namespace DFM.Authentication
{
	public interface IAuthService<in SignInInfo, out SessionInfo>
		where SignInInfo: ISignInInfo
		where SessionInfo: ISessionInfo
	{
		SessionInfo GetSession(String ticket);

		String CreateTicket(SignInInfo info);

		void DisableTicket(String ticket);

		Boolean VerifyTicketTFA();
	}
}
