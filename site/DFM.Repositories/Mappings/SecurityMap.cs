using DFM.Entities;
using DFM.BusinessLogic.Helpers;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;

namespace DFM.Repositories.Mappings
{
	public class SecurityMap : IAutoMappingOverride<Security>
	{
		public void Override(AutoMapping<Security> mapping)
		{
			mapping.Map(s => s.Token)
				.Length(MaximumLength.SECURITY_TOKEN)
				.Unique();

		}
	}
}
