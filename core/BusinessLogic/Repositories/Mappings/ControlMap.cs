using System;
using DFM.Entities;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;

namespace DFM.BusinessLogic.Repositories.Mappings
{
	// ReSharper disable once UnusedMember.Global
	public class ControlMap : IAutoMappingOverride<Control>
	{
		internal static Boolean IsTest = false;

		public void Override(AutoMapping<Control> mapping)
		{
			mapping.Map(u => u.Creation)
				.Default("'2011-09-21'");

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
