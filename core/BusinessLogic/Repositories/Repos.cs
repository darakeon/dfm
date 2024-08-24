using DFM.Authentication;
using DFM.BusinessLogic.Response;
using DFM.BusinessLogic.Validators;
using DFM.Files;

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
			IFileService fileService
		)
		{
			Acceptance = new AcceptanceRepository();
			Account = new AccountRepository();
			Archive = new ArchiveRepository();
			Category = new CategoryRepository();
			Settings = new SettingsRepository();
			Contract = new ContractRepository();
			Control = new ControlRepository(getUrl);
			Detail = new DetailRepository();
			Line = new LineRepository();
			Move = new MoveRepository(getUrl, valids.Move);
			Wipe = new WipeRepository(this, getUrl, fileService);
			Schedule = new ScheduleRepository(valids.Schedule);
			Security = new SecurityRepository(getUrl);
			Summary = new SummaryRepository();
			Ticket = new TicketRepository();
			Tips = new TipsRepository();
			User = new UserRepository();
		}

	}
}
