using System;

namespace DFM.Generic
{
    public class PseudoTicket
    {
        internal PseudoTicket(String key, TicketType type)
        {
            Key = key;
            Type = type;
        }

        public String Key { get; private set; }
        public TicketType Type { get; private set; }

    }



}
