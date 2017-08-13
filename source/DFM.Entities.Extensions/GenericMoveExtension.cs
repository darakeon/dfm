using System;
using System.Linq;
using DFM.Entities.Bases;
using DFM.Entities.Enums;

namespace DFM.Entities.Extensions
{
    public static class GenericMoveExtension
    {
        public static Boolean IsDetailed(this IMove move)
        {
            return !move.hasJustOneDetail()
                    || move.hasFirstDetailDescription();
        }

        private static Boolean hasJustOneDetail(this IMove move)
        {
            return move.DetailList.Count == 1;
        }

        private static Boolean hasFirstDetailDescription(this IMove move)
        {
            var detail = move.DetailList.First();

            return !String.IsNullOrEmpty(detail.Description)
                && detail.Description != move.Description;
        }

        public static Boolean HasDetails(this IMove move)
        {
            return move.DetailList.Any();
        }

        public static Int32 PositionInSchedule(this Move move)
        {
            var schedule = move.Schedule;

            var diff = 0;

            if (schedule == null)
                return diff;

            var days = move.Date - schedule.Date;
            var month = move.Date.Month - schedule.Date.Month;
            var year = move.Date.Year - schedule.Date.Year;

            switch (schedule.Frequency)
            {
                case ScheduleFrequency.Daily:
                    diff = (Int32) days.TotalDays;
                    break;

                case ScheduleFrequency.Monthly:
                    diff = month + year * 12;
                    break;

                case ScheduleFrequency.Yearly:
                    diff = year;
                    break;

                default:
                    throw new NotImplementedException();
            }

            return diff + 1;
        }


    }
}
