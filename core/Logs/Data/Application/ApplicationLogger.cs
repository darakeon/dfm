using System;
using Microsoft.Extensions.Logging;

namespace DFM.Logs.Data.Application;

public class ApplicationLogger(String categoryName) : ILogger
{
	private LogLevel level;

	public IDisposable BeginScope<TState>(TState state) where TState : notnull
	{
		return default!;
	}

	public Boolean IsEnabled(LogLevel logLevel)
	{
		//current Warning 3 , logLevel Debug 1		no
		//current Warning 3 , logLevel Warning 3	yes
		//current Debug 1 , logLevel Debug 1		yes
		//current Debug 1 , logLevel Warning 3		yes
		return level <= logLevel;
	}

	public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, String> formatter)
	{
		if (!IsEnabled(logLevel))
			return;

		LogFactory.Service.LogApplication(
			categoryName,
			logLevel,
			eventId,
			state,
			exception,
			formatter
		);
	}
}
