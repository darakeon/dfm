using System;
using DFM.BusinessLogic.Bases;
using DFM.Entities;
using DFM.Generic;

namespace DFM.BusinessLogic.Services
{
    internal class TicketService : BaseService<Ticket>
    {
        internal TicketService(IRepository<Ticket> repository) : base(repository) { }

        internal Ticket Create(Ticket ticket)
        {
            ticket.Key = Token.New();
            ticket.Creation = DateTime.Now;
            ticket.Active = true;

            return SaveOrUpdate(ticket);
        }

        internal Ticket SelectByKey(String key)
        {
            return SingleOrDefault(t => t.Key == key);
        }

        internal void Disable(Ticket ticket)
        {
            ticket.Active = false;
            SaveOrUpdate(ticket);
        }

    }

}
