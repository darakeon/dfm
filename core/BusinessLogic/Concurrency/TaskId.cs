using System;

namespace DFM.BusinessLogic.Concurrency
{
	internal class TaskId
	{
		internal static TaskId New()
		{
			return new(Guid.NewGuid(), DateTime.Today);
		}

		internal static TaskId Dead()
		{
			return new(Guid.Empty, DateTime.Today.AddDays(-1));
		}

		private TaskId(Guid guid, DateTime date)
		{
			ID = guid;
			Date = date;
		}

		public Guid ID { get; }
		public DateTime Date { get; }

		public TaskId FirstToday(TaskId other)
		{
			return other.Date < DateTime.Today
				? this
				: other;
		}

		public Boolean Equals(TaskId obj)
		{
			return obj.ID == ID;
		}
	}
}
