using System;
using DFM.Entities.Bases;
using DFM.Entities.Enums;
using DFM.Generic;

namespace DFM.Entities
{
    public partial class Summary
    {
		private void init()
		{
			Broken = true;
		}

		private void init(Month month, Category category)
		{
			Month = month;
			Category = category;
			Nature = SummaryNature.Month;
		}

		private void init(Year year, Category category)
		{
			Year = year;
			Category = category;
			Nature = SummaryNature.Year;
		}


		public virtual Double In
		{
			get { return InCents.ToVisual(); }
			set { InCents = value.ToCents(); }
		}

		public virtual Double Out
		{
			get { return OutCents.ToVisual(); }
			set { OutCents = value.ToCents(); }
		}


	    public override String ToString()
		{
			return String.Format("[{0}] {1}", ID, In - Out);
		}


        public virtual Double Value()
        {
            return Math.Round(In - Out, 2);
        }



        public virtual String UniqueID()
        {
            var yearID =
                Nature == SummaryNature.Year
                    ? Year.ID : 0;

            var monthID =
                Nature == SummaryNature.Month
                    ? Month.ID : 0;

            var category = Category;
            var categoryID = category == null ? 0 : category.ID;

            return String.Format("{0}_{1}_{2}", yearID, monthID, categoryID);
        }



        public virtual ISummarizable Parent()
        {
            switch (Nature)
            {
                case SummaryNature.Year:
                    return Year;
                case SummaryNature.Month:
                    return Month;
                default:
                    throw new NotImplementedException();
            }
        }



        public virtual User User()
        {
            switch (Nature)
            {
                case SummaryNature.Year:
                    return Year.User();
                case SummaryNature.Month:
                    return Month.User();
                default:
                    throw new NotImplementedException();
            }
        }



    }
}
