using DFM.Entities;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;

namespace DFM.BusinessLogic.Repositories.Mappings
{
	// ReSharper disable once UnusedMember.Global
	public class PurgeMap : IAutoMappingOverride<Purge>
	{
		public void Override(AutoMapping<Purge> mapping)
		{
			mapping.Map(p => p.When)
				.Column("When_");

			mapping.Map(p => p.S3)
				.Nullable();

			mapping.Map(p => p.TFA)
				.Nullable();
		}
	}
}
