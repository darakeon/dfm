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
			Frequency = ScheduleFrequency.Monthly;
			DetailList = new List<DetailInfo>();
		}

		public Int64 ID { get; set; }

		public String OutUrl { get; set; }
		public String InUrl { get; set; }
		public String CategoryName { get; set; }

		public String Description { get; set; }
		public DateTime Date { get; set; }

		public MoveNature Nature { get; set; }

		public Decimal? Value { get; set; }
		public IList<DetailInfo> DetailList { get; set; }

		public ScheduleFrequency Frequency { get; set; }
		public Boolean Boundless { get; set; }
		public Int16 Times { get; set; }
		public Boolean ShowInstallment { get; set; }

		internal void Update(Schedule schedule)
		{
			schedule.Description = Description;
			schedule.Date = Date;

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
	}
}
