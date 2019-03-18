using System;
using DFM.Entities;
using DFM.Entities.Enums;
using DK.NHibernate.Base;

namespace DFM.Tests.BusinessLogic.Helpers
{
	class FakeRepos
	{
		public static SecurityRepository Security = new SecurityRepository();
		public static TicketRepository Ticket = new TicketRepository();
		public static ContractRepository Contract = new ContractRepository();
		public static UserRepository User = new UserRepository();

		public class SecurityRepository : BaseRepository<Security>
		{
			public String GetLastTokenForUser(String email, SecurityAction action)
			{
				return SingleOrDefault(
					s => s.User.Email == email
					     && s.Action == action
					     && s.Expire >= DateTime.Now
				).Token;
			}
		}

		public class TicketRepository : BaseRepository<Ticket>
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

		public class ContractRepository : BaseRepository<Contract>
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

		public class UserRepository : BaseRepository<User>
		{
			public String GetTFAUser(String email)
			{
				return SingleOrDefault(
					t => t.Email == email
				).TFASecret;
			}
		}
	}
}