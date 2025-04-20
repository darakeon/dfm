using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace DFM.Generic.Tests;

public class DictionaryExtensionTests
{
	[Test]
	public void TryGetValue_Existent()
	{
		var dictionary = new Dictionary<String, String>
		{
			{ "Hey", "listen" }
		};

		var value = dictionary.TryGetValue("Hey", "navi");

		Assert.That(value, Is.EqualTo("listen"));
	}

	[Test]
	public void TryGetValue_NotExistent()
	{
		var dictionary = new Dictionary<String, String>
		{
			{ "Hey", "listen" }
		};

		var value = dictionary.TryGetValue("hey", "navi");

		Assert.That(value, Is.EqualTo("navi"));
	}

	[Test]
	public void TryGetValue_NoDefault()
	{
		var dictionary = new Dictionary<String, String>
		{
			{ "Hey", "listen" }
		};

		var value = dictionary.TryGetValue("hey");

		Assert.That(value, Is.Null);
	}

	[Test]
	public void TryGetValueOrAdd_Existent()
	{
		var dictionary = new Dictionary<String, String>
		{
			{ "Hey", "listen" }
		};

		var value = dictionary.TryGetValueOrAdd("Hey", "navi");

		Assert.That(value, Is.EqualTo("listen"));
	}

	[Test]
	public void TryGetValueOrAdd_NotExistent()
	{
		var dictionary = new Dictionary<String, String>
		{
			{ "Hey", "listen" }
		};

		var value = dictionary.TryGetValueOrAdd("hey", "navi");

		Assert.That(value, Is.EqualTo("navi"));
		Assert.That(dictionary, Contains.Key("hey"));
		Assert.That(dictionary["hey"], Is.EqualTo("navi"));
	}

	[Test]
	public void TryGetValueOrAdd_NoDefault()
	{
		var dictionary = new Dictionary<String, String>
		{
			{ "Hey", "listen" }
		};

		var value = dictionary.TryGetValueOrAdd("hey");

		Assert.That(value, Is.Null);
		Assert.That(dictionary, Contains.Key("hey"));
		Assert.That(dictionary["hey"], Is.Null);
	}
}
