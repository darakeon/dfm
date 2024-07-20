using System.Runtime.CompilerServices;
using DFM.Entities;
using DFM.Generic;

[assembly: InternalsVisibleTo("DFM.BusinessLogic.Tests")]
namespace DFM.Queue;

public class LocalQueueService : IQueueService
{
	private IDictionary<String, IDictionary<String, String>> queues =
		new Dictionary<String, IDictionary<String, String>>
		{
			{ Cfg.SQS.QueueImporter, new Dictionary<String, String>() },
		};

	private IDictionary<String, String> importer => queues[Cfg.SQS.QueueImporter];

	public Task<IDictionary<Line, Boolean>> Enqueue(IList<Line> lineList)
	{
		return Task.Run(() => enqueue(lineList));
	}

	private IDictionary<Line, Boolean> enqueue(IList<Line> lineList)
	{
		foreach (var line in lineList)
		{
			var json = SafeSerial.Serialize(line);

			importer.Add(line.ID.ToString(), json);
		}

		return lineList.ToDictionary(l => l, _ => true);
	}

	public Task<KeyValuePair<String, Line>?> Dequeue()
	{
		return Task.Run(dequeue);
	}

	private KeyValuePair<String, Line>? dequeue()
	{
		if (!importer.Any())
			return null;

		var item = importer.First();
		var json = item.Value;

		var line = SafeSerial.Deserialize<Line>(json);

		if (line == null)
			return null;

		return new KeyValuePair<String, Line>(item.Key, line);
	}

	public void Delete(String key)
	{
		importer.Remove(key);
	}

	// for tests purposes only
	internal void Expire(Line line)
	{
		importer.Remove(line.ID.ToString());
	}
}
