using System;
using System.Threading.Tasks;
using DFM.Logs.Data;
using DFM.Logs.Data.Application;
using DFM.Logs.Data.Errors;
using Microsoft.Extensions.Logging;

namespace DFM.Logs;

public abstract class BaseLogService : ILogService
{
	public async Task Log(Exception exception)
	{
		var error = new ErrorLog(exception);
		await saveLog(Division.Exception, error);
	}

	public async Task LogHandled(Exception exception, String message)
	{
		var error = new ErrorLog(exception, message);
		await saveLog(Division.Exception, error);
	}

	public async Task LogApplication<T>(String category, LogLevel logLevel, EventId eventId, T state, Exception? exception, Func<T, Exception?, String> formatter)
	{
		var log = new ApplicationLog<T>(category, logLevel, eventId, state, exception, formatter);
		await saveLog(Division.Application, log);
	}

	private protected abstract Task saveLog(Division division, Object content);
}
