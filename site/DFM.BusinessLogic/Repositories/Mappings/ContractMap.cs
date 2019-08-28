using DFM.Entities;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;

namespace DFM.BusinessLogic.Repositories.Mappings
{
	public class ContractMap : IAutoMappingOverride<Contract>
	{
		public void Override(AutoMapping<Contract> mapping)
		{
			mapping.Map(m => m.BeginDate).Not.Update();
			mapping.Map(m => m.Version).Not.Update();
		}
	}
}
