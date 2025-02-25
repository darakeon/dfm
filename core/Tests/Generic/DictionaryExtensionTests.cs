using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace DFM.Generic.Tests
{
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
	}
}
