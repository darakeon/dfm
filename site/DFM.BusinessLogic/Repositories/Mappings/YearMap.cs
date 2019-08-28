using DFM.Entities;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;

namespace DFM.BusinessLogic.Repositories.Mappings
{
	public class YearMap : IAutoMappingOverride<Year>
	{
		public void Override(AutoMapping<Year> mapping)
		{
			mapping.HasMany(y => y.MonthList)
				.Cascade.Delete()
				.Inverse();

			mapping.Map(m => m.Time)
				.UniqueKey("Year_TimeAccount");

			mapping.References(y => y.Account)
				.UniqueKey("Year_TimeAccount")
				.Not.Update()
				.Nullable();

			mapping.HasMany(y => y.SummaryList)
				.Cascade.SaveUpdate();
		}
	}
}
