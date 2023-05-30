using DFM.Entities;
using DFM.Entities.Bases;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;

namespace DFM.BusinessLogic.Repositories.Mappings
{
	// ReSharper disable once UnusedMember.Global
	public class SecurityMap : IAutoMappingOverride<Security>
	{
		public void Override(AutoMapping<Security> mapping)
		{
			mapping.Map(s => s.Token)
				.Length(MaxLen.SecurityToken)
				.Unique();

			mapping.References(s => s.User)
				.Nullable();

			mapping.References(s => s.Wipe)
				.Nullable();
		}
	}
}
