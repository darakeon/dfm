using System.Runtime.CompilerServices;
using DFM.BusinessLogic.Repositories;
using DFM.BusinessLogic.Services;
using DFM.Exchange.Exporter;
using DFM.BusinessLogic.Validators;

[assembly: InternalsVisibleTo("DFM.BusinessLogic.Tests")]
namespace DFM.BusinessLogic
{
	public class ServiceAccess
	{
		public ServiceAccess(
			Current.GetTicket getTicket,
			Current.GetUrl getUrl,
			IFileService fileService
		)
		{
			var valids = new Valids();
			var repos = new Repos(getUrl, valids, fileService);

			BaseMove = new BaseMoveSaverService(this, repos, valids);

			Auth = new AuthService(this, repos, valids);
			Law = new LawService(this, repos, valids);
			Admin = new AdminService(this, repos, valids);
			Clip = new ClipService(this, repos, valids);
			Money = new MoneyService(this, repos, valids);
			Report = new ReportService(this, repos, valids);
			Robot = new RobotService(this, repos, valids);
			Outside = new OutsideService(this, repos, valids);

			Current = new Current(Auth, getTicket);
			File = fileService;
		}

		internal BaseMoveSaverService BaseMove { get; }

		public AuthService Auth { get; }
		public LawService Law { get; }
		public AdminService Admin { get; }
		public ClipService Clip { get; }
		public MoneyService Money { get; }
		public ReportService Report { get; }
		public RobotService Robot { get; }
		public OutsideService Outside { get; }

		public Current Current { get; }

		internal IFileService File { get; }
	}
}
