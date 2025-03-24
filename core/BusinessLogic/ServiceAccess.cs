using System.Runtime.CompilerServices;
using DFM.BusinessLogic.Repositories;
using DFM.BusinessLogic.Services;
using DFM.BusinessLogic.Validators;
using DFM.Files;
using DFM.Logs;
using DFM.Queue;

[assembly: InternalsVisibleTo("DFM.BusinessLogic.Tests")]
namespace DFM.BusinessLogic
{
	public class ServiceAccess
	{
		public ServiceAccess(
			Current.GetTicket getTicket,
			Current.GetUrl getUrl,
			ILogService logService,
			IFileService wipeFileService,
			IFileService exportFileService,
			IQueueService queueService
		)
		{
			var valids = new Valids();
			var repos = new Repos(getUrl, valids, logService, wipeFileService, exportFileService);

			BaseMove = new BaseMoveSaverService(this, repos, valids);

			Auth = new AuthService(this, repos, valids);
			Law = new LawService(this, repos, valids);
			Admin = new AdminService(this, repos, valids);
			Clip = new ClipService(this, repos, valids);
			Money = new MoneyService(this, repos, valids);
			Report = new ReportService(this, repos, valids);
			Outside = new OutsideService(this, repos, valids);
			Attendant = new AttendantService(this, repos, valids, queueService);
			Executor = new ExecutorService(this, repos, valids, queueService);

			Current = new Current(Auth, getTicket);
		}

		internal BaseMoveSaverService BaseMove { get; }

		public AuthService Auth { get; }
		public LawService Law { get; }
		public AdminService Admin { get; }
		public ClipService Clip { get; }
		public MoneyService Money { get; }
		public ReportService Report { get; }
		public OutsideService Outside { get; }
		public AttendantService Attendant { get; }
		public ExecutorService Executor { get; }

		public Current Current { get; }
	}
}
