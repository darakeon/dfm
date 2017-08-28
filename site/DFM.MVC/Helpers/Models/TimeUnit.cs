using System;

namespace DFM.MVC.Helpers.Models
{
	public class TimeUnit
	{
		public TimeUnit(Int32 year, Int32 month)
		{
			Year = year;
			Month = month;
		}

		public TimeUnit(DateTime date)
			: this(date.Year, date.Month) { }

		public Int32 Year { get; }
		public Int32 Month { get; }

		public static TimeUnit operator +(TimeUnit unit, Int32 months)
		{
			var date = unit.ToDate().AddMonths(months);
			return new TimeUnit(date);
		}

		public static TimeUnit operator ++(TimeUnit unit)
		{
			return unit + 1;
		}

		public static TimeUnit operator -(TimeUnit unit, Int32 months)
		{
			return unit + (-months);
		}

		public static TimeUnit operator --(TimeUnit unit)
		{
			return unit - 1;
		}

		public static Boolean operator <(TimeUnit unit, DateTime dateTime)
		{
			return unit.ToDate(dateTime.Day) < dateTime;
		}

		public static Boolean operator >(TimeUnit unit, DateTime dateTime)
		{
			return unit.ToDate(dateTime.Day) > dateTime;
		}

		public static Boolean operator <=(TimeUnit unit, DateTime dateTime)
		{
			return !(unit > dateTime);
		}

		public static Boolean operator >=(TimeUnit unit, DateTime dateTime)
		{
			return !(unit < dateTime);
		}

		public static Boolean operator ==(TimeUnit unit, TimeUnit other)
		{
			return unit?.ToString() == other?.ToString();
		}

		public static Boolean operator !=(TimeUnit unit, TimeUnit other)
		{
			return unit?.ToString() != other?.ToString();
		}

		public String Label => $"{Year}-{Month.ToString("00")}";
		public String Url => $"{Year}{Month.ToString("00")}";

		public override string ToString()
		{
			return Label;
		}

		public DateTime ToDate(Int32 day = 1)
		{
			return new DateTime(Year, Month, day);
		}

		#region Equality Members
		protected bool Equals(TimeUnit other)
		{
			return Year == other.Year && Month == other.Month;
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			
			return obj.GetType() == GetType() 
			       && Equals((TimeUnit)obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				return (Year * 397) ^ Month;
			}
		}
		#endregion Equality Members
	}
}