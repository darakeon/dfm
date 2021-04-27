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
		internal static Boolean IsTest = false;

		public void Override(AutoMapping<User> mapping)
		{
			mapping.Map(u => u.Password)
				.Length(MaxLen.UserPassword);

			mapping.Map(u => u.Email)
				.Length(MaxLen.UserEmail)
				.Unique();

			mapping.Map(u => u.TFASecret)
				.Nullable();

			mapping.Map(u => u.Creation)
				.Default("'2011-09-21'");

			mapping.References(u => u.Config)
				.Cascade.All();

			mapping.Map(u => u.IsAdm)
				.Default("0")
				.Not.Update();

			var robot = mapping.Map(u => u.IsRobot)
				.Default("0");

			if (!IsTest)
				robot.Not.Update();
		}
	}
}
