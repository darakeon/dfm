using System;
using Redirector.Structure;

namespace Redirector.Tests
{
	class StatusTest
	{
		private static Verdict pass => new Verdict { Status = "PASS" };
		private static Verdict fail => new Verdict { Status = "FAIL" };
		private static Verdict gray => new Verdict { Status = "GRAY" };
		private static Verdict processFailed => new Verdict { Status = "PROCESSING_FAILED" };

		public static void Test()
		{
			TestAllStati();
			TestReceiptDkimFailed();
			TestReceiptDmarcFailed();
			TestReceiptSpamFailed();
			TestReceiptVirusFailed();
			TestReceiptSpfFailed();
			TestReceiptDkimProcessFailed();
			TestReceiptDmarcProcessFailed();
			TestReceiptSpamProcessFailed();
			TestReceiptVirusProcessFailed();
			TestReceiptSpfProcessFailed();
			TestReceiptDkimGray();
			TestReceiptDmarcGray();
			TestReceiptSpamGray();
			TestReceiptVirusGray();
			TestReceiptSpfGray();
			TestReceiptWithoutDkim();
			TestReceiptWithoutDmarc();
			TestReceiptWithoutSpam();
			TestReceiptWithoutVirus();
			TestReceiptWithoutSpf();
			TestReceiptAllPassed();
		}

		public static void TestAllStati()
		{
			assertTrue(pass.IsValid(), "TestAllStatusPass");
			assertFalse(fail.IsValid(), "TestAllStatusFail");
			assertTrue(gray.IsValid(), "TestAllStatusGray");
			assertFalse(processFailed.IsValid(), "TestAllStatusProcessFailed");
			assertTrue(new Verdict().IsValid(), "TestAllStatusNull");
		}

		public static void TestReceiptDkimFailed()
		{
			var receipt = new Receipt
			{
				DkimVerdict = fail,
				DmarcVerdict = pass,
				SpamVerdict = pass,
				VirusVerdict = pass,
				SpfVerdict = pass,
			};

			assertFalse(receipt.IsValid, "TestReceiptDkimFailed");
		}

		public static void TestReceiptDmarcFailed()
		{
			var receipt = new Receipt
			{
				DkimVerdict = pass,
				DmarcVerdict = fail,
				SpamVerdict = pass,
				VirusVerdict = pass,
				SpfVerdict = pass,
			};

			assertFalse(receipt.IsValid, "TestReceiptDmarcFailed");
		}

		public static void TestReceiptSpamFailed()
		{
			var receipt = new Receipt
			{
				DkimVerdict = pass,
				DmarcVerdict = pass,
				SpamVerdict = fail,
				VirusVerdict = pass,
				SpfVerdict = pass,
			};

			assertFalse(receipt.IsValid, "TestReceiptSpamFailed");
		}

		public static void TestReceiptVirusFailed()
		{
			var receipt = new Receipt
			{
				DkimVerdict = pass,
				DmarcVerdict = pass,
				SpamVerdict = pass,
				VirusVerdict = fail,
				SpfVerdict = pass,
			};

			assertFalse(receipt.IsValid, "TestReceiptVirusFailed");
		}

		public static void TestReceiptSpfFailed()
		{
			var receipt = new Receipt
			{
				DkimVerdict = pass,
				DmarcVerdict = pass,
				SpamVerdict = pass,
				VirusVerdict = pass,
				SpfVerdict = fail,
			};

			assertFalse(receipt.IsValid, "TestReceiptSpfFailed");
		}

		public static void TestReceiptDkimProcessFailed()
		{
			var receipt = new Receipt
			{
				DkimVerdict = processFailed,
				DmarcVerdict = pass,
				SpamVerdict = pass,
				VirusVerdict = pass,
				SpfVerdict = pass,
			};

			assertFalse(receipt.IsValid, "TestReceiptDkimProcessFailed");
		}

		public static void TestReceiptDmarcProcessFailed()
		{
			var receipt = new Receipt
			{
				DkimVerdict = pass,
				DmarcVerdict = processFailed,
				SpamVerdict = pass,
				VirusVerdict = pass,
				SpfVerdict = pass,
			};

			assertFalse(receipt.IsValid, "TestReceiptDmarcProcessFailed");
		}

		public static void TestReceiptSpamProcessFailed()
		{
			var receipt = new Receipt
			{
				DkimVerdict = pass,
				DmarcVerdict = pass,
				SpamVerdict = processFailed,
				VirusVerdict = pass,
				SpfVerdict = pass,
			};

			assertFalse(receipt.IsValid, "TestReceiptSpamProcessFailed");
		}

		public static void TestReceiptVirusProcessFailed()
		{
			var receipt = new Receipt
			{
				DkimVerdict = pass,
				DmarcVerdict = pass,
				SpamVerdict = pass,
				VirusVerdict = processFailed,
				SpfVerdict = pass,
			};

			assertFalse(receipt.IsValid, "TestReceiptVirusProcessFailed");
		}

		public static void TestReceiptSpfProcessFailed()
		{
			var receipt = new Receipt
			{
				DkimVerdict = pass,
				DmarcVerdict = pass,
				SpamVerdict = pass,
				VirusVerdict = pass,
				SpfVerdict = processFailed,
			};

			assertFalse(receipt.IsValid, "TestReceiptSpfProcessFailed");
		}

		public static void TestReceiptDkimGray()
		{
			var receipt = new Receipt
			{
				DkimVerdict = gray,
				DmarcVerdict = pass,
				SpamVerdict = pass,
				VirusVerdict = pass,
				SpfVerdict = pass,
			};

			assertTrue(receipt.IsValid, "TestReceiptDkimGray");
		}

		public static void TestReceiptDmarcGray()
		{
			var receipt = new Receipt
			{
				DkimVerdict = pass,
				DmarcVerdict = gray,
				SpamVerdict = pass,
				VirusVerdict = pass,
				SpfVerdict = pass,
			};

			assertTrue(receipt.IsValid, "TestReceiptDmarcGray");
		}

		public static void TestReceiptSpamGray()
		{
			var receipt = new Receipt
			{
				DkimVerdict = pass,
				DmarcVerdict = pass,
				SpamVerdict = gray,
				VirusVerdict = pass,
				SpfVerdict = pass,
			};

			assertTrue(receipt.IsValid, "TestReceiptSpamGray");
		}

		public static void TestReceiptVirusGray()
		{
			var receipt = new Receipt
			{
				DkimVerdict = pass,
				DmarcVerdict = pass,
				SpamVerdict = pass,
				VirusVerdict = gray,
				SpfVerdict = pass,
			};

			assertTrue(receipt.IsValid, "TestReceiptVirusGray");
		}

		public static void TestReceiptSpfGray()
		{
			var receipt = new Receipt
			{
				DkimVerdict = pass,
				DmarcVerdict = pass,
				SpamVerdict = pass,
				VirusVerdict = pass,
				SpfVerdict = gray,
			};

			assertTrue(receipt.IsValid, "TestReceiptSpfGray");
		}

		public static void TestReceiptWithoutDkim()
		{
			var receipt = new Receipt
			{
				DmarcVerdict = pass,
				SpamVerdict = pass,
				VirusVerdict = pass,
				SpfVerdict = pass,
			};

			assertTrue(receipt.IsValid, "TestReceiptWithoutDkim");
		}

		public static void TestReceiptWithoutDmarc()
		{
			var receipt = new Receipt
			{
				DkimVerdict = pass,
				SpamVerdict = pass,
				VirusVerdict = pass,
				SpfVerdict = pass,
			};

			assertTrue(receipt.IsValid, "TestReceiptWithoutDmarc");
		}

		public static void TestReceiptWithoutSpam()
		{
			var receipt = new Receipt
			{
				DkimVerdict = pass,
				DmarcVerdict = pass,
				VirusVerdict = pass,
				SpfVerdict = pass,
			};

			assertTrue(receipt.IsValid, "TestReceiptWithoutSpam");
		}

		public static void TestReceiptWithoutVirus()
		{
			var receipt = new Receipt
			{
				DkimVerdict = pass,
				DmarcVerdict = pass,
				SpamVerdict = pass,
				SpfVerdict = pass,
			};

			assertTrue(receipt.IsValid, "TestReceiptWithoutVirus");
		}

		public static void TestReceiptWithoutSpf()
		{
			var receipt = new Receipt
			{
				DkimVerdict = pass,
				DmarcVerdict = pass,
				SpamVerdict = pass,
				VirusVerdict = pass,
			};

			assertTrue(receipt.IsValid, "TestReceiptWithoutSpf");
		}

		public static void TestReceiptAllPassed()
		{
			var receipt = new Receipt
			{
				DkimVerdict = pass,
				DmarcVerdict = pass,
				SpamVerdict = pass,
				VirusVerdict = pass,
				SpfVerdict = pass,
			};

			assertTrue(receipt.IsValid, "TestReceiptAllPassed");
		}

		private static void assertFalse(Boolean actual, String test)
		{
			assert(actual, false, test);
		}

		private static void assertTrue(Boolean actual, String test)
		{
			assert(actual, true, test);
		}

		private static void assert<T>(T actual, T expected, String test)
		{
			if (!actual.Equals(expected))
			{
				Console.Error.WriteLine($"[{test}] Actual: {actual}, Expected: {expected}");
			}
		}
	}
}
