using System;

namespace DFM.Generic.Datetime
{
	class TimeZone
	{
		public Boolean Negative { get; set; }
		public Int16 HourDiff { get; set; }
		public Int16 MinuteDiff { get; set; }

		private Int32 sign => Negative ? -1 : 1;
		public Int32 Hour => sign * HourDiff;
		public Int32 Minute => sign * MinuteDiff;

		public override String ToString()
		{
			return $"UTC{Hour:+00;-00; 00}:{Minute:00;00;00}";
		}

		public Boolean Is(Int32 offset)
		{
			return Hour * 60 + Minute == offset;
		}
	}
}
