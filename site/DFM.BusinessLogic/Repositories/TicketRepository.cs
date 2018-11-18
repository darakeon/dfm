using System;
using System.Collections.Generic;
using System.Linq;
using DFM.Entities;
using DFM.Entities.Enums;

namespace DFM.BusinessLogic.Repositories
{
	internal class TicketRepository : BaseRepository<Ticket>
	{
		internal Ticket Create(User user, String key, TicketType type)
		{
			var ticket =
				new Ticket
				{
					ID = 0,
					Key = key,
					Type = type,
					Creation = user.Now(),
					Active = true,
					User = user,
				};

			return SaveOrUpdate(ticket);
		}



		internal Ticket GetByKey(String key)
		{
			return SingleOrDefault(t => t.Key == key);
		}

		internal Ticket GetByPartOfKey(User user, String key)
		{
			return List(user)
				.SingleOrDefault(
					t => t.Key.StartsWith(key)
				);
		}



		internal IEnumerable<Ticket> List(User user)
		{
			return SimpleFilter(t => t.User.ID == user.ID && t.Active);
		}



		internal void Disable(Ticket ticket)
		{
			ticket.Key += DateTime.UtcNow.ToString("yyyyMMddHHmmssffffff");
			ticket.Active = false;
			ticket.Expiration = DateTime.UtcNow;

			SaveOrUpdate(ticket);
		}

	}

}
