using DFM.Entities;
using DFM.Entities.Bases;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;

namespace DFM.BusinessLogic.Repositories.Mappings
{
	// ReSharper disable once UnusedMember.Global
	public class MoveMap : IAutoMappingOverride<Move>
	{
		public void Override(AutoMapping<Move> mapping)
		{
			mapping.IgnoreProperty(m => m.Guid);

			mapping.Map(m => m.ExternalId)
				.Not.Update()
				.Not.Nullable()
				.Unique();

			mapping.Map(m => m.Description)
				.Length(MaxLen.MoveDescription);

			mapping.References(m => m.Schedule)
				.Not.Update()
				.Nullable();

			mapping.IgnoreProperty(m => m.Value);
			mapping.IgnoreProperty(m => m.Conversion);

			mapping.References(m => m.Category)
				.Nullable();

			mapping.References(m => m.In)
				.Nullable();

			mapping.References(m => m.Out)
				.Nullable();

			mapping.HasMany(m => m.DetailList)
				.Cascade.Delete()
				.Not.LazyLoad();

		}
	}
}
