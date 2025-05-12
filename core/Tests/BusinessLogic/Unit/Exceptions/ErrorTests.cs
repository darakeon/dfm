using System;
using System.Linq;
using DFM.BusinessLogic.Exceptions;
using DFM.Generic;
using NUnit.Framework;

namespace DFM.BusinessLogic.Tests.Unit.Exceptions;

internal class ErrorTests
{
	[Test]
	public void NonRepeatedValues()
	{
		var repeatedErrors = EnumX.AllValues<Error>()
			.GroupBy(value => (Int32)value)
			.Where(group => group.Count() > 1)
			.ToList();

		Assert.That(repeatedErrors, Is.Empty);
	}
}
