using System;
using DFM.Entities.Bases;

namespace DFM.BusinessLogic.Helpers
{
	static class WarnHelper
	{
		public static Boolean PassedWarn1(this DateTime date) => date.passed(Limit1());
		public static Boolean PassedWarn2(this DateTime date) => date.passed(Limit2());
		public static Boolean PassedRemoval(this DateTime date) => date.passed(LimitRemoval());

		private static Boolean passed(this DateTime date, DateTime limit)
		{
			return date.ToUniversalTime() < limit;
		}

		public static DateTime Limit1() => limit(DayLimits.WIPE_WARN_1);
		public static DateTime Limit2() => limit(DayLimits.WIPE_WARN_2);
		public static DateTime LimitRemoval() => limit(DayLimits.WIPE);

		private static DateTime limit(Int32 days)
		{
			return DateTime.UtcNow.AddDays(-days);
		}
	}
}
