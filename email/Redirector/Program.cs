using System;
using System.IO;
using System.Text;
using Amazon.Lambda.Serialization.SystemTextJson;
using Redirector.Structure;

namespace Redirector
{
	class Program
	{
		private static bool valid = true;

		public static void Main()
		{
			testSES();
		}

		private static void testS3()
		{
			var s3 = new S3();
			s3.GetFile("AMAZON_SES_SETUP_NOTIFICATION");
			s3.DeleteFile("AMAZON_SES_SETUP_NOTIFICATION");
		}

		private static void testSES()
		{
			var package = createPackage();

			assert(package.Records.Count, 1);

			var record = package.Records[0];

			assert(record.EventSource, "aws:ses");
			assert(record.EventVersion, "1.0");

			var ses = record.SES;

			var mail = ses.Mail;
			assert(mail.Timestamp, new DateTime(1970, 01, 01));
			assert(mail.Source, "janedoe@example.com");
			assert(mail.MessageId, "AMAZON_SES_SETUP_NOTIFICATION");
			assert(mail.Destination.Length, 1);
			assert(mail.Destination[0], "johndoe@example.com");
			assert(mail.HeadersTruncated, false);

			var header = mail.Headers;
			assert(header[0].Name, "Return-Path");
			assert(header[0].Value, "<janedoe@example.com>");
			assert(header[1].Name, "Received");
			assert(
				header[1].Value,
				"from mailer.example.com (mailer.example.com [203.0.113.1]) " +
				"by inbound-smtp.us-west-2.amazonaws.com with SMTP id o3vrnil0e2ic " +
				"for johndoe@example.com; Wed, 07 Oct 2015 12:34:56 +0000 (UTC)"
			);
			assert(header[2].Name, "DKIM-Signature");
			assert(
				header[2].Value,
				"v=1; a=rsa-sha256; c=relaxed/relaxed; d=example.com; s=example; " +
				"h=mime-version:from:date:message-id:subject:to:content-type; " +
				"bh=jX3F0bCAI7sIbkHyy3mLYO28ieDQz2R0P8HwQkklFj4=; " +
				"b=sQwJ+LMe9RjkesGu+vqU56asvMhrLRRYrWCbV"
			);
			assert(header[3].Name, "MIME-Version");
			assert(header[3].Value, "1.0");
			assert(header[4].Name, "From");
			assert(header[4].Value, "Jane Doe <janedoe@example.com>");
			assert(header[5].Name, "Date");
			assert(header[5].Value, "Wed, 7 Oct 2015 12:34:56 -0700");
			assert(header[6].Name, "Message-ID");
			assert(header[6].Value, "<0123456789example.com>");
			assert(header[7].Name, "Subject");
			assert(header[7].Value, "Test Subject");
			assert(header[8].Name, "To");
			assert(header[8].Value, "johndoe@example.com");
			assert(header[9].Name, "Content-Type");
			assert(header[9].Value, "text/plain; charset=UTF-8");

			var commonHeaders = mail.CommonHeaders;
			assert(commonHeaders.ReturnPath, "janedoe@example.com");
			assert(commonHeaders.From.Length, 1);
			assert(commonHeaders.From[0], "Jane Doe <janedoe@example.com>");
			assert(commonHeaders.XDate, new DateTime(2015, 10, 7, 16, 34, 56));
			assert(commonHeaders.To.Length, 1);
			assert(commonHeaders.To[0], "johndoe@example.com");
			assert(commonHeaders.MessageId, "<0123456789example.com>");
			assert(commonHeaders.Subject, "Test Subject");

			var receipt = ses.Receipt;
			assert(receipt.Timestamp, new DateTime(1970, 1, 1));
			assert(receipt.ProcessingTimeMillis, 574);
			assert(receipt.Recipients.Length, 1);
			assert(receipt.Recipients[0], "johndoe@example.com");
			assert(receipt.SpamVerdict.XStatus, Status.Pass);
			assert(receipt.VirusVerdict.XStatus, Status.Pass);
			assert(receipt.SpfVerdict.XStatus, Status.Pass);
			assert(receipt.DkimVerdict.XStatus, Status.Pass);
			assert(receipt.Status, Status.Pass);

			var action = receipt.Action;
			assert(action.Type, "Lambda");
			assert(action.FunctionArn, "arn:aws:lambda:us-west-2:012345678912:function:Example");
			assert(action.InvocationType, "Event");

			if (valid)
				Handler.Send(package, null);
		}

		private static Package createPackage()
		{
			var json = File.ReadAllText("example.json");
			var bytes = Encoding.UTF8.GetBytes(json);
			var example = new MemoryStream(bytes);
			return new DefaultLambdaJsonSerializer()
				.Deserialize<Package>(example);
		}

		private static void assert<T>(T value, T expected)
		{
			if (value.Equals(expected))
				return;

			Console.WriteLine($"Difference: {value}, expected {expected}");
			valid = false;
		}
	}
}
