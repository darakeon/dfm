using NUnit.Framework;

namespace DFM.Generic.Tests
{
	public class StringExtensionTests
	{
		[Test]
		public void ToBase64()
		{
			var original = "$2b$12$ibiSaL7EiAFxPcPwDmx6vOZrnpEvUjEh95vzKCV.jj5445YlVCmJK";
			var expected = "JDJiJDEyJGliaVNhTDdFaUFGeFBjUHdEbXg2dk9acm5wRXZVakVoOTV2ektDVi5qajU0NDVZbFZDbUpL";

			Assert.AreEqual(expected, original.ToBase64());
		}
	}
}
