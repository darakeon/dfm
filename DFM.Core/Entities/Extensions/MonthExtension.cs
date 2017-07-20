using System;
using System.Collections.Generic;
using System.Linq;

namespace DFM.Core.Entities.Extensions
{
    public static class MonthExtension
    {
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



        internal static Boolean OutContains(this Month month, Move move)
        {
            return month.OutList.Contains(move);
        }

        internal static Boolean InContains(this Month month, Move move)
        {
            return month.InList.Contains(move);
        }

    }
}
