using DFM.Generic;
using System;
using System.Linq;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Amazon.Lambda.RuntimeSupport;
using Amazon.Lambda.Serialization.SystemTextJson;

namespace DFM.Robot
{
	class Program
	{
		public static async Task Main(String[] args)
		{
			Log.Ok("Starting lambda async");

			try
			{
				var handler = FunctionHandler;
				await LambdaBootstrapBuilder
					.Create(handler, new DefaultLambdaJsonSerializer())
					.Build()
					.RunAsync();
			}
			catch (UriFormatException) // not lambda
			{
				var task = getTask(args);
				main(task);
			}

			Log.Ok("Ended lambda async");
		}
		
		public static String FunctionHandler(String input, ILambdaContext context)
		{
			var task = getTask(input);
			main(task);
			return input.ToUpper();
		}

		private static void main(RobotTask task)
		{
			Log.Ok($"Starting {task}");

			Connection.Run(() =>
			{
				var service = new Service(task);
				var process = service.Execute(Log.Ok);
				process.Wait();

				if (process.Exception != null)
					Log.Error(process.Exception);
			});

			Log.Ok($"Ended {task}");
		}

		private static RobotTask getTask(String[] args)
		{
			var arg = args.FirstOrDefault()
			    ?? Environment.GetEnvironmentVariable("TASK");

			if (arg == null)
				throw new ArgumentException("Not task passed");

			return getTask(arg);
		}

		private static RobotTask getTask(String taskName)
		{
			var task = EnumX.Parse<RobotTask>(taskName);

			if (!EnumX.AllValues<RobotTask>().Contains(task))
				throw new ArgumentException("Invalid task");

			return task;
		}
	}
}
