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

		#region Random
		[Test]
		public void Random_NoneCleanTip()
		{
			var tips = new Tips
			{
				Type = TipType.None,
			};

			var randomTip = tips.Random();

			Assert.That(randomTip, Is.Null);
		}

		[Test]
		public void Random_NoneHasPermanent()
		{
			var tips = new Tips
			{
				Type = TipType.None,
				Permanent = 1,
			};

			var randomTip = tips.Random();

			Assert.That(randomTip, Is.Null);
		}

		[Test]
		public void Random_NoneHasTemporary()
		{
			var tips = new Tips
			{
				Type = TipType.None,
				Temporary = 1,
			};

			var randomTip = tips.Random();

			Assert.That(randomTip, Is.Null);
		}

		[Test]
		public void Random_BrowserCleanTip()
		{
			var tips = new Tips
			{
				Type = TipType.Browser,
			};

			var randomTip = tips.Random();

			var tipsAccepted = EnumX.AllValues<TipBrowser>()
				.Where(t => t != TipBrowser.None)
				.Select(t => t.ToString())
				.ToList();
			Assert.That(randomTip, Is.AnyOf(tipsAccepted));
		}

		[Test]
		public void Random_BrowserHasPermanent()
		{
			var tips = new Tips
			{
				Type = TipType.Browser,
				Permanent = (UInt64)TipBrowser.DeleteLogins,
			};

			var randomTip = tips.Random();

			Assert.That(randomTip, Is.Not.EqualTo(TipBrowser.DeleteLogins.ToString()));

			var tipsAccepted = EnumX.AllValues<TipBrowser>()
				.Where(t => t != TipBrowser.DeleteLogins && t != TipBrowser.None)
				.Select(t => t.ToString())
				.ToList();

			Assert.That(randomTip, Is.AnyOf(tipsAccepted));
		}

		[Test]
		public void Random_BrowserHasTemporary()
		{
			var tips = new Tips
			{
				Type = TipType.Browser,
				Temporary = (UInt64)TipBrowser.DeleteLogins,
			};

			var randomTip = tips.Random();

			Assert.That(randomTip, Is.Not.EqualTo(TipBrowser.DeleteLogins.ToString()));

			var tipsAccepted = EnumX.AllValues<TipBrowser>()
				.Where(t => t != TipBrowser.DeleteLogins && t != TipBrowser.None)
				.Select(t => t.ToString())
				.ToList();

			Assert.That(randomTip, Is.AnyOf(tipsAccepted));
		}

		[Test]
		public void Random_MobileCleanTip()
		{
			var tips = new Tips
			{
				Type = TipType.Mobile,
			};

			var randomTip = tips.Random();

			// Once the enum has valid elements
			// adjust this test to use the first element
			//var tipsAccepted = EnumX.AllValues<TipMobile>()
			//	.Where(t => t != TipMobile.None)
			//	.Select(t => t.ToString())
			//	.ToList();
			//Assert.That(randomTip, Is.AnyOf(tipsAccepted));
			Assert.That(randomTip, Is.Null);
			Assert.That(EnumX.AllValues<TipMobile>().Count, Is.EqualTo(1));
		}

		[Test]
		public void Random_MobileHasPermanent()
		{
			var tips = new Tips
			{
				Type = TipType.Mobile,
				Permanent = 1,
			};

			var randomTip = tips.Random();

			// Once the enum has more elements than 2
			// adjust this test to use the first element
			//Assert.That(randomTip, Is.Not.EqualTo(TipMobile.---.ToString()));
			//var tipsAccepted = EnumX.AllValues<TipMobile>()
			//	.Where(t => t != TipMobile.--- && t != TipMobile.None)
			//	.Select(t => t.ToString());
			//Assert.That(randomTip, Is.AnyOf(tipsAccepted));
			Assert.That(randomTip, Is.Null);
			Assert.That(EnumX.AllValues<TipMobile>().Count, Is.EqualTo(1));
		}

		[Test]
		public void Random_MobileHasTemporary()
		{
			var tips = new Tips
			{
				Type = TipType.Mobile,
				Temporary = 1,
			};

			var randomTip = tips.Random();

			// Once the enum has more elements than 2
			// adjust this test to use the first element
			//Assert.That(randomTip, Is.Not.EqualTo(TipMobile.---.ToString()));
			//var tipsAccepted = EnumX.AllValues<TipMobile>()
			//	.Where(t => t != TipMobile.--- && t != TipMobile.None)
			//	.Select(t => t.ToString());
			//Assert.That(randomTip, Is.AnyOf(tipsAccepted));
			Assert.That(randomTip, Is.Null);
			Assert.That(EnumX.AllValues<TipMobile>().Count, Is.EqualTo(1));
		}

		[Test]
		public void Random_LocalCleanTip()
		{
			var tips = new Tips
			{
				Type = TipType.Local,
			};

			var randomTip = tips.Random();

			// Once the enum has valid elements
			// adjust this test to use the first element
			//var tipsAccepted = EnumX.AllValues<TipLocal>()
			//	.Where(t => t != TipLocal.None)
			//	.Select(t => t.ToString())
			//	.ToList();
			//Assert.That(randomTip, Is.AnyOf(tipsAccepted));
			Assert.That(randomTip, Is.Null);
			Assert.That(EnumX.AllValues<TipLocal>().Count, Is.EqualTo(1));
		}

		[Test]
		public void Random_LocalHasPermanent()
		{
			var tips = new Tips
			{
				Type = TipType.Local,
				Permanent = 1,
			};

			var randomTip = tips.Random();

			// Once the enum has more elements than 2
			// adjust this test to use the first element
			//Assert.That(randomTip, Is.Not.EqualTo(TipLocal.---.ToString()));
			//var tipsAccepted = EnumX.AllValues<TipLocal>()
			//	.Where(t => t != TipLocal.--- && t != TipLocal.None)
			//	.Select(t => t.ToString());
			//Assert.That(randomTip, Is.AnyOf(tipsAccepted));
			Assert.That(randomTip, Is.Null);
			Assert.That(EnumX.AllValues<TipLocal>().Count, Is.EqualTo(1));
		}

		[Test]
		public void Random_LocalHasTemporary()
		{
			var tips = new Tips
			{
				Type = TipType.Local,
				Temporary = 1,
			};

			var randomTip = tips.Random();

			// Once the enum has more elements than 2
			// adjust this test to use the first element
			//Assert.That(randomTip, Is.Not.EqualTo(TipLocal.---.ToString()));
			//var tipsAccepted = EnumX.AllValues<TipLocal>()
			//	.Where(t => t != TipLocal.--- && t != TipLocal.None)
			//	.Select(t => t.ToString());
			//Assert.That(randomTip, Is.AnyOf(tipsAccepted));
			Assert.That(randomTip, Is.Null);
			Assert.That(EnumX.AllValues<TipLocal>().Count, Is.EqualTo(1));
		}

		[Test]
		public void Random_TestsCleanTip()
		{
			var tips = new Tips
			{
				Type = TipType.Tests,
			};

			var randomTip = tips.Random();

			var tipsAccepted = EnumX.AllValues<TipTests>()
				.Where(t => t != TipTests.None)
				.Select(t => t.ToString())
				.ToList();
			Assert.That(randomTip, Is.AnyOf(tipsAccepted));
		}

		[Test]
		public void Random_TestsHasPermanent()
		{
			var tips = new Tips
			{
				Type = TipType.Tests,
				Permanent = (UInt64)TipTests.TestTip1,
			};

			var randomTip = tips.Random();

			Assert.That(randomTip, Is.Not.EqualTo(TipTests.TestTip1.ToString()));

			var tipsAccepted = EnumX.AllValues<TipTests>()
				.Where(t => t != TipTests.TestTip1 && t != TipTests.None)
				.Select(t => t.ToString())
				.ToList();

			Assert.That(randomTip, Is.AnyOf(tipsAccepted));
		}

		[Test]
		public void Random_TestsHasTemporary()
		{
			var tips = new Tips
			{
				Type = TipType.Tests,
				Permanent = (UInt64)TipTests.TestTip1,
			};

			var randomTip = tips.Random();

			Assert.That(randomTip, Is.Not.EqualTo(TipTests.TestTip1.ToString()));

			var tipsAccepted = EnumX.AllValues<TipTests>()
				.Where(t => t != TipTests.TestTip1 && t != TipTests.None)
				.Select(t => t.ToString())
				.ToList();

			Assert.That(randomTip, Is.AnyOf(tipsAccepted));
		}
		#endregion

		#region IsFull
		[Test]
		public void IsFull_NoneEmpty()
		{
			var tips = new Tips
			{
				Type = TipType.None,
			};

			var full = tips.IsFull();

			Assert.That(full, Is.True);
		}

		[Test]
		public void IsFull_NoneFull()
		{
			var tips = new Tips
			{
				Type = TipType.None,
				Permanent = UInt64.MaxValue,
				Temporary = UInt64.MaxValue
			};

			var full = tips.IsFull();

			Assert.That(full, Is.True);
		}


		[Test]
		public void IsFull_BrowserEmpty()
		{
			var tips = new Tips
			{
				Type = TipType.Browser,
			};

			var full = tips.IsFull();

			Assert.That(full, Is.False);
		}

		[Test]
		public void IsFull_BrowserOnePermanent()
		{
			var tips = new Tips
			{
				Type = TipType.Browser,
			};

			tips.Permanent += (UInt64)TipBrowser.DeleteLogins;

			var full = tips.IsFull();

			Assert.That(full, Is.False);
		}

		[Test]
		public void IsFull_BrowserOneTemporary()
		{
			var tips = new Tips
			{
				Type = TipType.Browser,
			};

			tips.Temporary += (UInt64)TipBrowser.DeleteLogins;

			var full = tips.IsFull();

			Assert.That(full, Is.False);
		}

		[Test]
		public void IsFull_BrowserFullPermanent()
		{
			var tips = new Tips
			{
				Type = TipType.Browser,
			};

			foreach (var tip in EnumX.AllValues<TipBrowser>())
			{
				tips.Permanent += (UInt64)tip;
			}

			var full = tips.IsFull();

			Assert.That(full, Is.True);
		}

		[Test]
		public void IsFull_BrowserFullTemporary()
		{
			var tips = new Tips
			{
				Type = TipType.Browser,
			};

			foreach (var tip in EnumX.AllValues<TipBrowser>())
			{
				tips.Temporary += (UInt64)tip;
			}

			var full = tips.IsFull();

			Assert.That(full, Is.True);
		}

		[Test]
		public void IsFull_BrowserPermanentAndTemporaryNotFull()
		{
			var tips = new Tips
			{
				Type = TipType.Browser,
			};

			var last = EnumX.AllValues<TipBrowser>()
				.Where(t => t != TipBrowser.None)
				.Select(t => (UInt64)t)
				.Max();

			tips.Permanent += last;
			tips.Temporary += last;

			var full = tips.IsFull();

			Assert.That(full, Is.False);
		}


		[Test]
		public void IsFull_MobileEmpty()
		{
			var tips = new Tips
			{
				Type = TipType.Mobile,
			};

			var full = tips.IsFull();

			// Once the enum has more elements than 2
			// adjust this test to use the first element
			//Assert.That(full, Is.False);
			Assert.That(full, Is.True);
			Assert.That(EnumX.AllValues<TipMobile>().Count, Is.EqualTo(1));
		}

		[Test]
		public void IsFull_MobileOnePermanent()
		{
			var tips = new Tips
			{
				Type = TipType.Mobile,
			};

			//tips.Permanent += (UInt64)TipMobile.---;
			tips.Permanent += 1;

			var full = tips.IsFull();

			// Once the enum has more elements than 2
			// adjust this test to use the first element
			//Assert.That(full, Is.False);
			Assert.That(full, Is.True);
			Assert.That(EnumX.AllValues<TipMobile>().Count, Is.EqualTo(1));
		}

		[Test]
		public void IsFull_MobileOneTemporary()
		{
			var tips = new Tips
			{
				Type = TipType.Mobile,
			};

			//tips.Temporary += (UInt64)TipMobile.---;
			tips.Temporary += 1;

			var full = tips.IsFull();

			// Once the enum has more elements than 2
			// adjust this test to use the first element
			//Assert.That(full, Is.False);
			Assert.That(full, Is.True);
			Assert.That(EnumX.AllValues<TipMobile>().Count, Is.EqualTo(1));
		}

		[Test]
		public void IsFull_MobileFullPermanent()
		{
			var tips = new Tips
			{
				Type = TipType.Mobile,
			};

			foreach (var tip in EnumX.AllValues<TipMobile>())
			{
				tips.Permanent += (UInt64)tip;
			}

			var full = tips.IsFull();

			Assert.That(full, Is.True);
		}

		[Test]
		public void IsFull_MobileFullTemporary()
		{
			var tips = new Tips
			{
				Type = TipType.Mobile,
			};

			foreach (var tip in EnumX.AllValues<TipMobile>())
			{
				tips.Temporary += (UInt64)tip;
			}

			var full = tips.IsFull();

			Assert.That(full, Is.True);
		}

		[Test]
		public void IsFull_MobilePermanentAndTemporaryNotFull()
		{
			var tips = new Tips
			{
				Type = TipType.Mobile,
			};

			/*
			var last = EnumX.AllValues<TipMobile>()
				.Where(t => t != TipMobile.None)
				.Select(t => (UInt64)t)
				.Max();

			tips.Permanent += last;
			tips.Temporary += last;
			*/

			var full = tips.IsFull();

			// Once the enum has more elements than 2
			// adjust this test to use the first element
			//Assert.That(full, Is.False);
			Assert.That(full, Is.True);
			Assert.That(EnumX.AllValues<TipMobile>().Count, Is.EqualTo(1));
		}


		[Test]
		public void IsFull_LocalOnePermanent()
		{
			var tips = new Tips
			{
				Type = TipType.Local,
			};

			//tips.Permanent += (UInt64)TipLocal.---;
			tips.Permanent += 1;

			var full = tips.IsFull();

			// Once the enum has more elements than 2
			// adjust this test to use the first element
			//Assert.That(full, Is.False);
			Assert.That(full, Is.True);
			Assert.That(EnumX.AllValues<TipLocal>().Count, Is.EqualTo(1));
		}

		[Test]
		public void IsFull_LocalOneTemporary()
		{
			var tips = new Tips
			{
				Type = TipType.Local,
			};

			//tips.Temporary += (UInt64)TipLocal.---;
			tips.Temporary += 1;

			var full = tips.IsFull();

			// Once the enum has more elements than 2
			// adjust this test to use the first element
			//Assert.That(full, Is.False);
			Assert.That(full, Is.True);
			Assert.That(EnumX.AllValues<TipLocal>().Count, Is.EqualTo(1));
		}

		[Test]
		public void IsFull_LocalFullPermanent()
		{
			var tips = new Tips
			{
				Type = TipType.Local,
			};

			foreach (var tip in EnumX.AllValues<TipLocal>())
			{
				tips.Permanent += (UInt64)tip;
			}

			var full = tips.IsFull();

			Assert.That(full, Is.True);
		}

		[Test]
		public void IsFull_LocalFullTemporary()
		{
			var tips = new Tips
			{
				Type = TipType.Local,
			};

			foreach (var tip in EnumX.AllValues<TipLocal>())
			{
				tips.Temporary += (UInt64)tip;
			}

			var full = tips.IsFull();

			Assert.That(full, Is.True);
		}

		[Test]
		public void IsFull_LocalPermanentAndTemporaryNotFull()
		{
			var tips = new Tips
			{
				Type = TipType.Local,
			};

			/*
			var last = EnumX.AllValues<TipLocal>()
				.Where(t => t != TipLocal.None)
				.Select(t => (UInt64)t)
				.Max();

			tips.Permanent += last;
			tips.Temporary += last;
			*/

			var full = tips.IsFull();

			// Once the enum has more elements than 2
			// adjust this test to use the first element
			//Assert.That(full, Is.False);
			Assert.That(full, Is.True);
			Assert.That(EnumX.AllValues<TipLocal>().Count, Is.EqualTo(1));
		}


		[Test]
		public void IsFull_TestsEmpty()
		{
			var tips = new Tips
			{
				Type = TipType.Tests,
			};

			var full = tips.IsFull();

			Assert.That(full, Is.False);
		}

		[Test]
		public void IsFull_TestsOnePermanent()
		{
			var tips = new Tips
			{
				Type = TipType.Tests,
			};

			tips.Permanent += (UInt64)TipTests.TestTip1;

			var full = tips.IsFull();

			Assert.That(full, Is.False);
		}

		[Test]
		public void IsFull_TestsOneTemporary()
		{
			var tips = new Tips
			{
				Type = TipType.Tests,
			};

			tips.Temporary += (UInt64)TipTests.TestTip1;

			var full = tips.IsFull();

			Assert.That(full, Is.False);
		}

		[Test]
		public void IsFull_TestsFullPermanent()
		{
			var tips = new Tips
			{
				Type = TipType.Tests,
			};

			foreach (var tip in EnumX.AllValues<TipTests>())
			{
				tips.Permanent += (UInt64)tip;
			}

			var full = tips.IsFull();

			Assert.That(full, Is.True);
		}

		[Test]
		public void IsFull_TestsFullTemporary()
		{
			var tips = new Tips
			{
				Type = TipType.Tests,
			};

			foreach (var tip in EnumX.AllValues<TipTests>())
			{
				tips.Temporary += (UInt64)tip;
			}

			var full = tips.IsFull();

			Assert.That(full, Is.True);
		}

		[Test]
		public void IsFull_TestsPermanentAndTemporaryNotFull()
		{
			var tips = new Tips
			{
				Type = TipType.Tests,
			};

			var last = EnumX.AllValues<TipTests>()
				.Where(t => t != TipTests.None)
				.Select(t => (UInt64)t)
				.Max();

			tips.Permanent += last;
			tips.Temporary += last;

			var full = tips.IsFull();

			Assert.That(full, Is.False);
		}
		#endregion
	}
}
