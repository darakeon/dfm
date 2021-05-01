using System;
using System.Collections.Generic;
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

		public static Email GetByPosition(Int32 position)
		{
			// cannot have -0 and +0
			if (position == 0) return null;

			var dir = new DirectoryInfo(Cfg.Smtp.PickupDirectory);
			var files = dir.EnumerateFiles("*.eml");

			files = position > 0
				? files.OrderBy(f => f.CreationTime)
				: files.OrderByDescending(f => f.CreationTime);

			position = position > 0 ? position : -position;

			var lastEmail = files.Skip(position-1).First().FullName;

			var content = File.ReadAllLines(lastEmail);

			var headers = new Dictionary<String, String>();

			var l = 0;
			for (; l < content.Length && content[l] != ""; l++)
			{
				var line = content[l];

				if (line.StartsWith(" "))
				{
					var key = headers.Keys.Last();
					headers[key] += line;
				}
				else
				{
					var parts = line.Split(": ", 2);
					headers.Add(parts[0], parts[1]);
				}
			}

			var base64 = headers["Content-Transfer-Encoding"] == "base64";
			var subject = headers["Subject"];

			content = content.Skip(l).ToArray();

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
				subject = String.Join("",
					subject
						.Split(" ")
						.Select(s => convert(s[10..^2]))
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
