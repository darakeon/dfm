using DFM.Generic;
using System;
using System.Linq;

namespace DFM.Robot
{
	class Program
	{
		public static void Main(String[] args)
		{
			var task = getTask(args);

			LogJson.Ok($"Starting {task}");

			Connection.Run(() =>
			{
				var service = new Service(task);
				var process = service.Execute(LogJson.Ok);
				process.Wait();

				if (process.Exception != null)
					LogJson.Error(process.Exception);
			});

			LogJson.Ok($"Ended {task}");
		}

		private static RobotTask getTask(string[] args)
		{
			var arg = args.FirstOrDefault()
			    ?? Environment.GetEnvironmentVariable("TASK");

			if (arg == null)
				throw new ArgumentException("Not task passed");

			var task = EnumX.Parse<RobotTask>(arg);

			if (!EnumX.AllValues<RobotTask>().Contains(task))
				throw new ArgumentException("Invalid task");
			
			return task;
		}
	}
}
