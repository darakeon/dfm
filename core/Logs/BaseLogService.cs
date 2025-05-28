using System;
using System.Threading.Tasks;
using DFM.Logs.Data;
using DFM.Logs.Data.Application;
using DFM.Logs.Data.Console;
using DFM.Logs.Data.Errors;
using DFM.Logs.Data.Requests;
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

	public async Task LogRequest(String moment, String method, String path, DateTime time)
	{
		var log = new RequestLog(moment, method, path, time);
		await saveLog(Division.Request, log);
	}

	public async Task LogConsoleOk(Object message)
	{
		var log = ConsoleLog.Ok(message);
		await saveLog(Division.Console, log);
	}

	public async Task LogConsoleError(Object message)
	{
		var log = ConsoleLog.Error(message);
		await saveLog(Division.Console, log);
	}

	public async Task LogNH(String message)
	{
		var log = ConsoleLog.Error(message);
		await saveLog(Division.NHibernate, log);
	}

	private protected abstract Task saveLog(Division division, Object content);
}
