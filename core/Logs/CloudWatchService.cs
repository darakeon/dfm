using System;

namespace DFM.Logs;

public class CloudWatchService : ILogService
{
	public void Log(Exception exception)
	{
	}

	public void LogHandled(Exception exception, String message)
	{
	}
}
