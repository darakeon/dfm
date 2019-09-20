using System.Collections.Generic;
using DFM.BusinessLogic.Response;
using DFM.Entities;

namespace DFM.MVC.Models
{
	public class LoginsIndexModel : BaseSiteModel
	{
		public LoginsIndexModel()
		{
			LoginsList = safe.ListLogins();
		}

		public IList<TicketInfo> LoginsList { get; set; }
	}
}
