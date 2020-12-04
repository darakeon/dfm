using System;
using DFM.Authentication;
using DFM.BusinessLogic.Helpers;
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
		internal DetailRepository Detail;
		internal MoveRepository Move;
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
			Detail = new DetailRepository();
			Move = new MoveRepository(getUrl);
			Schedule = new ScheduleRepository();
			Security = new SecurityRepository(getUrl);
			Summary = new SummaryRepository();
			Ticket = new TicketRepository();
			User = new UserRepository();
		}
	}
}
