using System;

namespace DFM.API.Helpers.Authorize
{
	[Flags]
	public enum AuthParams
	{
		None           = 0,
		Admin          = 1 << 0,
		IgnoreContract = 1 << 1,
		IgnoreTFA      = 1 << 2,
		Mobile         = 1 << 3,
	}
}