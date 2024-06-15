using DFM.Entities;
using DFM.Generic;

namespace DFM.Queue.TestApp
{
	internal class Program
	{
		public static void Main()
		{
			Cfg.Init("amazon");

			var sqs = new SQSService();


			var line = new Line{ID = 1};
			var taskSend = sqs.Enqueue(
				new List<Line> { line }
			);

			taskSend.Wait();

			Console.WriteLine(taskSend.Result);


			var taskReceived = sqs.Dequeue();

			taskReceived.Wait();

			Console.WriteLine(taskReceived.Result);


			sqs.Delete(taskReceived.Result?.Key);
		}
	}
}
