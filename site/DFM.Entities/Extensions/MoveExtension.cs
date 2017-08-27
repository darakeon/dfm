using System;
using System.Linq;
using DFM.Entities.Bases;

namespace DFM.Entities.Extensions
{
    public static class MoveExtension
    {
		public static Boolean HasValue(this IMove move)
		{
            return (move.Value.HasValue && move.Value != 0) ||
                move.DetailList.Any(d => d.Value != 0 && d.Amount != 0);
		}

		public static Boolean IsDetailed(this IMove move)
		{
            return !move.Value.HasValue;
		}

        public static Boolean HasCategory(this IMove move)
        {
            return move.Category != null;
        }


    }
}
