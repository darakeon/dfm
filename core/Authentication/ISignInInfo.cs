using System;
using DFM.Generic.Settings;

namespace DFM.Authentication
{
	public interface ISignInInfo
	{
		String Email { get; set; }
		String Password { get; set; }
		String TicketKey { get; set; }
		TicketType TicketType { get; set; }
	}
}
