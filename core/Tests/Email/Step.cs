using System;
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

		[Given(@"I have this e-mail to send")]
		public void GivenIHaveThisEMailToSend(Table table)
		{
			var email = table.Rows[0];

			sender = new Sender()
				.Subject(email["Subject"])
				.Body(email["Body"])
				.To(email["To"]);
		}

		[Given(@"I have this e-mail to send to default")]
		public void GivenIHaveThisEMailToSendToDefault(Table table)
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
		public void WhenITryToSendTheEMail()
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

		[When(@"a user removal is formatted because of (\w+)")]
		public void WhenAUserRemovalIsFormattedBecauseOf(RemovalReason reason)
		{
			format = Format.UserRemoval(user, reason);
		}

		[When(@"a wipe notice is formatted because of (\w+)")]
		public void WhenAWipeNoticeIsFormattedBecauseOf(RemovalReason reason)
		{
			format = Format.WipeNotice(user, reason);
		}

		[When(@"the e-mail is sent")]
		public void WhenTheEMailIsSent()
		{
			var fileContent = format.Layout;

			new Sender()
				.To(user.Email)
				.Subject(format.Subject)
				.Body(fileContent)
				.Send();
		}

		[Then(@"I will receive this e-mail error: ([A-Za-z]+)")]
		public void ThenIWillReceiveThisError(String errorMessage)
		{
			Assert.IsNotNull(error);
			Assert.AreEqual(errorMessage, error.Type.ToString());
		}

		[Then(@"I will receive no e-mail error")]
		public void ThenIWillReceiveNoEmailError()
		{
			Assert.IsNull(error);
		}

		[Then(@"there will be a ([\w\-]+) e-mail sent")]
		public void ThenThereWillBeAE_MailSent(String template)
		{
			var path = Path.Combine("Templates", $"{template}.html");
			var pattern = File.ReadAllText(path)
				.Replace("{{", @"\{\{")
				.Replace("}}", @"\}\}")
				.Replace("/", @"\/")
				.Replace("\n", "")
				.Replace("\t", "");

			var email = EmlHelper.ByEmail(user.Email);

			var body = email.Body
				.Replace("\n", "")
				.Replace("\t", "");

			Assert.True(match(body, pattern));
		}

		private MailError error
		{
			get => get<MailError>("error");
			set => set("error", value);
		}

		private Sender sender
		{
			get => get<Sender>("sender");
			set => set("sender", value);
		}

		private User user
		{
			get => get<User>("user");
			set => set("user", value);
		}

		private Format format
		{
			get => get<Format>("format");
			set => set("format", value);
		}
	}
}
