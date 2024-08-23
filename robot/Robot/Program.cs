using DFM.Generic;
using System;
using System.Linq;

namespace DFM.Robot
{
	class Program
	{
		public static void Main(String[] args)
		{
			var arg = args.FirstOrDefault();

			if (arg == null)
				throw new ArgumentException("Not task passed");

			var task = EnumX.Parse<RobotTask>(arg);

			if (!EnumX.AllValues<RobotTask>().Contains(task))
				throw new ArgumentException("Invalid task");

			log($"Starting {task}");

			Connection.Run(() =>
			{
				var service = new Service(task);
				var process = service.Execute();
				process.Wait();

				if (process.Exception != null)
					log(process.Exception);
			});

			log($"Ended {task}");
		}

		static void log(object msg)
		{
			Console.WriteLine($"{DateTime.Now:yyyy-MM-dd_HH-mm-ss}\t{msg}");
		}
	}
}
