using System;
using System.Linq;
using DFM.Entities.Enums;
using DFM.Entities.Extensions;

namespace DFM.Entities
{
    public partial class Account
    {
        public virtual Double Sum()
        {
            return YearList.Sum(m => m.Sum());
        }

        public virtual Boolean IsOpen()
        {
            return EndDate == null;
        }

        public virtual Boolean HasMoves()
        {
            return YearList.Any(
                    y => y.MonthList.Any(
                            m => m.MoveList().Any()
                        )
                );
        }

        public virtual Boolean AuthorizeCRUD(User user)
        {
            return User == user;
        }

        public virtual AccountSign? Sign()
        {
            var hasRed = RedLimit != null;
            var hasYellow = YellowLimit != null;
            var sum = Sum();

            if (hasRed && sum < RedLimit)
                return AccountSign.Red;
            
            if (hasYellow && sum < YellowLimit)
                return AccountSign.Yellow;

            if (hasRed || hasYellow)
                return AccountSign.Green;
            
            return null;
        }

    }
}
