using System;
using System.Linq;
using DFM.Core.Helpers;

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

        internal static Year GetYear(this Account account, Int32 year)
        {
            try
            {
                return account.YearList
                    .SingleOrDefault(m => m.Time == year);
            }
            catch (InvalidOperationException e)
            {
                if (e.Message == "Sequence contains more than one matching element")
                    throw new DFMCoreException("YearAmbiguousInAccount");
                throw;
            }
        }

    }
}
