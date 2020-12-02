using System;
using System.IO;
using System.Linq;
using System.Text;
using DFM.Generic;

namespace DFM.BusinessLogic.Tests
{
	class Email
	{
		private Email(String subject, String body)
		{
			Subject = subject;
			Body = body;
		}

		public String Subject { get; }
		public String Body { get; }

		public static Email GetLast()
		{
			var dir = new DirectoryInfo(Cfg.Smtp.PickupDirectory);

			var lastEmail = dir.EnumerateFiles()
				.OrderByDescending(f => f.CreationTime)
				.First().FullName;

			var content = File.ReadAllLines(lastEmail);

			var base64 = content[8].EndsWith("base64");

			var subject = content[6].Substring(9);

			content = content
				.Skip(9)
				.Where(c => !String.IsNullOrEmpty(c))
				.ToArray();

			if (!base64)
			{
				content = content
					.Select(
						c => c.Substring(0, c.Length - 1)
					).ToArray();
			}

			var body = String.Join("", content);

			if (base64)
			{
				subject = convert(
					subject.Substring(10, subject.Length - 12)
				);

				body = convert(body);
			}

			return new Email(subject, body);
		}

		private static String convert(String text)
		{
			var bytes = Convert.FromBase64String(text);
			return Encoding.UTF8.GetString(bytes);
		}
	}
}
