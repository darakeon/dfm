using DFM.Entities;
using DFM.Entities.Bases;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;

namespace DFM.BusinessLogic.Repositories.Mappings;

// ReSharper disable once UnusedMember.Global
public class PlanMap : IAutoMappingOverride<Plan>
{
	public void Override(AutoMapping<Plan> mapping)
	{
		mapping.Map(l => l.Name)
			.Length(MaxLen.PlanName)
			.UniqueKey("UK_Plan");

		mapping.IgnoreProperty(l => l.Price);
	}
}
