using DFM.BusinessLogic.Helpers;
using DFM.Entities;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;

namespace DFM.Repositories.Mappings
{
    class ConfigMap : IAutoMappingOverride<Config>
    {
        public void Override(AutoMapping<Config> mapping)
        {
            mapping.Map(u => u.Language)
                .Length(MaximumLength.CONFIG_LANGUAGE)
                .Default("'" + Defaults.CONFIG_LANGUAGE + "'");

            mapping.Map(u => u.TimeZone)
                .Default("'" + Defaults.CONFIG_TIMEZONE + "'");

            mapping.Map(u => u.SendMoveEmail)
                .Default("'" + Defaults.CONFIG_SEND_MOVE_EMAIL + "'");

            mapping.Map(u => u.UseCategories)
                .Default("'" + Defaults.CONFIG_USE_CATEGORIES + "'");

        }
    }
}
