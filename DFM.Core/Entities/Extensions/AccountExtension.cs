using System;
using System.Linq;

namespace DFM.Core.Entities.Extensions
{
    public static class AccountExtension
    {
        public static Double MovesSum(this Account account)
        {
            return account.YearList.Sum(m => m.Value());
        }

        public static Boolean Open(this Account account)
        {
            return account.EndDate == null;
        }

        public static Boolean HasMoves(this Account account)
        {
            return account.YearList.Any(
                    y => y.MonthList.Any(
                            m => m.MoveList().Any()
                        )
                );
        }

    }
}
