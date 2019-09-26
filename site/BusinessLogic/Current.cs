using DFM.Authentication;
using DFM.BusinessLogic.Response;

namespace DFM.BusinessLogic
{
	public class Current : Current<SignInInfo, SessionInfo>
	{
		internal Current(
			ISafeService<SignInInfo, SessionInfo> userService,
			GetTicket getTicket
		)
			: base(userService, getTicket)
		{
		}
	}
}
