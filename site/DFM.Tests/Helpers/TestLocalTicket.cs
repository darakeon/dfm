using DK.Generic.Extensions;
using DK.MVC.Cookies;

namespace DFM.Tests.Helpers
{
	class TestLocalTicket : PseudoTicket
	{
		public TestLocalTicket() 
			: base(Token.New(), TicketType.Local) { }
	}
}
