using DFM.Entities;
using DFM.BusinessLogic.Helpers;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;

namespace DFM.Repositories.Mappings
{
    public class AccountMap : IAutoMappingOverride<Account>
    {
        public void Override(AutoMapping<Account> mapping)
        {
            mapping.Not.LazyLoad();

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
