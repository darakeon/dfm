using System;
using System.Collections.Generic;
using DFM.Entities.Bases;

namespace DFM.Entities
{
	public partial class Month : ISummarizable
	{
		public Month()
		{
			init();
		}

		public virtual Int64 ID { get; set; }

		public virtual Int16 Time { get; set; }

		public virtual Year Year { get; set; }

		public virtual IList<Summary> SummaryList { get; set; }
		public virtual IList<Move> InList { get; set; }
		public virtual IList<Move> OutList { get; set; }





	}
}