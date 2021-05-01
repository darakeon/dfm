using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using DFM.Entities.Enums;
using DFM.Generic;
using DFM.Language;
using DFM.Language.Emails;

namespace DFM.BusinessLogic.Tests
{
	class Email
	{
		private Email(String receiver, String subject, String body, DateTime creation, EmailType? type)
		{
			Receiver = receiver;
			Subject = subject;
			Body = body;
			Creation = creation;
			Type = type;
		}

		public String Receiver { get; }
		public String Subject { get; }
		public String Body { get; }
		public DateTime Creation { get; }
		public EmailType? Type { get; }

		private static IDictionary<String, EmailType> emailTypesField;
		private static IDictionary<String, EmailType> emailTypes =>
			emailTypesField ??= chargeTypes();

		private static IDictionary<String, EmailType> chargeTypes()
		{
			var types = new Dictionary<String, EmailType>
			{
				{EmailType.MoveNotification.ToString(), EmailType.MoveNotification},
			};

			EnumX.AllExcept(SecurityAction.UnsubscribeMoveMail)
				.ForEach(
					sa => types.Add(sa.ToString(), EmailType.SecurityAction)
				);

			EnumX.AllValues<RemovalReason>()
				.ForEach(
					rr => types.Add(rr.ToString(), EmailType.RemovalReason)
				);

			var langs = PlainText.AcceptedLanguages();
			var keys = types.Keys.ToList();

			foreach (var key in keys)
			{
				var value = types[key];
				types.Remove(key);

				foreach (var lang in langs)
				{
					var newKey = PlainText.Email[key, lang, "Subject"];
					types.Add(newKey, value);
				}
			}

			return types;
		}

		public static Email GetByPosition(Int32 position)
		{
			// cannot have -0 and +0
			if (position == 0) return null;

			var emailFile = getEmailFile(position);

			return fillEmail(emailFile);
		}

		private static FileInfo getEmailFile(Int32 position)
		{
			var files = getEmailFiles();

			files = position > 0
				? files.OrderBy(f => f.CreationTime)
				: files.OrderByDescending(f => f.CreationTime);

			position = position > 0 ? position : -position;

			return files.Skip(position - 1).First();
		}

		private static IEnumerable<FileInfo> getEmailFiles()
		{
			var dir = new DirectoryInfo(Cfg.Smtp.PickupDirectory);
			return dir.EnumerateFiles("*.eml");
		}

		private static Email fillEmail(FileInfo emailFile)
		{
			var content = File.ReadAllLines(emailFile.FullName);

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

			var receiver = headers["X-Receiver"];

			var base64 = headers["Content-Transfer-Encoding"] == "base64";
			var subject = headers["Subject"];

			content = content.Skip(l).ToArray();

			if (!base64)
			{
				content = content
					.Where(c => !String.IsNullOrEmpty(c))
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

			var creation = emailFile.CreationTimeUtc;
			var type = emailTypes.FirstOrDefault(
				t => t.Key == subject
			).Value;

			return new Email(receiver, subject, body, creation, type);
		}

		private static String convert(String text)
		{
			var bytes = Convert.FromBase64String(text);
			return Encoding.UTF8.GetString(bytes);
		}

		public static Int32 CountEmails(String email, EmailType type, DateTime datetime)
		{
			return getEmailFiles()
				.Where(f => f.CreationTimeUtc > datetime)
				.Select(fillEmail)
				.Count(e => e.Receiver == email && e.Type == type);
		}
	}
}
