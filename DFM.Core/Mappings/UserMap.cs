using DFM.Core.Entities;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;

namespace DFM.Core.Mappings
{
    public class UserMap : IAutoMappingOverride<User>
    {
        public void Override(AutoMapping<User> mapping)
        {
            mapping.Map(u => u.Login)
                .Unique();

            mapping.Map(u => u.Language)
                .Default("1");

            mapping.HasMany(u => u.AccountList)
                .Cascade.SaveUpdate()
                .Not.LazyLoad();
        }
    }
}
