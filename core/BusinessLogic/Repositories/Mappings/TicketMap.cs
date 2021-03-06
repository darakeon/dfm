﻿using DFM.Entities;
using DFM.Entities.Bases;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;

namespace DFM.BusinessLogic.Repositories.Mappings
{
	// ReSharper disable once UnusedMember.Global
	public class TicketMap : IAutoMappingOverride<Ticket>
	{
		public void Override(AutoMapping<Ticket> mapping)
		{
			mapping.Map(t => t.Key, "Key_")
				.Length(MaxLen.TicketKey)
				.Unique();

			mapping.Map(t => t.Active)
				.Default("1");

			mapping.Map(t => t.Expiration)
				.Nullable();
		}
	}
}
