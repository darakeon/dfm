using DFM.Core.Entities;
using DFM.Core.Helpers;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;

namespace DFM.Core.Mappings
{
    public class UserMap : IAutoMappingOverride<User>
    {
        public void Override(AutoMapping<User> mapping)
        {
            mapping.Map(u => u.Password)
                .Length(MaximumLength.UserPassword);

            mapping.Map(u => u.Email)
                .Length(MaximumLength.UserEmail)
                .Unique();

            mapping.Map(u => u.Language)
                .Length(MaximumLength.UserLanguage)
                .Default("'pt-BR'");

            mapping.HasMany(u => u.AccountList)
                .Cascade.SaveUpdate()
                .Not.LazyLoad();

            mapping.HasMany(u => u.ScheduleList)
                .Cascade.SaveUpdate();
        }
    }
}
