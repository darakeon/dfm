using System;
using System.Collections.Concurrent;

namespace DFM.BusinessLogic.Concurrency
{
	class Tasks : ConcurrentDictionary<string, TaskId>
	{
		public Boolean FirstToday(String key)
		{
			var taskId = TaskId.New();

			var run = AddOrUpdate(
				key, taskId,
				(_, oldValue) => taskId.FirstToday(oldValue)
			);

			return taskId.Equals(run);
		}

		public void Remove(String key)
		{
			var dead = TaskId.Dead();
			AddOrUpdate(key, dead, (_, _) => dead);
		}
	}
}
