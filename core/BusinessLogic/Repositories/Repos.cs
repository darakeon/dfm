using DFM.Authentication;
using DFM.BusinessLogic.Response;
using DFM.BusinessLogic.Validators;
using DFM.Files;
using DFM.Logs;

namespace DFM.BusinessLogic.Repositories
{
	internal class Repos
	{
		internal AcceptanceRepository Acceptance;
		internal AccountRepository Account;
		internal ArchiveRepository Archive;
		internal CategoryRepository Category;
		internal SettingsRepository Settings;
		internal ContractRepository Contract;
		internal ControlRepository Control;
		internal DetailRepository Detail;
		internal LineRepository Line;
		internal MoveRepository Move;
		internal OrderRepository Order;
		internal PlanRepository Plan;
		internal WipeRepository Wipe;
		internal ScheduleRepository Schedule;
		internal SecurityRepository Security;
		internal SummaryRepository Summary;
		internal TicketRepository Ticket;
		internal TipsRepository Tips;
		internal UserRepository User;

		internal Repos(
			Current<SignInInfo, SessionInfo>.GetUrl getUrl,
			Valids valids,
			ILogService logService,
			IFileService wipeFileService,
			IFileService exportFileService
		)
		{
			Acceptance = new AcceptanceRepository();
			Account = new AccountRepository();
			Archive = new ArchiveRepository();
			Category = new CategoryRepository();
			Settings = new SettingsRepository();
			Contract = new ContractRepository();
			Control = new ControlRepository(getUrl, logService);
			Detail = new DetailRepository();
			Line = new LineRepository();
			Move = new MoveRepository(getUrl, valids.Move, logService);
			Order = new OrderRepository(this, getUrl, logService, exportFileService);
			Plan = new PlanRepository();
			Wipe = new WipeRepository(this, getUrl, logService, wipeFileService, exportFileService);
			Schedule = new ScheduleRepository(valids.Schedule);
			Security = new SecurityRepository(getUrl, logService);
			Summary = new SummaryRepository();
			Ticket = new TicketRepository();
			Tips = new TipsRepository();
			User = new UserRepository(getUrl, logService);
		}

	}
}
