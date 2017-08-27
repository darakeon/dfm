namespace DFM.Entities
{
    public partial class Year
    {
        public virtual User User()
        {
            return Account.User;
        }


        public virtual Year Clone()
        {
            return new Year
            {
                Account = Account,
                MonthList = MonthList,
                SummaryList = SummaryList,
                Time = Time
            };
        }

    }
}
