using DFM.BusinessLogic.Services;
using DFM.BusinessLogic.SuperServices;
using DFM.Entities;

namespace DFM.BusinessLogic
{
    public class ServiceAccess 
    {
        public ServiceAccess(IConnector resolver)
        {
            account = new AccountService(resolver.Resolve<Account>());
            category = new CategoryService(resolver.Resolve<Category>());
            detail = new DetailService(resolver.Resolve<Detail>());
            month = new MonthService(resolver.Resolve<Month>());
            move = new MoveService<Move>(resolver.Resolve<Move>());
            futureMove = new MoveService<FutureMove>(resolver.Resolve<FutureMove>());
            schedule = new ScheduleService(resolver.Resolve<Schedule>());
            security = new SecurityService(resolver.Resolve<Security>());
            summary = new SummaryService(resolver.Resolve<Summary>());
            user = new UserService(resolver.Resolve<User>());
            year = new YearService(resolver.Resolve<Year>());

            Money = new MoneyService(move, futureMove, detail, summary, schedule, month, year);
            Report = new ReportService(account, year, month, summary);
            Safe = new SafeService(user, security);
            Admin = new AdminService(account, category);
            Robot = new RobotService(schedule);
        }


        private AccountService account { get; set; }
        private CategoryService category { get; set; }
        private DetailService detail { get; set; }
        private MonthService month { get; set; }
        private MoveService<Move> move { get; set; }
        private MoveService<FutureMove> futureMove { get; set; }
        private ScheduleService schedule { get; set; }
        private SecurityService security { get; set; }
        private SummaryService summary { get; set; }
        private UserService user { get; set; }
        private YearService year { get; set; }


        public MoneyService Money { get; private set; }
        public ReportService Report { get; private set; }
        public SafeService Safe { get; private set; }
        public AdminService Admin { get; private set; }
        public RobotService Robot { get; private set; }

    }
}
