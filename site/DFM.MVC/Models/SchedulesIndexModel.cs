using System.Collections.Generic;
using System.Linq;
using DFM.Entities;

namespace DFM.MVC.Models
{
	public class SchedulesIndexModel : BaseLoggedModel
	{
		public SchedulesIndexModel()
		{
			ScheduleList = 
				Current.User.ScheduleList
					.Where(a => a.Active)
					.OrderByDescending(a => a.Date)
					.ToList();
		}

		public IList<Schedule> ScheduleList { get; set; }
	}
}