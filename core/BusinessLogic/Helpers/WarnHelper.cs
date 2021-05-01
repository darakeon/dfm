using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFM.BusinessLogic.Helpers
{
	static class WarnHelper
	{
		public static Boolean PassedWarn1(this DateTime date) => date.passed(30);
		public static Boolean PassedWarn2(this DateTime date) => date.passed(60);
		public static Boolean PassedRemoval(this DateTime date) => date.passed(90);

		private static Boolean passed(this DateTime date, Int32 days)
		{
			var now = DateTime.UtcNow;
			var pastDate = now.AddDays(-days);
			date = date.ToUniversalTime();
			return date < pastDate;
		}
	}
}
