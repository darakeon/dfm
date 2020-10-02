using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using DFM.BusinessLogic.Response;
using Keon.Util.Reflection;

namespace DFM.MVC.Models
{
	public class ReportsSearchModel : BaseSiteModel
	{
		public String Terms { get; set; }
		public IList<MoveInfo> MoveList { get; set; }

		public void Search()
		{
			var result = service.Report.SearchByDescription(Terms);
			MoveList = result.MoveList;
		}

		public String FieldName
		{
			get { return getName(m => m.Terms); }
		}

		private String getName(Expression<Func<ReportsSearchModel, object>> prop)
		{
			return prop.GetName();
		}
	}
}
