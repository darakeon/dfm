using DFM.BusinessLogic.Helpers;
using DFM.Entities;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;

namespace DFM.Repositories.Mappings
{
	public class MoveMap : IAutoMappingOverride<Move>
	{
		public void Override(AutoMapping<Move> mapping)
		{
			mapping.Map(m => m.Description)
				.Length(MaximumLength.Move_Description);

			mapping.References(m => m.Schedule)
				.Not.Update()
				.Nullable();

			mapping.IgnoreProperty(m => m.FakeID);
			mapping.IgnoreProperty(m => m.Value);
			mapping.IgnoreProperty(m => m.User);

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
