using System.Collections.Generic;
using DFM.BusinessLogic.Response;

namespace DFM.MVC.Models
{
	public class LoginsIndexModel : BaseSiteModel
	{
		public LoginsIndexModel()
		{
			LoginsList = auth.ListLogins();
		}

		public IList<TicketInfo> LoginsList { get; set; }
	}
}
