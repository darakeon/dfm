using System;
using DFM.Entities.Enums;

namespace DFM.Entities.Extensions
{
    public static class SummaryExtension
    {
        public static Double Value(this Summary summary)
        {
            return Math.Round(summary.In - summary.Out, 2);
        }



        public static String UniqueID(this Summary summary)
        {
            var yearID =
                summary.Nature == SummaryNature.Year
                    ? summary.Year.ID : 0;

            var monthID =
                summary.Nature == SummaryNature.Month
                    ? summary.Month.ID : 0;

            return String.Format("{0}_{1}_{2}", yearID, monthID, summary.Category.ID);
        }


    }
}
