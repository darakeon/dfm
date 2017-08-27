using System;
using System.Collections.Generic;
using System.Linq;
using DFM.Entities.Enums;

namespace DFM.Entities
{
    public partial class Year
    {
		private void init()
		{
			MonthList = new List<Month>();
			SummaryList = new List<Summary>();
		}

		public virtual Summary AddSummary(Category category)
		{
			var summary = new Summary
			{
				Category = category,
				Year = this,
				Nature = SummaryNature.Year,
			};

			SummaryList.Add(summary);

			return summary;
		}


		public override String ToString()
		{
			return String.Format("[{0}] {1}", ID, Time);
		}


		public virtual Month this[Int32 monthDate, Boolean orNew = false]
		{
			get
			{
				var month = MonthList
					.SingleOrDefault(y => y.Time == monthDate);

				return orNew && month == null
					? new Month() : month;
			}
		}

		public virtual Summary this[String categoryName]
		{
			get
			{
				return String.IsNullOrEmpty(categoryName)
					? SummaryList.SingleOrDefault(y => y.Category == null)
					: SummaryList.SingleOrDefault(y => y.Category != null && y.Category.Name == categoryName);
			}
		}


        public virtual User User()
        {
            return Account.User;
        }


        public virtual Year Clone()
        {
            return new Year
            {
                Account = Account,
                MonthList = MonthList,
                SummaryList = SummaryList,
                Time = Time
            };
        }

    }
}
