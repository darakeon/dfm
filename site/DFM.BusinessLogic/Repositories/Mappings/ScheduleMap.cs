using DFM.Entities;
using DFM.Entities.Bases;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;

namespace DFM.BusinessLogic.Repositories.Mappings
{
	public class ScheduleMap : IAutoMappingOverride<Schedule>
	{
		public void Override(AutoMapping<Schedule> mapping)
		{
			mapping.IgnoreProperty(m => m.Value);

			mapping.Map(m => m.Description)
				.Length(MaxLen.ScheduleDescription);

			mapping.References(m => m.Category)
				.Nullable();

			mapping.Map(s => s.Active)
				.Default("1");

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
