using DFM.Authentication;
using DFM.BusinessLogic.Repositories;
using DFM.BusinessLogic.Services;
using DFM.Entities;
using DFM.BusinessLogic.Bases;

namespace DFM.BusinessLogic
{
    public class ServiceAccess 
    {
        public ServiceAccess(IConnector resolver)
        {
            var account = new AccountRepository(resolver.Resolve<Account>());
            var category = new CategoryRepository(resolver.Resolve<Category>());
            var detail = new DetailRepository(resolver.Resolve<Detail>());
            var month = new MonthRepository(resolver.Resolve<Month>());
            var move = new MoveRepository(resolver.Resolve<Move>());
            var schedule = new ScheduleRepository(resolver.Resolve<Schedule>());
            var security = new SecurityRepository(resolver.Resolve<Security>());
            var summary = new SummaryRepository(resolver.Resolve<Summary>());
            var ticket = new TicketRepository(resolver.Resolve<Ticket>());
            var user = new UserRepository(resolver.Resolve<User>());
            var year = new YearRepository(resolver.Resolve<Year>());

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
