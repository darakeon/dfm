using System;

namespace DFM.Logs;

public class LocalLogService : ILogService
{
	public void Log(Exception exception)
	{
	}

	public void LogHandled(Exception exception, String message)
	{
	}
}
