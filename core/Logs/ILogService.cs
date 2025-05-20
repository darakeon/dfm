using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace DFM.Logs;

public interface ILogService
{
	Task Log(Exception exception);
	Task LogHandled(Exception exception, String message);

	Task LogApplication<T>(String category, LogLevel logLevel, EventId eventId, T state, Exception? exception, Func<T, Exception?, String> formatter);

	Task LogRequest(String moment, String method, String path, DateTime time);

	Task LogConsoleOk(object message);
	Task LogConsoleError(object message);
}
