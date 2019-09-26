using DFM.Entities;
using DFM.Entities.Bases;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;

namespace DFM.BusinessLogic.Repositories.Mappings
{
	// ReSharper disable once UnusedMember.Global
	public class ConfigMap : IAutoMappingOverride<Config>
	{
		public void Override(AutoMapping<Config> mapping)
		{
			mapping.Map(c => c.Language)
				.Length(MaxLen.ConfigLanguage)
				.Default("'" + Defaults.ConfigLanguage + "'");

			mapping.Map(c => c.TimeZone)
				.Default("'" + Defaults.ConfigTimezone + "'");

			mapping.Map(c => c.SendMoveEmail)
				.Default("'" + Defaults.ConfigSendMoveEmail + "'");

			mapping.Map(c => c.UseCategories)
				.Default("'" + Defaults.ConfigUseCategories + "'");

			mapping.Map(c => c.Theme)
				.Default("'" + (int) Defaults.DefaultTheme + "'");
		}
	}
}
