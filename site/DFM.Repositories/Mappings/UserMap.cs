using DFM.Entities;
using DFM.BusinessLogic.Helpers;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;

namespace DFM.Repositories.Mappings
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
                .Default("'" + Defaults.UserLanguage + "'");

            mapping.Map(u => u.Creation)
                .Default("'2011-09-21'");

            mapping.Map(u => u.TimeZone)
                .Default("'" + Defaults.UserLanguage + "'");

            mapping.HasMany(u => u.AccountList)
                .Cascade.None()
                .Not.LazyLoad();

            mapping.HasMany(u => u.ScheduleList)
                .Cascade.SaveUpdate();
        }
    }
}
