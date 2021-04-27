using System;

namespace DFM.BusinessLogic.Concurrency
{
	internal class TaskId
	{
		private const Int32 minutes = 15;

		internal static TaskId New()
		{
			return new(Guid.NewGuid(), DateTime.Now);
		}

		internal static TaskId Dead()
		{
			return new(Guid.Empty, DateTime.MinValue);
		}

		private TaskId(Guid guid, DateTime start)
		{
			ID = guid;
			Start = start;
		}

		public Guid ID { get; }
		public DateTime Start { get; }
		public DateTime End => Start.AddMinutes(minutes);

		public TaskId LastValid(TaskId other)
		{
			return other.End < Start
				? this
				: other;
		}

		public Boolean Equals(TaskId obj)
		{
			return obj.ID == ID;
		}
	}
}
