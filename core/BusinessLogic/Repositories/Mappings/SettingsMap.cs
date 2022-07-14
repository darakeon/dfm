using System;
using DFM.Entities;
using DFM.Entities.Bases;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;

namespace DFM.BusinessLogic.Repositories.Mappings
{
	// ReSharper disable once UnusedMember.Global
	public class SettingsMap : IAutoMappingOverride<Settings>
	{
		public void Override(AutoMapping<Settings> mapping)
		{
			mapping.Map(c => c.Language)
				.Length(MaxLen.SettingsLanguage)
				.Default("'" + Defaults.SettingsLanguage + "'");

			mapping.Map(c => c.SendMoveEmail)
				.Default("'" + Defaults.SettingsSendMoveEmail + "'");

			mapping.Map(c => c.UseCategories)
				.Default("'" + Defaults.SettingsUseCategories + "'");

			mapping.Map(c => c.Theme)
				.Default("'" + (Int32) Defaults.DefaultTheme + "'");
		}
	}
}
