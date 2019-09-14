using System;
using System.Collections.Generic;
using DFM.Entities.Bases;
using DFM.Entities.Enums;

namespace DFM.Entities
{
	public partial class Move : IMove
	{
		public Move()
		{
			init();
		}

		public virtual Int64 ID { get; set; }

		public virtual String Description { get; set; }
		public virtual MoveNature Nature { get; set; }
		public virtual Int32? ValueCents { get; set; }

		public virtual DateTime Date
		{
			get
			{
				if (Day == 0)
					return default(DateTime);

				return new DateTime(Year, Month, Day);
			}
			set
			{
				Day = (Int16)value.Day;
				Month = (Int16)value.Month;
				Year = (Int16)value.Year;
			}
		}

		public virtual Int16 Day { get; set; }
		public virtual Int16 Month { get; set; }
		public virtual Int16 Year { get; set; }

		public virtual Boolean Checked { get; set; }

		public virtual Category Category { get; set; }
		public virtual Schedule Schedule { get; set; }

		public virtual IList<Detail> DetailList { get; set; }

		public virtual Account In { get; set; }
		public virtual Account Out { get; set; }
	}
}
