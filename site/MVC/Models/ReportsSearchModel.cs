using System;
using System.Collections.Generic;
using DFM.BusinessLogic.Response;

namespace DFM.MVC.Models
{
	public class ReportsSearchModel : BaseSiteModel
	{
		public String Terms { get; set; }
		public IList<MoveInfo> MoveList { get; set; }

		public void Search()
		{
			var result = report.SearchByDescription(Terms);
			MoveList = result.MoveList;
		}

		public String FieldName =>
			getName<ReportsSearchModel>(m => m.Terms);
	}
}
