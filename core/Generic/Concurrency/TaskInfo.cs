using System;

namespace DFM.Generic.Concurrency
{
	public class TaskInfo
	{
		internal static TaskInfo New(Int32 minutes)
		{
			return new(Guid.NewGuid(), DateTime.Now, minutes);
		}

		internal static TaskInfo Dead(Int32 minutes)
		{
			return new(Guid.Empty, DateTime.MinValue, minutes);
		}

		private TaskInfo(Guid guid, DateTime start, Int32 minutes)
		{
			ID = guid;
			Start = start;
			End = Start.AddMinutes(minutes);
		}

		public Guid ID { get; }
		public DateTime Start { get; }
		public DateTime End { get; }

		internal TaskInfo LastValid(TaskInfo other)
		{
			return other.End < Start
				? this
				: other;
		}

		public Boolean Equals(TaskInfo obj)
		{
			return obj.ID == ID;
		}
	}
}
