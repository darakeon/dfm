using System;

namespace DFM.MVC.Areas.API.Jsons
{
    public class DateJson : IComparable<DateJson>
    {
        public DateJson() { }

        public DateJson(DateTime dateTime) : this()
        {
            Year = (Int16)dateTime.Year;
            Month = (Int16)dateTime.Month;
            Day = (Int16) dateTime.Day;
        }

        public Int16 Year { get; set; }
        public Int16 Month { get; set; }
        public Int16 Day { get; set; }


        public int CompareTo(DateJson obj)
        {
            var year = Year.CompareTo(obj.Year);

            if (year != 0)
                return year;

            var month = Month.CompareTo(obj.Month);

            if (month != 0)
                return month;

            return Day.CompareTo(obj.Day);
        }

	    public DateTime ToSystemDate()
	    {
		    return new DateTime(Year, Month, Day);
	    }

	    public override string ToString()
	    {
		    var year = Year.ToString("0000");
		    var month = Month.ToString("00");
		    var day = Day.ToString("00");

			return $"{year}-{month}-{day}";
	    }
    }
}