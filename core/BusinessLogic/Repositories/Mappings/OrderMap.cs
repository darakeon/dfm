using DFM.Entities;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;

namespace DFM.BusinessLogic.Repositories.Mappings;

// ReSharper disable once UnusedMember.Global
public class OrderMap : IAutoMappingOverride<Order>
{
	public void Override(AutoMapping<Order> mapping)
	{
		mapping.Table("Order_");

		mapping.IgnoreProperty(o => o.Guid);

		mapping.Map(o => o.ExternalId)
			.UniqueKey("UK_Order");

		mapping.References(o => o.User)
			.Not.Nullable();
	}
}
