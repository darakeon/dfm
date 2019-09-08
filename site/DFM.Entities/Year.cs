using System;
using System.Collections.Generic;
using DFM.Entities.Bases;

namespace DFM.Entities
{
	public partial class Year : ISummarizable
	{
		public Year()
		{
			init();
		}


		public virtual Int64 ID { get; set; }

		public virtual Int16 Time { get; set; }

		public virtual Account Account { get; set; }

		public virtual IList<Month> MonthList { get; set; }
		public virtual IList<Summary> SummaryList { get; set; }



	}
}
