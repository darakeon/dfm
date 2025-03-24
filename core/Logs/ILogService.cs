using System;

namespace DFM.Logs;

public interface ILogService
{
	void Log(Exception exception);
	void LogHandled(Exception exception, String message);
}
