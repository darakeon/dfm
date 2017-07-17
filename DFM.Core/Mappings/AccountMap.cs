using DFM.Core.Entities;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;

namespace DFM.Core.Mappings
{
    public class AccountMap : IAutoMappingOverride<Account>
    {
        public void Override(AutoMapping<Account> mapping)
        {
            mapping.Map(a => a.EndDate)
                .Nullable();

            mapping.HasMany(a => a.MoveList)
                .Cascade.SaveUpdate();

            mapping.References(a => a.User)
                .Cascade.None();
        }
    }
}
