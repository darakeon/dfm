using System;
using DFM.BusinessLogic.Bases;
using DFM.Entities;
using DFM.Entities.Extensions;

namespace DFM.BusinessLogic.Repositories
{
    internal class TicketRepository : BaseRepository<Ticket>
    {
        internal TicketRepository(IData<Ticket> repository) : base(repository) { }

        internal Ticket Create(User user, String machineId, String ticketKey)
        {
            var ticket = 
                new Ticket
                    {
                        ID = 0,
                        Key = ticketKey,
                        Creation = user.Now(),
                        Active = true,
                        User = user,
                        MachineId = machineId,
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
