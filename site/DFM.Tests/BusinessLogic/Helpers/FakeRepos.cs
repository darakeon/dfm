using System;
using DFM.Entities;
using DFM.Entities.Enums;
using Keon.NHibernate.Base;

namespace DFM.Tests.BusinessLogic.Helpers
{
	class FakeRepos
	{
		public static SecurityRepository Security = new SecurityRepository();
		public static TicketRepository Ticket = new TicketRepository();
		public static ContractRepository Contract = new ContractRepository();
		public static UserRepository User = new UserRepository();
		public static ScheduleRepository Schedule = new ScheduleRepository();

		public class SecurityRepository : BaseRepositoryLong<Security>
		{
			public String GetLastTokenForUser(String email, SecurityAction action)
			{
				return NewQuery()
					.SimpleFilter(
						s => s.User.Email == email
						     && s.Action == action
						     && s.Expire >= DateTime.Now
					)
					.OrderBy(s => s.ID, false)
					.FirstOrDefault
					.Token;
			}
		}

		public class TicketRepository : BaseRepositoryLong<Ticket>
		{
			public String GetLastTicketForUser(String email)
			{
				return SingleOrDefault(
					t => t.User.Email == email
					     && t.Expiration == null
				).Key;
			}

			public String GetUserEmailByTicket(String ticket)
			{
				return SingleOrDefault(
					t => t.Key == ticket
					     && t.Active
				).User.Email;
			}
		}

		public class ContractRepository : BaseRepositoryLong<Contract>
		{
			public void Create(String contractVersion)
			{
				var contract = new Contract
				{
					Version = contractVersion,
					BeginDate = DateTime.Now,
				};

				SaveOrUpdate(contract);
			}
		}

		public class UserRepository : BaseRepositoryLong<User>
		{
			public String GetTFAUser(String email)
			{
				return SingleOrDefault(
					t => t.Email == email
				).TFASecret;
			}
		}

		public class ScheduleRepository : BaseRepositoryLong<Schedule>
		{
			public Boolean GetState(Int64 id)
			{
				return SingleOrDefault(
					t => t.ID== id
				).Active;
			}
		}
	}
}
