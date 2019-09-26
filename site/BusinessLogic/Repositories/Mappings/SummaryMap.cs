using DFM.Entities;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;

namespace DFM.BusinessLogic.Repositories.Mappings
{
	// ReSharper disable once UnusedMember.Global
	public class SummaryMap : IAutoMappingOverride<Summary>
	{
		public void Override(AutoMapping<Summary> mapping)
		{
			mapping.IgnoreProperty(s => s.In);
			mapping.IgnoreProperty(s => s.Out);

			mapping.References(s => s.Account)
				.UniqueKey("UK_Summary")
				.Not.Update();

			mapping.References(s => s.Category)
				.UniqueKey("UK_Summary")
				.Not.Update()
				.Nullable();

			mapping.Map(s => s.Time)
				.UniqueKey("UK_Summary");

			mapping.Map(s => s.Nature)
				.UniqueKey("UK_Summary");
		}

	}
}
