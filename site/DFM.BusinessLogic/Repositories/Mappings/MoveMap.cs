using DFM.Entities;
using DFM.Entities.Bases;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;

namespace DFM.BusinessLogic.Repositories.Mappings
{
	public class MoveMap : IAutoMappingOverride<Move>
	{
		public void Override(AutoMapping<Move> mapping)
		{
			mapping.Map(m => m.Description)
				.Length(MaxLen.MoveDescription);

			mapping.References(m => m.Schedule)
				.Not.Update()
				.Nullable();

			mapping.IgnoreProperty(m => m.FakeID);
			mapping.IgnoreProperty(m => m.Value);
			mapping.IgnoreProperty(m => m.Date);

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
