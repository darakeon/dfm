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

			Console.WriteLine($"Starting {task}");

			Connection.Run(() =>
			{
				var service = new Service(task);
				service.Execute();
			});

			Console.WriteLine($"Ended {task}");
		}
	}
}
