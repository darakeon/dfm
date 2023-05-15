using System;
using DFM.Entities;
using DFM.Entities.Bases;
using DFM.Entities.Enums;

namespace DFM.BusinessLogic.Response
{
	public class TicketInfo
	{
		internal static TicketInfo Convert(Ticket ticket, String currentTicket)
		{
			return new()
			{
				Creation = ticket.Creation,
				Expiration = ticket.Expiration,
				Key = ticket.Key[..Defaults.TicketShowedPart],
				Type = ticket.Type,
				Current = ticket.Key == currentTicket,
			};
		}

		public DateTime Creation { get; set; }
		public DateTime? Expiration { get; set; }
		public String Key { get; set; }
		public TicketType Type { get; set; }
		public Boolean Current { get; set; }
	}
}
