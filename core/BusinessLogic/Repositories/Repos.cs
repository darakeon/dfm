using DFM.Authentication;
using DFM.BusinessLogic.Response;

namespace DFM.BusinessLogic.Repositories
{
	internal class Repos
	{
		internal AcceptanceRepository Acceptance;
		internal AccountRepository Account;
		internal CategoryRepository Category;
		internal ConfigRepository Config;
		internal ContractRepository Contract;
		internal ControlRepository Control;
		internal DetailRepository Detail;
		internal MoveRepository Move;
		internal PurgeRepository Purge;
		internal ScheduleRepository Schedule;
		internal SecurityRepository Security;
		internal SummaryRepository Summary;
		internal TicketRepository Ticket;
		internal UserRepository User;

		internal Repos(Current<SignInInfo, SessionInfo>.GetUrl getUrl)
		{
			Acceptance = new AcceptanceRepository();
			Account = new AccountRepository();
			Category = new CategoryRepository();
			Config = new ConfigRepository();
			Contract = new ContractRepository();
			Control = new ControlRepository(getUrl);
			Detail = new DetailRepository();
			Move = new MoveRepository(getUrl);
			Purge = new PurgeRepository(this);
			Schedule = new ScheduleRepository();
			Security = new SecurityRepository(getUrl);
			Summary = new SummaryRepository();
			Ticket = new TicketRepository();
			User = new UserRepository();
		}
	}
}
