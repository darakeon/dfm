using System;

namespace DFM.BusinessLogic.Response
{
	public class ScheduleResult
	{
		public ScheduleResult(Guid guid)
		{
			Guid = guid;
		}

		public Guid Guid { get; set; }
	}
}
