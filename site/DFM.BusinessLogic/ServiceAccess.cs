using System;
using DFM.Authentication;
using DFM.BusinessLogic.Helpers;
using DFM.BusinessLogic.Repositories;
using DFM.BusinessLogic.Services;

namespace DFM.BusinessLogic
{
	public class ServiceAccess
	{
		public ServiceAccess(Current.GetTicket getTicket, Func<PathType, String> getPath)
		{
			var acceptance = new AcceptanceRepository();
			var account = new AccountRepository();
			var category = new CategoryRepository();
			var config = new ConfigRepository();
			var contract = new ContractRepository();
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

			Safe = new SafeService(this, user, security, ticket, contract, acceptance, getPath);
			Admin = new AdminService(this, account, category, year, month, summary, config, schedule);
			Money = new MoneyService(this, move, detail, schedule);
			Robot = new RobotService(this, schedule, detail);
			Report = new ReportService(this, account, year, month);

			Current = new Current(Safe, getTicket);
		}

		internal BaseMoveSaverService BaseMove { get; }

		public MoneyService Money { get; }
		public ReportService Report { get; }
		public SafeService Safe { get; }
		public AdminService Admin { get; }
		public RobotService Robot { get; }

		public Current Current { get; }
	}
}
