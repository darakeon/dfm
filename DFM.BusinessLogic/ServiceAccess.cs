using DFM.BusinessLogic.Services;
using DFM.BusinessLogic.SuperServices;
using DFM.Entities;

namespace DFM.BusinessLogic
{
    public class ServiceAccess 
    {
        public ServiceAccess(IConnector resolver)
        {
            var account = new AccountService(resolver.Resolve<Account>());
            var category = new CategoryService(resolver.Resolve<Category>());
            var detail = new DetailService(resolver.Resolve<Detail>());
            var month = new MonthService(resolver.Resolve<Month>());
            var move = new MoveService(resolver.Resolve<Move>());
            var futureMove = new FutureMoveService(resolver.Resolve<FutureMove>());
            var schedule = new ScheduleService(resolver.Resolve<Schedule>());
            var security = new SecurityService(resolver.Resolve<Security>());
            var summary = new SummaryService(resolver.Resolve<Summary>());
            var user = new UserService(resolver.Resolve<User>());
            var year = new YearService(resolver.Resolve<Year>());

            Money = new MoneyService(move, detail, category, summary, month, year, account, schedule);
            Report = new ReportService(account, year, month);
            Safe = new SafeService(user, security);
            Admin = new AdminService(account, category);
            Robot = new RobotService(Money, schedule, futureMove, detail, category);
        }


        public MoneyService Money { get; private set; }
        public ReportService Report { get; private set; }
        public SafeService Safe { get; private set; }
        public AdminService Admin { get; private set; }
        public RobotService Robot { get; private set; }

    }
}
