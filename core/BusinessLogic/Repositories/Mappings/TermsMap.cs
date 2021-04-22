using DFM.Entities;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;

namespace DFM.BusinessLogic.Repositories.Mappings
{
	// ReSharper disable once UnusedMember.Global
	public class TermsMap : IAutoMappingOverride<Terms>
	{
		public void Override(AutoMapping<Terms> mapping)
		{
			mapping.Map(m => m.Json).Not.Update();
			mapping.Map(m => m.Language).Not.Update();
		}
	}
}
