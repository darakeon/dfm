using DFM.Entities;
using DFM.Entities.Bases;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;
using System;

namespace DFM.BusinessLogic.Repositories.Mappings
{
	// ReSharper disable once UnusedMember.Global
	public class WipeMap : IAutoMappingOverride<Wipe>
	{
		public void Override(AutoMapping<Wipe> mapping)
		{
			mapping.Table($"`{nameof(Wipe).ToLower()}`");

			mapping.Map(p => p.When)
				.Column("When_");

			mapping.Map(u => u.UsernameStart)
				.Length(MaxLen.WipeUsernameStart)
				.CustomType<String>();

			mapping.Map(u => u.DomainStart)
				.Length(MaxLen.WipeDomainStart)
				.CustomType<String>();

			mapping.Map(p => p.S3)
				.Nullable();
		}
	}
}
