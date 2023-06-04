using System.Runtime.CompilerServices;
using DFM.BusinessLogic.Repositories;
using DFM.BusinessLogic.Services;
using DFM.Exchange;

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
			var repos = new Repos(getUrl, fileService);

			BaseMove = new BaseMoveSaverService(this, repos);

			Auth = new AuthService(this, repos);
			Law = new LawService(this, repos);
			Admin = new AdminService(this, repos);
			Clip = new ClipService(this, repos);
			Money = new MoneyService(this, repos);
			Report = new ReportService(this, repos);
			Robot = new RobotService(this, repos);
			Outside = new OutsideService(this, repos);

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
