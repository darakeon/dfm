using DFM.BusinessLogic.Services;
using DFM.Entities;

namespace DFM.BusinessLogic
{
    public class DataAccess 
    {
        public DataAccess(IResolver resolver)
        {
            resolver.Register<Account>();
            resolver.Register<Category>();
            resolver.Register<Detail>();
            resolver.Register<Month>();
            resolver.Register<Move>();
            resolver.Register<Schedule>();
            resolver.Register<Security>();
            resolver.Register<Summary>();
            resolver.Register<User>();
            resolver.Register<Year>();

            Account = new AccountService(this, resolver.Resolve<Account>());
            Category = new CategoryService(this, resolver.Resolve<Category>());
            Detail = new DetailService(this, resolver.Resolve<Detail>());
            Month = new MonthService(this, resolver.Resolve<Month>());
            Move = new MoveService(this, resolver.Resolve<Move>());
            Schedule = new ScheduleService(this, resolver.Resolve<Schedule>());
            Security = new SecurityService(this, resolver.Resolve<Security>());
            Summary = new SummaryService(this, resolver.Resolve<Summary>());
            User = new UserService(this, resolver.Resolve<User>());
            Year = new YearService(this, resolver.Resolve<Year>());
        }


        public AccountService Account { get; private set; }
        public CategoryService Category { get; private set; }
        public DetailService Detail { get; private set; }
        internal MonthService Month { get; private set; }
        public MoveService Move { get; private set; }
        public ScheduleService Schedule { get; private set; }
        public SecurityService Security { get; private set; }
        internal SummaryService Summary { get; private set; }
        public UserService User { get; private set; }
        internal YearService Year { get; private set; }

    }
}
