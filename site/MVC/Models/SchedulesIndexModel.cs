using System.Collections.Generic;
using System.Linq;
using DFM.BusinessLogic.Response;
using DFM.Entities.Bases;

namespace DFM.MVC.Models
{
	public class SchedulesIndexModel : BaseSiteModel
	{
		public SchedulesIndexModel()
		{
			ScheduleList =
				attendant.GetScheduleList()
					.OrderByDescending(a => a.GetDate())
					.ToList();
		}

		public IList<ScheduleItem> ScheduleList { get; set; }
	}
}
