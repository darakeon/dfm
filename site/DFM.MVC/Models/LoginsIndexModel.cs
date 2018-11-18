using System.Collections.Generic;
using DFM.Entities;

namespace DFM.MVC.Models
{
	public class LoginsIndexModel : BaseSiteModel
	{
		public LoginsIndexModel()
		{
			LoginsList = safe.ListLogins();
		}

		public IList<Ticket> LoginsList { get; set; }
	}
}