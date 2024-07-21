using DFM.Entities;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;

namespace DFM.BusinessLogic.Repositories.Mappings;

// ReSharper disable once UnusedMember.Global
public class ArchiveMap : IAutoMappingOverride<Archive>
{
	public void Override(AutoMapping<Archive> mapping)
	{
		mapping.IgnoreProperty(m => m.Guid);

		mapping.References(m => m.User)
			.Not.Nullable();

		mapping.HasMany(a => a.LineList)
			.Cascade.SaveUpdate();
	}
}
