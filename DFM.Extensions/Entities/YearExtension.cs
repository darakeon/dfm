using DFM.Entities;

namespace DFM.Extensions.Entities
{
    public static class YearExtension
    {
        public static Year Clone(this Year year)
        {
            return new Year
            {
                Account = year.Account,
                MonthList = year.MonthList,
                SummaryList = year.SummaryList,
                Time = year.Time
            };
        }

    }
}
