using System;
using System.Collections.Generic;
using System.Linq;

namespace DFM.Entities.Extensions
{
    public static class MonthExtension
    {
        public static User User(this Month month)
        {
            return month.Account().User;
        }

        public static Account Account(this Month month)
        {
            return month.Year.Account;
        }

        public static IList<Move> MoveList(this Month month)
        {
            var list = new List<Move>();

            list.AddRange(month.OutList);
            list.AddRange(month.InList);

            return list.OrderBy(m => m.ID).ToList();
        }



        public static void AddOut(this Month month, Move move)
        {
            move.Out = month;
            month.OutList.Add(move);
        }

        public static void AddIn(this Month month, Move move)
        {
            move.In = month;
            month.InList.Add(move);
        }



        public static Boolean OutContains(this Month month, Move move)
        {
            return month.OutList.Contains(move);
        }

        public static Boolean InContains(this Month month, Move move)
        {
            return month.InList.Contains(move);
        }

    }
}
