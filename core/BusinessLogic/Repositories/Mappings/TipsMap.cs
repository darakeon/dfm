using DFM.Entities;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;

namespace DFM.BusinessLogic.Repositories.Mappings
{
	// ReSharper disable once UnusedMember.Global
	public class TipsMap : IAutoMappingOverride<Tips>
	{
		public void Override(AutoMapping<Tips> mapping)
		{
			mapping.References(t => t.User)
				.UniqueKey("Tips_UserType");

			mapping.Map(t => t.Type)
				.UniqueKey("Tips_UserType");

			mapping.Map(t => t.Repeat)
				.Column("Repeat_");
		}
	}
}
