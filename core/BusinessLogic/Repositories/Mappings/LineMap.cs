using DFM.Entities;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;

namespace DFM.BusinessLogic.Repositories.Mappings;

// ReSharper disable once UnusedMember.Global
public class LineMap : IAutoMappingOverride<Line>
{
	public void Override(AutoMapping<Line> mapping)
	{
	}
}
