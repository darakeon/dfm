using System;

namespace DFM.MVC.Areas.API.Helpers
{
    public class DFMDate : IComparable<DFMDate>
    {
        public DFMDate(DateTime dateTime)
        {
            Year = (Int16)dateTime.Year;
            Month = (Int16)dateTime.Month;
            Day = (Int16) dateTime.Day;
        }

        public Int16 Year { get; private set; }
        public Int16 Month { get; private set; }
        public Int16 Day { get; private set; }


        public int CompareTo(DFMDate obj)
        {
            var year = Year.CompareTo(obj.Year);

            if (year != 0)
                return year;

            var month = Month.CompareTo(obj.Month);

            if (month != 0)
                return month;

            return Day.CompareTo(obj.Day);
        }

    }
}