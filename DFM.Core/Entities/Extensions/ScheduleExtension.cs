using System;
using System.Linq;

namespace DFM.Core.Entities.Extensions
{
    public static class ScheduleExtension
    {
        public static Boolean Contains(this Schedule schedule, Move move)
        {
            return schedule.MoveList.Contains(move);
        }

    }
}
