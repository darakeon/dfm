using DFM.Core.Entities;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;

namespace DFM.Core.Mappings
{
    public class CategoryMap : IAutoMappingOverride<Category>
    {
        public void Override(AutoMapping<Category> mapping)
        {
            mapping.Map(c => c.Active)
                .Default("1");

            mapping.References(c => c.User)
                .Cascade.None();
        }
    }
}
