using DFM.Authentication;
using DFM.BusinessLogic.Services;
using DFM.BusinessLogic.SuperServices;
using DFM.Entities;
using DFM.BusinessLogic.Bases;

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
            var schedule = new ScheduleService(resolver.Resolve<Schedule>());
            var security = new SecurityService(resolver.Resolve<Security>());
            var summary = new SummaryService(resolver.Resolve<Summary>());
            var ticket = new TicketService(resolver.Resolve<Ticket>());
            var user = new UserService(resolver.Resolve<User>());
            var year = new YearService(resolver.Resolve<Year>());

            TransactionController = resolver.GetTransactionController();

            BaseMove = new BaseMoveSaverService(this, move, detail, summary, month, year);

            Safe = new SafeService(this, user, security, ticket);
            Admin = new AdminService(this, account, category, year, month, summary);
            Money = new MoneyService(this, move, detail, month, schedule);
            Robot = new RobotService(this, schedule, detail);
            Report = new ReportService(this, account, year, month);

            Current = new Current(Safe);
        }


        internal ITransactionController TransactionController { get; private set; }

        internal BaseMoveSaverService BaseMove { get; private set; }

        public MoneyService Money { get; private set; }
        public ReportService Report { get; private set; }
        public SafeService Safe { get; private set; }
        public AdminService Admin { get; private set; }
        public RobotService Robot { get; private set; }

        public Current Current { get; private set; } 

    }
}
