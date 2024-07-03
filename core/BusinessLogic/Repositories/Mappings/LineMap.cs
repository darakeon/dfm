using DFM.Entities;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;

namespace DFM.BusinessLogic.Repositories.Mappings;

// ReSharper disable once UnusedMember.Global
public class LineMap : IAutoMappingOverride<Line>
{
	public void Override(AutoMapping<Line> mapping)
	{
		mapping.Map(l => l.Position)
			.UniqueKey("Line_NumberArchive");

		mapping.References(l => l.Archive)
			.UniqueKey("Line_NumberArchive");

		mapping.HasMany(l => l.DetailList)
			.Cascade.AllDeleteOrphan()
			.Not.LazyLoad();

		mapping.Map(l => l.In)
			.Column("In_");

		mapping.Map(l => l.Out)
			.Column("Out_");

		mapping.IgnoreProperty(l => l.HasIn);
		mapping.IgnoreProperty(l => l.HasOut);
		mapping.IgnoreProperty(l => l.HasCategory);
	}
}
