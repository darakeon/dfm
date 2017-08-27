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
                .Length(MaximumLength.ConfigLanguage)
                .Default("'" + Defaults.ConfigLanguage + "'");

            mapping.Map(u => u.TimeZone)
                .Default("'" + Defaults.ConfigTimeZone + "'");

            mapping.Map(u => u.SendMoveEmail)
                .Default("'" + Defaults.ConfigSendMoveEmail + "'");

            mapping.Map(u => u.UseCategories)
                .Default("'" + Defaults.ConfigUseCategories + "'");

        }
    }
}
