using System;

namespace DFM.Entities.Enums
{
	public enum TipType
	{
		None = TicketType.None,
		Browser = TicketType.Browser,
		Mobile = TicketType.Mobile,
		Local = TicketType.Local,
		Tests = TicketType.Tests,
	}

	[Flags]
	public enum TipTests : UInt64
	{
		None = 0,
		TestTip1 = 1UL << 0,
		TestTip2 = 1UL << 1,
		TestTip3 = 1UL << 2,
	}

	[Flags]
	public enum TipBrowser : UInt64
	{
		None = 0,
		DeleteLogins = 1UL << 0,
	}

	[Flags]
	public enum TipMobile : UInt64
	{
		None = 0,
	}
}
