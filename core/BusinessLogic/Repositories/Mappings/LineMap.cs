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
			.Cascade.SaveUpdate();

		mapping.Map(l => l.In)
			.Column("In_");

		mapping.Map(l => l.Out)
			.Column("Out_");
	}
}
