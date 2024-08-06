using DFM.Entities;
using DFM.Entities.Bases;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;

namespace DFM.BusinessLogic.Repositories.Mappings;

// ReSharper disable once UnusedMember.Global
public class LineMap : IAutoMappingOverride<Line>
{
	public void Override(AutoMapping<Line> mapping)
	{
		mapping.Map(l => l.Position)
			.UniqueKey("UK_Line");

		mapping.References(l => l.Archive)
			.UniqueKey("UK_Line");

		mapping.Map(l => l.Description)
			.Length(MaxLen.LineDescription);

		mapping.IgnoreProperty(l => l.Value);
		mapping.IgnoreProperty(l => l.Conversion);

		mapping.Map(l => l.Category)
			.Length(MaxLen.CategoryName);

		mapping.HasMany(l => l.DetailList)
			.Cascade.AllDeleteOrphan()
			.Not.LazyLoad();

		mapping.Map(l => l.In)
			.Length(MaxLen.AccountName)
			.Column("In_");

		mapping.Map(l => l.Out)
			.Length(MaxLen.AccountName)
			.Column("Out_");

		mapping.IgnoreProperty(l => l.HasIn);
		mapping.IgnoreProperty(l => l.HasOut);
		mapping.IgnoreProperty(l => l.HasCategory);
	}
}
