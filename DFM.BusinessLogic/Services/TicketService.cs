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

            ticket = SaveOrUpdate(ticket);

            return ticket;
        }



    }

}
