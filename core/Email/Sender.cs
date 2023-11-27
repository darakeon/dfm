using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Mail;
using DFM.Generic;
using DfM.Logs;

namespace DFM.Email
{
	public class Sender
	{
		private readonly String from;
		private String to, subject, body;
		private readonly IList<String> files;
		private readonly NameValueCollection headers;
		private readonly String @default;

		public Sender()
		{
			files = new List<String>();
			headers = new NameValueCollection();

			if (Cfg.Smtp.From != null)
				from = Cfg.Smtp.From;

			@default = Cfg.Email;
		}

		public Sender To(String email)
		{
			to = email;
			return this;
		}

		public Sender ToDefault()
		{
			to = @default;
			return this;
		}

		public Sender Subject(String text)
		{
			subject = text;
			return this;
		}

		public Sender Body(String html)
		{
			body = html;
			return this;
		}

		public Sender Attach(String fileFullName)
		{
			files.Add(fileFullName);
			return this;
		}

		public Sender UnsubscribeLink(String link)
		{
			headers.Add("List-Unsubscribe-Post", "List-Unsubscribe=One-Click");
			headers.Add("List-Unsubscribe", link);
			return this;
		}

		public void Send()
		{
			if (Cfg.ForceEmailError)
				throw MailError.WithMessage(EmailStatus.EmailNotSent);

			if (String.IsNullOrEmpty(subject))
				throw MailError.WithMessage(EmailStatus.InvalidSubject);

			if (String.IsNullOrEmpty(body))
				throw MailError.WithMessage(EmailStatus.InvalidBody);

			if (String.IsNullOrEmpty(to))
				throw MailError.WithMessage(EmailStatus.InvalidAddress);

			var smtp = Cfg.Smtp;

			var credentials = new NetworkCredential(
				smtp.UserName,
				smtp.Password
			);

			using var client = new SmtpClient(smtp.Host, smtp.Port)
			{
				Timeout = 60000,
				DeliveryMethod = smtp.DeliveryMethod,
				EnableSsl = smtp.EnableSsl,
				UseDefaultCredentials = smtp.DefaultCredentials,
				Credentials = credentials,
				PickupDirectoryLocation = smtp.PickupDirectory
			};

			try
			{
				var message =
					new MailMessage(from, to, subject, body)
						{IsBodyHtml = true};

				var attachments = files.Select(
					fileFullName => new Attachment(fileFullName)
				);

				foreach (var attachment in attachments)
				{
					message.Attachments.Add(attachment);
				}

				message.Headers.Add(headers);

				client.Send(message);
			}
			catch (Exception exception)
			{
				exception.TryLog();
				throw MailError.WithMessage(exception);
			}
		}
	}
}
