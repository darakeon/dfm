using DFM.Generic;

namespace DFM.Logs
{
	public static class LogFactory
	{
		private static ILogService? logService;

		public static ILogService Service =>
			logService ??= Cfg.Log.Local
				? new LocalLogService()
				: new CloudWatchService();
	}
}
