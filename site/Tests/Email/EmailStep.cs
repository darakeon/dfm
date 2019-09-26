using System;
using DFM.Email;
using DFM.Tests.BusinessLogic.Helpers;
using DFM.Tests.Helpers;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace DFM.Tests.Email
{
	[Binding]
	public class EmailStep : ContextHelper
	{

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

		[When(@"I try to send the e-mail")]
		public void WhenITryToSendTheEMail()
		{
			ConfigHelper.ActivateEmailSystem();

			try
			{
				sender.Send();
			}
			catch (MailError e)
			{
				error = e;
			}

			ConfigHelper.DeactivateEmailSystem();
		}



		[Then(@"I will receive this e-mail error: ([A-Za-z]+)")]
		public void ThenIWillReceiveThisError(String errorMessage)
		{
			Assert.IsNotNull(error);
			Assert.AreEqual(errorMessage, error.Type.ToString());
		}

		[Then(@"I will receive no e-mail error")]
		public void ThenIWillReceiveNoError()
		{
			Assert.IsNull(error);
		}


		private static MailError error
		{
			get => get<MailError>("error");
			set => set("error", value);
		}

		private static Sender sender
		{
			get => get<Sender>("sender");
			set => set("sender", value);
		}




	}
}
