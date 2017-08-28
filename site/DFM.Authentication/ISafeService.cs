using System;
using DFM.Entities;
using DFM.Entities.Enums;

namespace DFM.Authentication
{
	public interface ISafeService
	{
		User GetUserByTicket(String ticket);

		String ValidateUserAndCreateTicket(String username, String password, String ticket, TicketType ticketType);

		void DisableTicket(String ticket);

	}
}
