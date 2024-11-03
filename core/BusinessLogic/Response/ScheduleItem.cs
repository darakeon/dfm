using System;
using System.Linq;
using DFM.Entities;
using DFM.Entities.Enums;

namespace DFM.BusinessLogic.Response
{
	public class ScheduleItem : ScheduleInfo
	{
		public String OutName { get; private init; }
		public String InName { get; private init; }

		public Int16 Run { get; set; }
		public Int16 Deleted { get; set; }
		public ScheduleStatus Status { get; set; }

		internal static ScheduleItem Convert(Schedule schedule)
		{
			return new()
			{
				Guid = schedule.Guid,
				OutUrl = schedule.Out?.Url,
				InUrl = schedule.In?.Url,
				CategoryName = schedule.Category?.Name,
				OutName = schedule.Out?.Name,
				InName = schedule.In?.Name,

				Description = schedule.Description,
				Year = schedule.Year,
				Month = schedule.Month,
				Day = schedule.Day,

				Nature = schedule.Nature,

				Value = schedule.Value,
				Conversion = schedule.Conversion,
				DetailList = schedule.DetailList
					.Select(DetailInfo.Convert)
					.ToList(),

				Frequency = schedule.Frequency,
				Boundless = schedule.Boundless,
				Times = schedule.Times,
				Run = schedule.LastRun,
				Deleted = schedule.Deleted,
				Status = schedule.LastStatus,
				ShowInstallment = schedule.ShowInstallment,
			};
		}
	}
}
