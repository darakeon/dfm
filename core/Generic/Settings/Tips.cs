using System;
using Microsoft.Extensions.Configuration;

namespace DFM.Generic.Settings
{
	public class Tips
	{
		public Tips(IConfiguration tips)
		{
			Countdown = Int16.Parse(tips["Countdown"]);
			Repeat = Int16.Parse(tips["Repeat"]);
			Reset = Int16.Parse(tips["Reset"]);
		}

		public readonly Int16 Countdown;
		public readonly Int16 Repeat;
		public readonly Int16 Reset;
	}
}
