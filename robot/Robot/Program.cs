using DFM.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Amazon.Lambda.RuntimeSupport;
using Amazon.Lambda.Serialization.SystemTextJson;
using Newtonsoft.Json;
using DFM.Generic.Settings;
using DFM.Logs;

namespace DFM.Robot
{
	class Program
	{
		private const String envVarName = "ASPNETCORE_ENVIRONMENT";

		private static readonly String name = 
			Environment.GetEnvironmentVariable(envVarName);

		private static ILogService log => LogFactory.Service;


		public static async Task Main(String[] args)
		{
			Cfg.Init(name);

			await log.LogConsoleOk("Starting lambda async");

			await Connection.Run(async () =>
			{
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
					await main(task);
				}
			});

			await log.LogConsoleOk("Ended lambda async");
		}
		
		public static async Task<String> FunctionHandler(String input, ILambdaContext context)
		{
			var task = getTask(input);
			await main(task);
			return input.ToUpper();
		}

		private static async Task main(RobotTask task)
		{
			await log.LogConsoleOk($"Starting {task}");

			var service = new Service(task, log.LogConsoleOk);
			var process = service.Execute();
			await process;

			if (process.Exception != null)
				await log.LogConsoleError(process.Exception);

			await log.LogConsoleOk($"Ended {task}");
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
			if (taskName.StartsWith("{"))
			{
				var taskJson = JsonConvert
					.DeserializeObject<
						Dictionary<String, String>
					>(taskName);

				taskName = taskJson["Task"];
			}

			var task = EnumX.Parse<RobotTask>(taskName);

			if (!EnumX.AllValues<RobotTask>().Contains(task))
				throw new ArgumentException("Invalid task");

			return task;
		}
	}
}
