using DFM.BusinessLogic.Helpers;
using DFM.Entities;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;

namespace DFM.BusinessLogic.Repositories.Mappings
{
	public class CategoryMap : IAutoMappingOverride<Category>
	{
		public void Override(AutoMapping<Category> mapping)
		{
			mapping.Map(c => c.Name)
				.Length(MaxLen.Category_Name);

			mapping.Map(c => c.Active)
				.Default("1");
		}
	}
}
