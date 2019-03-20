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
			mapping.Map(c => c.Language)
				.Length(MaximumLength.Config_Language)
				.Default("'" + Defaults.CONFIG_LANGUAGE + "'");

			mapping.Map(c => c.TimeZone)
				.Default("'" + Defaults.CONFIG_TIMEZONE + "'");

			mapping.Map(c => c.SendMoveEmail)
				.Default("'" + Defaults.CONFIG_SEND_MOVE_EMAIL + "'");

			mapping.Map(c => c.UseCategories)
				.Default("'" + Defaults.CONFIG_USE_CATEGORIES + "'");

			mapping.Map(c => c.Theme)
				.Default("'" + (int) Defaults.DEFAULT_THEME + "'");
		}
	}
}
