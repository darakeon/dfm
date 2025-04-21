using System;
using Microsoft.Extensions.Logging;

namespace DFM.Logs.Data.Application;

public class ApplicationLoggerProvider : ILoggerProvider
{
	public ILogger CreateLogger(String categoryName)
	{
		return new ApplicationLogger(categoryName);
	}

	public void Dispose()
	{
	}
}
