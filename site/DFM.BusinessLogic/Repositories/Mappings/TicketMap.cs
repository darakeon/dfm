using DFM.Entities;
using DFM.Entities.Bases;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;

namespace DFM.BusinessLogic.Repositories.Mappings
{
	public class TicketMap : IAutoMappingOverride<Ticket>
	{
		public void Override(AutoMapping<Ticket> mapping)
		{
			mapping.Map(t => t.Key, "Key_")
				.Length(MaxLen.Ticket_Key)
				.Unique();

			mapping.Map(t => t.Active)
				.Default("1");

			mapping.Map(t => t.Expiration)
				.Nullable();
		}
	}
}
