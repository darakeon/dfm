using System;
using System.Collections.Concurrent;

namespace DFM.Generic.Concurrency
{
	public class Tasks
	{
		private Int32 minutes { get; }
		private ConcurrentDictionary<String, TaskInfo> dic { get; }

		public Tasks(Int32 minutes)
		{
			this.minutes = minutes;
			dic = new ConcurrentDictionary<string, TaskInfo>();
		}

		public Boolean AddIfNotExists(String key)
		{
			var taskInfo = TaskInfo.New(minutes);

			var run = dic.AddOrUpdate(
				key, taskInfo,
				(_, oldValue) => taskInfo.LastValid(oldValue)
			);

			return taskInfo.Equals(run);
		}

		public void Remove(String key)
		{
			var dead = TaskInfo.Dead(minutes);
			dic.AddOrUpdate(key, dead, (_, _) => dead);
		}

		public void Clear()
		{
			dic.Clear();
		}
	}
}
