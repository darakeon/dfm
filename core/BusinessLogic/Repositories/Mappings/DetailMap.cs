using DFM.Entities;
using DFM.Entities.Bases;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;

namespace DFM.BusinessLogic.Repositories.Mappings
{
	// ReSharper disable once UnusedMember.Global
	public class DetailMap : IAutoMappingOverride<Detail>
	{
		public void Override(AutoMapping<Detail> mapping)
		{
			mapping.IgnoreProperty(m => m.Guid);

			mapping.Map(m => m.ExternalId)
				.Not.Update()
				.Not.Nullable()
				.Unique();

			mapping.IgnoreProperty(d => d.Value);
			mapping.IgnoreProperty(d => d.Conversion);

			mapping.Map(d => d.Description)
				.Length(MaxLen.DetailDescription);

			mapping.Map(d => d.Amount)
				.Default("1");

			mapping.References(d => d.Move)
				.Nullable();

			mapping.References(d => d.Schedule)
				.Nullable();

		}
	}
}
