using System.Runtime.CompilerServices;
using DFM.BusinessLogic.Repositories;
using DFM.BusinessLogic.Services;

[assembly: InternalsVisibleTo("DFM.BusinessLogic.Tests")]
namespace DFM.BusinessLogic
{
	public class ServiceAccess
	{
		public ServiceAccess(Current.GetTicket getTicket, Current.GetUrl getUrl)
		{
			var repos = new Repos(getUrl);

			BaseMove = new BaseMoveSaverService(this, repos);

			Safe = new SafeService(this, repos);
			Admin = new AdminService(this, repos);
			Money = new MoneyService(this, repos);
			Robot = new RobotService(this, repos);
			Report = new ReportService(this, repos);

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
