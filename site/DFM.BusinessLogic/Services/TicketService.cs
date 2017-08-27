using System;
using DFM.BusinessLogic.Bases;
using DFM.Entities;

namespace DFM.BusinessLogic.Services
{
    internal class TicketService : BaseService<Ticket>
    {
        internal TicketService(IRepository<Ticket> repository) : base(repository) { }

        internal Ticket Create(User user, String ticketKey)
        {
            var ticket = 
                new Ticket
                    {
                        ID = 0,
                        Key = ticketKey,
                        Creation = DateTime.Now,
                        Active = true,
                        User = user,
                    };

            return SaveOrUpdate(ticket);
        }

        internal Ticket GetByKey(String key)
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
