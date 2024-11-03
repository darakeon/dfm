using System;
using System.Collections.Generic;
using System.Linq;
using DFM.BusinessLogic.Exceptions;
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

		public Guid Guid { get; set; }

		public String OutUrl { get; set; }
		public String InUrl { get; set; }
		public String CategoryName { get; set; }

		public String Description { get; set; }

		public Int16 Year { get; set; }
		public Int16 Month { get; set; }
		public Int16 Day { get; set; }

		public MoveNature Nature { get; set; }

		public Decimal Value { get; set; }
		public Decimal? Conversion { get; set; }
		public IList<DetailInfo> DetailList { get; set; }

		public ScheduleFrequency Frequency { get; set; }
		public Boolean Boundless { get; set; }
		public Int16 Times { get; set; }
		public Boolean ShowInstallment { get; set; }

		internal void Update(Schedule schedule)
		{
			if (Guid != schedule.Guid)
				throw Error.InvalidSchedule.Throw();

			schedule.Description = Description;
			schedule.Year = Year;
			schedule.Month = Month;
			schedule.Day = Day;

			schedule.Nature = Nature;

			schedule.Value = Value;
			schedule.Conversion = Conversion;
			schedule.DetailList = DetailList
				.Select(d => d.Convert())
				.ToList();

			schedule.Frequency = Frequency;
			schedule.Boundless = Boundless;
			schedule.Times = Times;
			schedule.ShowInstallment = ShowInstallment;
		}
	}
}
