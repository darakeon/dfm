using System;
using System.Linq;
using System.Linq.Expressions;
using DFM.Authentication;
using DFM.BusinessLogic.Response;
using DFM.Entities;
using DFM.Exchange;
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
			var accounts = Account.Where(a => a.User.ID == user.ID);

			var csv = new CSV();

			foreach (var account in accounts)
			{
				csv.Add(Move.ByAccount(account));
				csv.Add(Schedule.ByAccount(account));
			}

			csv.Create(user);

			purge(Ticket, t => t.User, u => u.ID == user.ID);
			purge(Security, s => s.User, u => u.ID == user.ID);
			purge(Acceptance, a => a.User, u => u.ID == user.ID);

			foreach (var account in accounts)
			{
				purge(Summary, m => m.Account, a => a.ID == account.ID);

				purge(Move, m => m.In, a => a.ID == account.ID);
				purge(Move, m => m.Out, a => a.ID == account.ID);

				purge(Schedule, m => m.In, a => a.ID == account.ID);
				purge(Schedule, m => m.Out, a => a.ID == account.ID);
			}

			purge(Account, a => a.User, u => u.ID == user.ID);
			purge(Category, c => c.User, u => u.ID == user.ID);

			User.Delete(user);
			Config.Delete(user.Config);
			Control.Delete(user.Control);
		}

		private void purge<E, P>(
			Repo<E> repo,
			Expression<Func<E, P>> parent,
			Expression<Func<P, Boolean>> condition
		)
			where E : class, IEntity<Int64>, new()
		{
			repo.NewQuery()
				.Where(parent, condition)
				.List.ToList()
				.ForEach(repo.Delete);
		}
	}
}
