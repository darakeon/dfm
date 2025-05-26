using System;
using DFM.Entities.Enums;

namespace DFM.Authentication
{
	public class ClientTicket
	{
		public ClientTicket(String key, TicketType type)
		{
			Key = key;
			Type = type;
		}

		public String Key { get; }
		public TicketType Type { get; }
	}
}
