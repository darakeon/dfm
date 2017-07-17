using System;
using System.Collections.Generic;
using System.Linq;

namespace DFM.Core.Entities.Extensions
{
    public static class MonthExtension
    {
        public static Double Value(this Month month)
        {
            return month.SummaryList.Sum(s => s.Value);
        }

        internal static Account Account(this Month month)
        {
            return month.Year.Account;
        }

        internal static IList<Move> MoveList(this Month month)
        {
            var list = new List<Move>();

            list.AddRange(month.OutList.Where(m => m.Show()));
            list.AddRange(month.InList.Where(m => m.Show()));

            return list.OrderBy(m => m.ID).ToList();
        }



        internal static void AjustSummaryList(this Month month, Category category)
        {
            if (!month.SummaryList.Any(s => s.Category == category))
                month.AjustSummaryList(new Summary { Category = category });
        }


        internal static void AddOut(this Month month, Move move)
        {
            move.Out = month;
            month.OutList.Add(move);
        }

        internal static void AddIn(this Month month, Move move)
        {
            move.In = month;
            month.InList.Add(move);
        }

    }
}
