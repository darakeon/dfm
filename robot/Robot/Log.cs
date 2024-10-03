using System;
using System.Collections.Generic;
namespace DFM.Robot;

class Log
{
	public static void Ok(object message)
	{
		log(message, "OK");
	}

	public static void Error(object message)
	{
		log(message, "ERROR");
	}

	private static void log(object message, String status)
	{
		Console.WriteLine($"{DateTime.Now:yyyy-MM-dd_HH-mm-ss} [{status}] {message}");
	}
}
