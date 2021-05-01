using System;
using System.Linq;
using System.Linq.Expressions;
using DFM.Authentication;
using DFM.BusinessLogic.Response;
using DFM.Entities;
using Keon.Util.DB;

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
			Schedule = new ScheduleRepository();
			Security = new SecurityRepository(getUrl);
			Summary = new SummaryRepository();
			Ticket = new TicketRepository();
			User = new UserRepository();
		}

		public void Purge(User user)
		{
			purge(Ticket, s => s.User, u => u.ID == user.ID);
			purge(Security, s => s.User, u => u.ID == user.ID);
			purge(Acceptance, s => s.User, u => u.ID == user.ID);

			User.Delete(user);
			Config.Delete(user.Config);
			Control.Delete(user.Control);
		}

		private void purge<E, P>(
			Repo<E> repo,
			Expression<Func<E, P>> parent,
			Expression<Func<P, Boolean>> condition
		)
			where E : class, IEntity<long>, new()
		{
			repo.NewQuery()
				.Where(parent, condition)
				.List.ToList()
				.ForEach(repo.Delete);
		}
	}
}
