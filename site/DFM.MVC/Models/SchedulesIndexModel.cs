using System.Collections.Generic;
using System.Linq;
using DFM.BusinessLogic.Response;

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

		public IList<ScheduleInfo> ScheduleList { get; set; }
	}
}
