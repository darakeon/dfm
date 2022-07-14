using System;
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

			mapping.IgnoreProperty(u => u.Email);

			mapping.Map(u => u.Username)
				.Length(MaxLen.UserEmailUsername)
				.CustomType<String>()
				.UniqueKey("UK_Email");

			mapping.Map(u => u.Domain)
				.Length(MaxLen.UserEmailDomain)
				.CustomType<String>()
				.UniqueKey("UK_Email");

			mapping.Map(u => u.TFASecret)
				.Nullable();

			mapping.References(u => u.Settings)
				.Cascade.All();

			mapping.References(u => u.Control)
				.Cascade.All();
		}
	}
}
