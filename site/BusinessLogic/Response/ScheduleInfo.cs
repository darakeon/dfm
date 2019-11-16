using System;
using System.Collections.Generic;
using System.Linq;
using DFM.Entities;
using DFM.Entities.Enums;

namespace DFM.BusinessLogic.Response
{
	public class ScheduleInfo : IMoveInfo
	{
		public ScheduleInfo()
		{
			Times = 1;
			Frequency = ScheduleFrequency.Monthly;
			DetailList = new List<DetailInfo>();
		}

		public Int64 ID { get; set; }

		public String OutUrl { get; set; }
		public String InUrl { get; set; }
		public String CategoryName { get; set; }
		public String OutName { get; private set; }
		public String InName { get; private set; }

		public String Description { get; set; }

		public Int16 Year { get; set; }
		public Int16 Month { get; set; }
		public Int16 Day { get; set; }

		public MoveNature Nature { get; set; }

		public Decimal Total { get; private set; }
		public Decimal? Value { get; set; }
		public IList<DetailInfo> DetailList { get; set; }

		public ScheduleFrequency Frequency { get; set; }
		public Boolean Boundless { get; set; }
		public Int16 Times { get; set; }
		public Boolean ShowInstallment { get; set; }

		internal void Update(Schedule schedule)
		{
			schedule.Description = Description;
			schedule.Year = Year;
			schedule.Month = Month;
			schedule.Day = Day;

			schedule.Nature = Nature;

			schedule.Value = Value;
			schedule.DetailList = DetailList
				.Select(d => d.Convert())
				.ToList();

			schedule.Frequency = Frequency;
			schedule.Boundless = Boundless;
			schedule.Times = Times;
			schedule.ShowInstallment = ShowInstallment;
		}

		internal static ScheduleInfo Convert(Schedule schedule)
		{
			return new ScheduleInfo
			{
				ID = schedule.ID,
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

				Total = schedule.Total(),
				Value = schedule.Value,
				DetailList = schedule.DetailList
					.Select(DetailInfo.Convert)
					.ToList(),

				Frequency = schedule.Frequency,
				Boundless = schedule.Boundless,
				Times = schedule.Times,
				ShowInstallment = schedule.ShowInstallment,
			};
		}
	}
}
