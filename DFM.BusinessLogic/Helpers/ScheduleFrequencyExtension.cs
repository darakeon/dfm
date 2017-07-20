using System;
using DFM.Core.Enums;
using DFM.BusinessLogic.Exceptions;

namespace DFM.BusinessLogic.Helpers
{
    public static class ScheduleFrequencyExtension
    {
        internal static DateTime Next(this ScheduleFrequency frequency, DateTime date)
        {
            switch (frequency)
            {
                case ScheduleFrequency.Daily:
                    return date.AddDays(1);
                case ScheduleFrequency.Monthly:
                    return date.AddMonths(1);
                case ScheduleFrequency.Yearly:
                    return date.AddYears(1);
                default:
                    throw DFMCoreException.WithMessage(ExceptionPossibilities.ScheduleFrequencyNotRecognized);
            }
        }

        internal static Int32 AppliedTimes(this ScheduleFrequency frequency, DateTime firstDate, DateTime lastDate)
        {
            var months = lastDate.Month - firstDate.Month;
            var years = lastDate.Year - firstDate.Year;

            switch (frequency)
            {
                case ScheduleFrequency.Daily:
                    return (lastDate - firstDate).TotalDays.toInt();
                case ScheduleFrequency.Monthly:
                    return months;
                case ScheduleFrequency.Yearly:
                    return months + (years * 12);
                default:
                    throw DFMCoreException.WithMessage(ExceptionPossibilities.ScheduleFrequencyNotRecognized);
            }
        }


        private static Int32 toInt(this Double d)
        {
            return (Int32) Math.Floor(d);
        }

    }
}
