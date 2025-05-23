﻿using System;
using System.IO;
using DFM.Entities;
using DFM.Entities.Enums;
using DFM.Generic;
using DFM.Language;
using DFM.Tests.Util;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace DFM.Email.Tests
{
	[Binding]
	public class Step : ContextHelper
	{
		public Step(ScenarioContext context) : base(context)
		{
			Cfg.Init();
			PlainText.Initialize(runPath);
		}

		[BeforeTestRun]
		public static void RegisterRun()
		{
			testsStart = DateTime.UtcNow;
		}

		[Given(@"I have this e-mail to send")]
		public void GivenIHaveThisEmailToSend(Table table)
		{
			var email = table.Rows[0];

			sender = new Sender()
				.Subject(email["Subject"])
				.Body(email["Body"])
				.To(email["To"]);

			if (email.ContainsKey("Unsubscribe Link"))
			{
				sender.UnsubscribeLink(email["Unsubscribe Link"]);
			}

			if (email.ContainsKey("Attachment"))
			{
				var path = Path.Combine("Templates", email["Attachment"]);
				sender.Attach(path);
			}
		}

		[Given(@"I have this e-mail to send to default")]
		public void GivenIHaveThisEmailToSendToDefault(Table table)
		{
			var email = table.Rows[0];

			sender = new Sender()
				.Subject(email["Subject"])
				.Body(email["Body"])
				.ToDefault();
		}

		[Given(@"an user")]
		public void GivenAnUser()
		{
			var email = $"{scenarioCode}@dontflymoney.com";

			user = new User
			{
				Email = email,
				Settings = new Settings
				{
					Language = "pt-BR"
				}
			};
		}

		[When(@"I try to send the e-mail")]
		public void WhenITryToSendTheEmail()
		{
			try
			{
				sender.Send();
			}
			catch (MailError e)
			{
				error = e;
			}
		}

		[When(@"a move notification is formatted")]
		public void WhenAMoveNotificationIsFormatted()
		{
			format = Format.MoveNotification(user);
		}

		[When(@"a security action is formatted to (\w+)")]
		public void WhenASecurityActionIsFormattedTo(SecurityAction action)
		{
			format = Format.SecurityAction(user, action);
		}

		[When(@"a security warning is formatted to (\w+)")]
		public void WhenASecurityWarningIsFormattedTo(SecurityWarning warning)
		{
			format = Format.SecurityWarning(user, warning);
		}

		[When(@"a user removal is formatted because of (\w+)")]
		public void WhenAUserRemovalIsFormattedBecauseOf(RemovalReason reason)
		{
			try
			{
				format = Format.UserRemoval(user, reason);
			}
			catch (MailError e)
			{
				error = e;
			}
		}

		[When(@"a wipe notice is formatted because of (\w+)")]
		public void WhenAWipeNoticeIsFormattedBecauseOf(RemovalReason reason)
		{
			format = Format.WipeNotice(user, reason);
		}

		[When(@"an export data is formatted")]
		public void WhenAnExportDataIsFormatted()
		{
			format = Format.ExportData(user);
		}

		[When(@"the e-mail is sent")]
		public void WhenTheEmailIsSent()
		{
			new Sender()
				.To(user.Email)
				.Subject(format.Subject)
				.Body(format.Layout)
				.Send();
		}

		[Then(@"I will receive this e-mail error: ([A-Za-z]+)")]
		public void ThenIWillReceiveThisError(String errorMessage)
		{
			Assert.That(error, Is.Not.Null);
			Assert.That(error.Type.ToString(), Is.EqualTo(errorMessage));
		}

		[Then(@"I will receive no e-mail error")]
		public void ThenIWillReceiveNoEmailError()
		{
			Assert.That(error, Is.Null);
		}

		[Then(@"there will be a ([\w\-]+) e-mail sent")]
		public void ThenThereWillBeATemplateEmailSent(String template)
		{
			var path = Path.Combine("Templates", $"{template}.html");
			var pattern = File.ReadAllText(path)
				.Replace("{{", @"\{\{")
				.Replace("}}", @"\}\}")
				.Replace("/", @"\/")
				.Replace("\r", "")
				.Replace("\n", "")
				.Replace("\t", "");

			var emailSent = EmlHelper.ByEmail(user.Email, testsStart);

			Assert.That(
				emailSent, Is.Not.Null,
				$"No email for {testsStart} - now {DateTime.Now} / {DateTime.UtcNow}"
			);

			var body = emailSent?.Body
				.Replace("\r", "")
				.Replace("\n", "")
				.Replace("\t", "");

			var resultPath = Path.Combine("Templates", "result.html");
			File.WriteAllText(resultPath, body);
			Assert.That(match(body, pattern), Is.True);
		}

		[Then(@"there will be no e-mail sent")]
		public void ThenThereWillNoEmailSent()
		{
			var email = EmlHelper.ByEmail(user.Email, testsStart);
			Assert.That(email, Is.Null);
		}

		[Then("there will be a header in the email sent to (.+) with the link (.+)")]
		public void ThenThereWillBeAHeaderWithTheLink(String email, String headerLink)
		{
			var emailSent = EmlHelper.ByEmail(email, testsStart);

			Assert.That(
				emailSent, Is.Not.Null,
				$"No email for {testsStart} - now {DateTime.Now} / {DateTime.UtcNow}"
			);

			Assert.That(
				emailSent?.Headers["List-Unsubscribe-Post"],
				Is.EqualTo("List-Unsubscribe=One-Click")
			);

			Assert.That(
				emailSent?.Headers["List-Unsubscribe"],
				Is.EqualTo(headerLink)
			);
		}

		[Then(@"there will be an attachment in the email sent to (.+) with the content of (.+)")]
		public void ThenThereWillBeAnAttachmentInTheEmailSentToWithTheContentOf(String email, String file)
		{
			var emailSent = EmlHelper.ByEmail(email, testsStart);

			file = Path.Combine("Templates", file);
			var content = File.ReadAllText(file);

			var defaultAttachmentStart =
				"<div style='text-align: center; font-family: courier new; padding: 3px; background: #000; border-top: 6px double #C00; border-bottom: 6px double #80C; color: #CCC; font-weight: bold;'>OCTET-STREAM</div>\n\ufeff";

			content = (defaultAttachmentStart + content).Trim();

			Assert.That(emailSent.Attachments, Is.Not.Empty);
			Assert.That(emailSent.Attachments.Count, Is.EqualTo(1));
			Assert.That(emailSent.Attachments[0], Is.EqualTo(content));
		}

		private MailError error
		{
			get => get<MailError>();
			set => set(value);
		}

		private Sender sender
		{
			get => get<Sender>();
			set => set(value);
		}

		private User user
		{
			get => get<User>();
			set => set(value);
		}

		private Format format
		{
			get => get<Format>();
			set => set(value);
		}

		protected static DateTime testsStart { get; private set; }
	}
}
