using DFM.Entities;
using DFM.Entities.Bases;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;

namespace DFM.BusinessLogic.Repositories.Mappings
{
	// ReSharper disable once UnusedMember.Global
	public class UserMap : IAutoMappingOverride<User>
	{
		public void Override(AutoMapping<User> mapping)
		{
			mapping.Map(u => u.Password)
				.Length(MaxLen.UserPassword);

			mapping.Map(u => u.Email)
				.Length(MaxLen.UserEmail)
				.Unique();

			mapping.Map(u => u.TFASecret)
				.Nullable();

			mapping.References(u => u.Config)
				.Cascade.All();

			mapping.References(u => u.Control)
				.Cascade.All();
		}
	}
}
