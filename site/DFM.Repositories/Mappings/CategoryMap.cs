using DFM.Entities;
using DFM.BusinessLogic.Helpers;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;

namespace DFM.Repositories.Mappings
{
	public class CategoryMap : IAutoMappingOverride<Category>
	{
		public void Override(AutoMapping<Category> mapping)
		{
			mapping.Map(c => c.Name)
				.Length(MaximumLength.CATEGORY_NAME);
			
			mapping.Map(c => c.Active)
				.Default("1");
		}
	}
}
