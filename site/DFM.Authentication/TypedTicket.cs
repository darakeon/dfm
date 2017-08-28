using System;
using DFM.Entities.Enums;

namespace DFM.Authentication
{
	public class TypedTicket
	{
		public TypedTicket(String key, TicketType type)
		{
			Key = key;
			Type = type;
		}

		public String Key { get; private set; }
		public TicketType Type { get; private set; }
	}
}