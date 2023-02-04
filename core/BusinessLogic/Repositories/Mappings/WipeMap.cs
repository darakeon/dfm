using DFM.Entities;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;

namespace DFM.BusinessLogic.Repositories.Mappings
{
	// ReSharper disable once UnusedMember.Global
	public class WipeMap : IAutoMappingOverride<Wipe>
	{
		public void Override(AutoMapping<Wipe> mapping)
		{
			mapping.Table($"`{nameof(Wipe).ToLower()}`");

			mapping.Map(p => p.When)
				.Column("When_");

			mapping.Map(p => p.S3)
				.Nullable();
		}
	}
}
