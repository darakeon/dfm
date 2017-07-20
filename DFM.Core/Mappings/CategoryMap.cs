using DFM.Entities;
using DFM.Core.Helpers;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;

namespace DFM.Core.Mappings
{
    public class CategoryMap : IAutoMappingOverride<Category>
    {
        public void Override(AutoMapping<Category> mapping)
        {
            mapping.Not.LazyLoad();

            mapping.Map(c => c.Name)
                .Length(MaximumLength.CategoryName);
            
            mapping.Map(c => c.Active)
                .Default("1");

            mapping.References(c => c.User)
                .Cascade.None();
        }
    }
}
