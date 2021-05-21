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

			Console.WriteLine($"Starting {arg}");

			Connection.Run(() =>
			{
				var service = new Service(arg);
				service.Execute();
			});

			Console.WriteLine($"Ended {arg}");
		}
	}
}
