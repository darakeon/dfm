using DFM.Entities;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;

namespace DFM.BusinessLogic.Repositories.Mappings
{
	// ReSharper disable once UnusedMember.Global
	public class AcceptanceMap : IAutoMappingOverride<Acceptance>
	{
		public void Override(AutoMapping<Acceptance> mapping)
		{
			mapping.Map(m => m.CreateDate).Not.Update();
			mapping.References(m => m.User).Not.Update();
			mapping.References(m => m.Contract).Not.Update().Not.LazyLoad();
		}
	}
}
