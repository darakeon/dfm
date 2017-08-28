using DFM.Authentication;
using DFM.BusinessLogic.Repositories;
using DFM.BusinessLogic.Services;

namespace DFM.BusinessLogic
{
    public class ServiceAccess 
    {
        public ServiceAccess()
        {
            var account = new AccountRepository();
            var category = new CategoryRepository();
            var config = new ConfigRepository();
            var detail = new DetailRepository();
            var month = new MonthRepository();
            var move = new MoveRepository();
            var schedule = new ScheduleRepository();
            var security = new SecurityRepository();
            var summary = new SummaryRepository();
            var ticket = new TicketRepository();
            var user = new UserRepository();
            var year = new YearRepository();

            BaseMove = new BaseMoveSaverService(this, move, detail, summary, month, year);

            Safe = new SafeService(this, user, security, ticket);
            Admin = new AdminService(this, account, category, year, month, summary, config, schedule);
            Money = new MoneyService(this, move, detail, schedule);
            Robot = new RobotService(this, schedule, detail);
            Report = new ReportService(this, account, year, month);

            Current = new Current(Safe);
        }


        internal BaseMoveSaverService BaseMove { get; private set; }

        public MoneyService Money { get; private set; }
        public ReportService Report { get; private set; }
        public SafeService Safe { get; }
        public AdminService Admin { get; private set; }
        public RobotService Robot { get; private set; }

        public Current Current { get; private set; }

    }
}
