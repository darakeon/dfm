using System;
using DFM.Authentication;
using DFM.Entities.Enums;

namespace DFM.BusinessLogic.Response
{
	public class SignInInfo : ISignInInfo
	{
		public String Email { get; set; }
		public String Password { get; set; }
		public String TicketKey { get; set; }
		public TicketType TicketType { get; set; }
	}
}
