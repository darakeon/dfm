using System;
using System.Linq;
using Amazon.Lambda.Core;
using Amazon.Lambda.Serialization.SystemTextJson;
using Redirector.Structure;

namespace Redirector
{
	public class Handler
	{
		[LambdaSerializer(typeof(DefaultLambdaJsonSerializer))]
		public static void Send(Package package, ILambdaContext context)
		{
			Console.WriteLine("Started");

			foreach (var record in package.Records)
			{
				var receipt = record.SES.Receipt;
				var mail = record.SES.Mail;
				var headers = mail.CommonHeaders;

				if (wrongEvent(record) || spam(headers, receipt))
					continue;

				sendEmail(headers, mail);
			}
		}

		private static Boolean wrongEvent(Record record)
		{
			if (record.EventSource == "aws:ses" && record.EventVersion == "1.0")
				return false;

			Console.WriteLine("EVENT PROBLEM!");
			Console.WriteLine(record.EventSource);
			Console.WriteLine(record.EventVersion);

			return true;
		}

		private static Boolean spam(CommonHeaders headers, Receipt receipt)
		{
			if (receipt.IsValid)
				return false;

			Console.WriteLine("SPAM!");
			Console.WriteLine(headers.From);
			Console.WriteLine(headers.Date);
			Console.WriteLine(headers.Subject);
			Console.WriteLine(receipt.VirusVerdict);
			Console.WriteLine(receipt.SpamVerdict);
			Console.WriteLine(receipt.SpfVerdict);
			Console.WriteLine(receipt.DkimVerdict);

			return true;
		}

		private static void sendEmail(CommonHeaders headers, Mail mail)
		{
			new Email(
				createFrom(headers, mail),
				headers.Subject,
				mail.MessageId
			).BuildBody(
				mail.Timestamp,
				headers.To,
				mail.MessageId,
				mail.CommonHeaders.MessageId
			).Send();
		}

		private static String[] createFrom(CommonHeaders headers, Mail mail)
		{
			return headers.From
				.Union(new[] { headers.ReturnPath, mail.Source })
				.ToArray();
		}
	}
}
