using System;
using DFM.Entities;
using DFM.Entities.Bases;
using DFM.Entities.Enums;

namespace DFM.BusinessLogic.Response
{
	public class TicketInfo
	{
		internal static TicketInfo Convert(Ticket ticket)
		{
			return new TicketInfo
			{
				Creation = ticket.Creation,
				Expiration = ticket.Expiration,
				Key = ticket.Key.Substring(0, Defaults.TICKET_SHOWED_PART),
				Type = ticket.Type,
			};
		}

		public DateTime Creation { get; set; }
		public DateTime? Expiration { get; set; }
		public String Key { get; set; }
		public TicketType Type { get; set; }
	}
}
