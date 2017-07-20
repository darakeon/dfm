using System;
using System.Linq;
using DFM.Entities;
using DFM.Entities.Enums;

namespace DFM.Extensions
{
    public static class AccountExtension
    {
        public static Double Sum(this Account account)
        {
            return account.YearList.Sum(m => m.Sum());
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

        public static Boolean AuthorizeCRUD(this Account account, User user)
        {
            return account.User == user;
        }

        public static AccountSign? Sign(this Account account)
        {
            var hasRed = account.RedLimit != null;
            var hasYellow = account.YellowLimit != null;
            var sum = account.Sum();

            if (hasRed && sum < account.RedLimit)
                return AccountSign.Red;
            
            if (hasYellow && sum < account.YellowLimit)
                return AccountSign.Yellow;

            if (hasRed || hasYellow)
                return AccountSign.Green;
            
            return null;
        }

    }
}
