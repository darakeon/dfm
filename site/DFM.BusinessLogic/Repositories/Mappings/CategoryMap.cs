using DFM.Entities;
using DFM.Entities.Bases;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;

namespace DFM.BusinessLogic.Repositories.Mappings
{
	// ReSharper disable once UnusedMember.Global
	public class CategoryMap : IAutoMappingOverride<Category>
	{
		public void Override(AutoMapping<Category> mapping)
		{
			mapping.Map(c => c.Name)
				.Length(MaxLen.CategoryName)
				.UniqueKey("Category_NameUser");

			mapping.Map(c => c.Active)
				.Default("1");

			mapping.References(a => a.User)
				.UniqueKey("Category_NameUser");
		}
	}
}
