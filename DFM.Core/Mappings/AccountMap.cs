using DFM.Core.Entities;
using DFM.Core.Helpers;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;

namespace DFM.Core.Mappings
{
    public class AccountMap : IAutoMappingOverride<Account>
    {
        public void Override(AutoMapping<Account> mapping)
        {
            mapping.Map(a => a.Name)
                .Length(MaximumLength.AccountName);

            mapping.Map(a => a.EndDate)
                .Nullable();

            mapping.Map(a => a.RedLimit)
                .Nullable();

            mapping.Map(a => a.YellowLimit)
                .Nullable();

            mapping.HasMany(a => a.YearList)
                .Cascade.Delete();

            mapping.References(a => a.User)
                .Cascade.None();
        }
    }
}
