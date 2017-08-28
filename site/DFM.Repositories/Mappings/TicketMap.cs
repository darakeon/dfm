using DFM.Entities;
using DFM.BusinessLogic.Helpers;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;

namespace DFM.Repositories.Mappings
{
    public class TicketMap : IAutoMappingOverride<Ticket>
    {
        public void Override(AutoMapping<Ticket> mapping)
        {
            mapping.Map(t => t.Key, "Key_")
                .Length(MaximumLength.TICKET_KEY)
                .Unique();

            mapping.Map(t => t.Active)
                .Default("'1'");

            mapping.Map(t => t.Expiration)
                .Nullable();
        }
    }
}
