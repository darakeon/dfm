using System;

namespace DFM.Authentication
{
	public interface ISafeService<in SignInInfo, out SessionInfo>
		where SignInInfo: ISignInInfo
		where SessionInfo: ISessionInfo
	{
		SessionInfo GetSessionByTicket(String ticket);

		String ValidateUserAndCreateTicket(SignInInfo info);

		void DisableTicket(String ticket);

		Boolean VerifyTicket();
	}
}
