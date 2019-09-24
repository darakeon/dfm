using DFM.Entities;
using DFM.Entities.Bases;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;

namespace DFM.BusinessLogic.Repositories.Mappings
{
	public class SecurityMap : IAutoMappingOverride<Security>
	{
		public void Override(AutoMapping<Security> mapping)
		{
			mapping.Map(s => s.Token)
				.Length(MaxLen.SecurityToken)
				.Unique();

		}
	}
}
