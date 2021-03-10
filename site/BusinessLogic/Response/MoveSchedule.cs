using System;
using DFM.Entities;
using DFM.Entities.Bases;
using DFM.Entities.Enums;

namespace DFM.BusinessLogic.Response
{
	public class MoveSchedule
	{
		internal MoveSchedule(Schedule schedule)
		{
			Guid = schedule.Guid;
			Start = schedule.GetDate();
			Frequency = schedule.Frequency;
			Times = schedule.Times;
		}

		public Guid Guid { get; }
		public DateTime Start { get; }
		public ScheduleFrequency Frequency { get; }
		public Int16 Times { get; }

		public override bool Equals(object obj)
		{
			return obj is MoveSchedule schedule
				&& schedule.Guid == Guid;
		}

		public override int GetHashCode()
		{
			return Guid.GetHashCode();
		}
	}
}
