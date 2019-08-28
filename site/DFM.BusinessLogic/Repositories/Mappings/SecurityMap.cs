using DFM.BusinessLogic.Helpers;
using DFM.Entities;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;

namespace DFM.BusinessLogic.Repositories.Mappings
{
	public class SecurityMap : IAutoMappingOverride<Security>
	{
		public void Override(AutoMapping<Security> mapping)
		{
			mapping.Map(s => s.Token)
				.Length(MaxLen.Security_Token)
				.Unique();

		}
	}
}
