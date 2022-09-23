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
			mapping.Map(c => c.Creation)
				.Default("'2011-09-21'");

			mapping.Map(c => c.IsAdm)
				.Default("0")
				.Not.Update();

			var robot = mapping.Map(c => c.IsRobot)
				.Default("0");

			if (!IsTest)
				robot.Not.Update();

			mapping.Map(c => c.ProcessingDeletion)
				.Default("0");

			mapping.Map(c => c.MiscDna)
				.Default("347");
		}
	}
}
