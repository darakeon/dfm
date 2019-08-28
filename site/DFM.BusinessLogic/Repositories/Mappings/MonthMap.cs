using DFM.Entities;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;

namespace DFM.BusinessLogic.Repositories.Mappings
{
	public class MonthMap : IAutoMappingOverride<Month>
	{
		public void Override(AutoMapping<Month> mapping)
		{
			mapping.Map(m => m.Time)
				.UniqueKey("Month_TimeYear");

			mapping.References(m => m.Year)
				.UniqueKey("Month_TimeYear")
				.Cascade.SaveUpdate()
				.Not.Update()
				.Nullable();

			mapping.HasMany(m => m.SummaryList)
				.Cascade.SaveUpdate();
		}
	}
}
