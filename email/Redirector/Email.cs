using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;

namespace Redirector
{
	public class Email
	{
		public Email(String[] from, String subject, String messageId)
		{
			this.from = from;
			this.subject = subject;
			this.messageId = messageId;
			s3 = new S3();

			Console.WriteLine($"[{messageId}] {subject}");
		}

		private readonly String[] from;
		private readonly String subject;
		private readonly String messageId;

		private readonly S3 s3;

		private String body;
		private Boolean s3EmailFileExists;

		private readonly String newLine = Environment.NewLine;

		private readonly IDictionary<String, StringBuilder> received =
			new Dictionary<String, StringBuilder>();

		public Email BuildBody(DateTime date, String[] to, params String[] messageIds)
		{
			var toAll = String.Join(",", to);
			var messageIdsAll = String.Join(",", messageIds);

			body = $"To: {toAll}<br />"
				+ $"Date: {date}<br />"
				+ $"Message IDs: {messageIdsAll}<br />"
				+ bodyParse();

			return this;
		}

		private String bodyParse()
		{
			var email = getEmailFile();

			if (!s3EmailFileExists)
				return $"Could not recover e-mail, look for {messageId}";

			var contents = email
				.Split($"{newLine}Content-Type:")
				.ToList();

			if (contents.Count == 1)
				return email;

			var boundary = isolate(email, "boundary=\"(\\w+)\"");

			foreach (var content in contents.Skip(1))
			{
				processContent(content, boundary);
			}

			Console.WriteLine($"Found options: {String.Join(", ", received.Keys)}");

			if (received.ContainsKey("text/html"))
				return received["text/html"].ToString()
					.Replace($"={newLine}", "")
					.Replace("=0A", "<br />")
					.Replace("=3D", "=");

			if (received.ContainsKey("text/plain"))
				return received["text/plain"].ToString();

			return email;
		}

		private String getEmailFile()
		{
			try
			{
				var content = s3.GetFile(messageId);
				Console.WriteLine($"{messageId}: {content}");
				s3EmailFileExists = true;
				return content;
			}
			catch (Exception e)
			{
				Console.WriteLine("Fail while reading S3:");
				Console.WriteLine(e);
				s3EmailFileExists = false;
				return null;
			}
		}

		private String isolate(String text, String pattern)
		{
			var match = Regex.Match(text, pattern);
			return match.Success ? match.Groups[1].Value : null;
		}

		private void processContent(String content, String boundary)
		{
			var lines = content
				.Split(newLine)
				.ToList();

			var key = isolate(lines[0], " (.+);");

			lines = lines.Skip(1)
				.Where(l => !l.StartsWith("Content-Transfer-Encoding:"))
				.Where(l => boundary == null || !l.Contains(boundary))
				.Select(l => l.Trim())
				.Where(l => !String.IsNullOrEmpty(l))
				.ToList();

			if (!lines.Any())
				return;

			received.Add(key, new StringBuilder());

			foreach (var line in lines)
			{
				received[key].AppendLine(line);
			}
		}

		public void Send()
		{
			var message = buildMessage();

			var credential = new NetworkCredential(
				Cfg.Smtp.Username,
				Cfg.Smtp.Password
			);

			var host = $"email-smtp.{Cfg.Region.SystemName}.amazonaws.com";
			var client = new SmtpClient(host, Cfg.Smtp.Port)
			{
				Credentials = credential,
				EnableSsl = true
			};

			Console.WriteLine($"Trying to send the e-mail {messageId}");
			client.Send(message);
			Console.WriteLine($"E-mail {messageId} sent successfully!");

			if (s3EmailFileExists)
				s3.DeleteFile(messageId);
		}

		private MailMessage buildMessage()
		{
			var message = new MailMessage
			{
				IsBodyHtml = true,
				From = new MailAddress(Cfg.Smtp.From, Cfg.Smtp.FromName),
				Subject = subject,
				Body = body,
			};
			message.To.Add(new MailAddress(Cfg.Smtp.To));

			foreach (var contact in from)
			{
				add(message.ReplyToList, contact);
			}

			return message;
		}

		private void add(MailAddressCollection list, String contact)
		{
			var regex = new Regex("^(.+) ?<(.+)>");

			if (regex.IsMatch(contact))
			{
				var pieces = regex.Match(contact)
					.Groups.Values
					.Select(g => g.Value).ToList();
				var fromName = pieces[1];
				var fromMail = pieces[2];

				list.Add(new MailAddress(fromMail, fromName));
			}
			else
			{
				list.Add(contact);
			}
		}
	}
}
