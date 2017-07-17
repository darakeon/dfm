using System;
using DFM.Core.Helpers;

namespace DFM.Core.Enums
{
    public enum ScheduleFrequency
    {
        Daily,
        Monthly,
        Yearly,
    }

    public static class ScheduleFrequencyExtension
    {
        public static DateTime Next(this ScheduleFrequency frequency, DateTime date)
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
                    throw new DFMCoreException("ScheduleFrequencyNotRecognized");
            }
        }
    }
}
