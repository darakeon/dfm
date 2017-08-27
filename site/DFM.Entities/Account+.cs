using System;
using System.Collections.Generic;
using System.Linq;
using DFM.Entities.Enums;
using DFM.Entities.Extensions;

namespace DFM.Entities
{
    public partial class Account
    {
		private void init()
		{
			YearList = new List<Year>();
		}
		
		public override String ToString()
		{
			return String.Format("[{0}] {1}", ID, Name);
		}


		public virtual Year this[Int32 yearDate, Boolean orNew = false]
		{
			get
			{
				var year = YearList
					.SingleOrDefault(y => y.Time == yearDate);

				return orNew && year == null
					? new Year() : year;
			}
		}
		
		public virtual Double Total()
        {
            return YearList.Sum(m => m.Total());
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
            var sum = Total();

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
