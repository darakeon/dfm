using System;
using System.Linq;
using DFM.Entities.Enums;
using DFM.Generic;
using NUnit.Framework;

namespace DFM.Entities.Tests
{
	internal class TipsTests
	{
		#region IsPermanent
		[Test]
		public void IsPermanent_NoneNotDisabled()
		{
			var tips = new Tips
			{
				Type = TipType.None,
			};

			var disabled = tips.IsPermanent(1);

			Assert.That(disabled, Is.True);
		}

		[Test]
		public void IsPermanent_NoneDisabled()
		{
			var tips = new Tips
			{
				Type = TipType.None,
			};

			tips.Permanent += 1;

			var disabled = tips.IsPermanent(1);

			Assert.That(disabled, Is.True);
		}

		[Test]
		public void IsPermanent_BrowserNotDisabled()
		{
			var tips = new Tips
			{
				Type = TipType.Browser,
			};

			var disabled = tips.IsPermanent(TipBrowser.DeleteLogins);

			Assert.That(disabled, Is.False);
		}

		[Test]
		public void IsPermanent_BrowserDisabled()
		{
			var tips = new Tips
			{
				Type = TipType.Browser,
			};

			tips.Permanent += (UInt64)TipBrowser.DeleteLogins;

			var disabled = tips.IsPermanent(TipBrowser.DeleteLogins);

			Assert.That(disabled, Is.True);
		}

		[Test]
		public void IsPermanent_MobileNotDisabled()
		{
			var tips = new Tips
			{
				Type = TipType.Mobile,
			};

			var disabled = tips.IsPermanent(1);

			// Once the enum has more elements than None
			// adjust this test to use the first element
			//Assert.That(disabled, Is.False);
			Assert.That(disabled, Is.True);
			Assert.That(EnumX.AllValues<TipMobile>().Count, Is.EqualTo(1));
		}

		[Test]
		public void IsPermanent_MobileDisabled()
		{
			var tips = new Tips
			{
				Type = TipType.Mobile,
			};

			tips.Permanent += 1;

			//var disabled = tips.IsPermanent(TipMobile.---);
			var disabled = tips.IsPermanent(TipMobile.None);

			Assert.That(disabled, Is.True);
			// Once the enum has more elements than None
			// adjust this test to use the first element
			Assert.That(EnumX.AllValues<TipMobile>().Count, Is.EqualTo(1));
		}

		[Test]
		public void IsPermanent_LocalNotDisabled()
		{
			var tips = new Tips
			{
				Type = TipType.Local,
			};

			var disabled = tips.IsPermanent(1);

			// Once the enum has more elements than None
			// adjust this test to use the first element
			//Assert.That(disabled, Is.False);
			Assert.That(disabled, Is.True);
			Assert.That(EnumX.AllValues<TipLocal>().Count, Is.EqualTo(1));
		}

		[Test]
		public void IsPermanent_LocalDisabled()
		{
			var tips = new Tips
			{
				Type = TipType.Local,
			};

			tips.Permanent += 1;

			//var disabled = tips.IsPermanent(TipLocal.---);
			var disabled = tips.IsPermanent(TipLocal.None);

			Assert.That(disabled, Is.True);
			// Once the enum has more elements than None
			// adjust this test to use the first element
			Assert.That(EnumX.AllValues<TipLocal>().Count, Is.EqualTo(1));
		}

		[Test]
		public void IsPermanent_TestsNotDisabled()
		{
			var tips = new Tips
			{
				Type = TipType.Tests,
			};

			var disabled = tips.IsPermanent(TipTests.TestTip1);

			Assert.That(disabled, Is.False);
		}

		[Test]
		public void IsPermanent_TestsDisabled()
		{
			var tips = new Tips
			{
				Type = TipType.Tests,
			};

			tips.Permanent += (UInt64)TipTests.TestTip1;

			var disabled = tips.IsPermanent(TipTests.TestTip1);

			Assert.That(disabled, Is.True);
		}
		#endregion

		#region LastGiven
		[Test]
		public void LastGiven_NoneLastZero()
		{
			var tips = new Tips
			{
				Type = TipType.None,
			};

			var lastTip = tips.LastGiven();

			Assert.That(lastTip, Is.Null);
		}

		[Test]
		public void LastGiven_BrowserLastZero()
		{
			var tips = new Tips
			{
				Type = TipType.Browser,
			};

			var lastTip = tips.LastGiven();

			Assert.That(lastTip, Is.Null);
		}

		[Test]
		public void LastGiven_BrowserLastOne()
		{
			var tips = new Tips
			{
				Type = TipType.Browser,
				Last = (UInt64)TipBrowser.DeleteLogins,
			};

			var lastTip = tips.LastGiven();

			Assert.That(lastTip, Is.EqualTo(TipBrowser.DeleteLogins.ToString()));
		}

		[Test]
		public void LastGiven_MobileLastZero()
		{
			var tips = new Tips
			{
				Type = TipType.Mobile,
			};

			var lastTip = tips.LastGiven();

			Assert.That(lastTip, Is.Null);
		}

		[Test]
		public void LastGiven_MobileLastOne()
		{
			var tips = new Tips
			{
				Type = TipType.Mobile,
				Last = 1,
			};

			var lastTip = tips.LastGiven();

			// Once the enum has more elements than None
			// adjust this test to use the first element
			// Assert.That(disabled, Is.EqualTo(TipMobile.---.ToString()));
			Assert.That(lastTip, Is.Null);
			Assert.That(EnumX.AllValues<TipMobile>().Count, Is.EqualTo(1));
		}

		[Test]
		public void LastGiven_LocalLastZero()
		{
			var tips = new Tips
			{
				Type = TipType.Local,
			};

			var lastTip = tips.LastGiven();

			Assert.That(lastTip, Is.Null);
		}

		[Test]
		public void LastGiven_LocalLastOne()
		{
			var tips = new Tips
			{
				Type = TipType.Local,
				Last = 1,
			};

			var lastTip = tips.LastGiven();

			// Once the enum has more elements than None
			// adjust this test to use the first element
			// Assert.That(disabled, Is.EqualTo(TipLocal.---.ToString()));
			Assert.That(lastTip, Is.Null);
			Assert.That(EnumX.AllValues<TipLocal>().Count, Is.EqualTo(1));
		}

		[Test]
		public void LastGiven_TestsLastZero()
		{
			var tips = new Tips
			{
				Type = TipType.Tests,
			};

			var lastTip = tips.LastGiven();

			Assert.That(lastTip, Is.Null);
		}

		[Test]
		public void LastGiven_TestsLastOne()
		{
			var tips = new Tips
			{
				Type = TipType.Tests,
				Last = (UInt64)TipTests.TestTip1,
			};

			var lastTip = tips.LastGiven();

			Assert.That(lastTip, Is.EqualTo(TipTests.TestTip1.ToString()));
		}
		#endregion
	}
}
