using System;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace DFM.Logs.Data.Application;

internal class ApplicationLog<TState>(
	String category,
	LogLevel logLevel,
	EventId eventId,
	TState state,
	Exception? exception,
	Func<TState, Exception?, String> formatter
)
{
	public String Category { get; } = category;
	public String LogLevel { get; } = logLevel.ToString();
	public EventId EventId { get; } = eventId;
	public String Message { get; } = formatter(state, exception);

	public override String ToString()
	{
		return JsonConvert.SerializeObject(
			this,
			Formatting.Indented
		);
	}
}
