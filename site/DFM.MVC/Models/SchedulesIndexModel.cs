using System.Collections.Generic;
using System.Linq;
using DFM.Entities;

namespace DFM.MVC.Models
{
	public class SchedulesIndexModel : BaseSiteModel
	{
		public SchedulesIndexModel()
		{
			ScheduleList =
				robot.GetScheduleList()
					.OrderByDescending(a => a.Date)
					.ToList();
		}

		public IList<Schedule> ScheduleList { get; set; }
	}
}