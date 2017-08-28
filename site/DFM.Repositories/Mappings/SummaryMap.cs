using DFM.Entities;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;

namespace DFM.Repositories.Mappings
{
	public class SummaryMap : IAutoMappingOverride<Summary>
	{
		public void Override(AutoMapping<Summary> mapping)
		{
			mapping.IgnoreProperty(s => s.In);
			mapping.IgnoreProperty(s => s.Out);

			mapping.References(s => s.Month)
				.UniqueKey("Summary_CategoryTime")
				.Not.Update()
				.Nullable();

			mapping.References(s => s.Year)
				.UniqueKey("Summary_CategoryTime")
				.Not.Update()
				.Nullable();

			mapping.References(s => s.Category)
				.UniqueKey("Summary_CategoryTime")
				.Nullable();
		}

	}
}
