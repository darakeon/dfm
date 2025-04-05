using System;
using System.Threading.Tasks;
using DFM.Generic;

namespace DFM.Logs.TestApp;

internal class Program
{
	public static void Main(string[] args)
	{
		Cfg.Init("amazon");

		var cloudWatch = new CloudWatchService();

		var log = cloudWatch.Log(new Exception("Not handled"));
		var logHandled = cloudWatch.LogHandled(new Exception("Handled"), "Under control!");

		Task.WaitAll(log, logHandled);
	}
}
