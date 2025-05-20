using System;

namespace DFM.Logs.Data.Console;

internal class ConsoleLog
{
	private ConsoleLog(Object message, ConsoleLogStatus status)
	{
		Status = status;
		Message = message;
	}

	public ConsoleLogStatus Status { get; private set; }
	public Object Message { get; private set; }

	public static ConsoleLog Ok(Object message)
	{
		return new ConsoleLog(message, ConsoleLogStatus.Ok);
	}

	public static ConsoleLog Error(Object message)
	{
		return new ConsoleLog(message, ConsoleLogStatus.Error);
	}

	public override String ToString()
	{
		return $"[{Status}] {Message}";
	}
}
