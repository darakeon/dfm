using DFM.Authentication;
using DFM.BusinessLogic.Repositories;
using DFM.BusinessLogic.Response;
using DFM.Entities;

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
