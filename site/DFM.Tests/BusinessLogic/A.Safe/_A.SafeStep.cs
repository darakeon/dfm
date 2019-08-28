using System;
using System.Collections.Generic;
using System.Linq;
using DFM.BusinessLogic.Exceptions;
using DFM.BusinessLogic.Helpers;
using DFM.Entities;
using DFM.Entities.Enums;
using DFM.Generic;
using DFM.Tests.BusinessLogic.Helpers;
using DFM.Tests.Helpers;
using Keon.TwoFactorAuth;
using NUnit.Framework;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
using TK = Keon.Util.Extensions.Token;

namespace DFM.Tests.BusinessLogic.A.Safe
{
	[Binding]
	public class SafeStep : BaseStep
	{
		#region Variables
		private static User otherUser
		{
			get => Get<User>("OtherUser");
			set => Set("OtherUser", value);
		}

		private static String email
		{
			get => Get<String>("Email");
			set => Set("Email", value);
		}

		private static String newEmail
		{
			get => Get<String>("NewEmail");
			set => Set("NewEmail", value);
		}

		private static String ticket
		{
			get => Get<String>("ticket");
			set => Set("ticket", value);
		}

		private static String password
		{
			get => Get<String>("Password");
			set => Set("Password", value);
		}

		private static String retypePassword
		{
			get => Get<String>("RetypePassword");
			set => Set("RetypePassword", value);
		}

		private static String newPassword
		{
			get => Get<String>("NewPassword");
			set => Set("NewPassword", value);
		}

		private static String currentPassword
		{
			get => Get<String>("CurrentPassword");
			set => Set("CurrentPassword", value);
		}

		private static String token
		{
			get => Get<String>("Token");
			set => Set("Token", value);
		}

		private static SecurityAction action
		{
			get => Get<SecurityAction>("Action");
			set => Set("Action", value);
		}

		private static IList<Ticket> logins
		{
			get => Get<IList<Ticket>>("logins");
			set => Set("logins", value);
		}

		private static Boolean? accepted
		{
			get => Get<Boolean?>("accepted");
			set => Set("accepted", value);
		}

		private static TFATable tfa
		{
			get => Get<TFATable>("tfa");
			set => Set("tfa", value);
		}

		private static Boolean? ticketVerified
		{
			get => Get<Boolean?>("ticketVerified");
			set => Set("ticketVerified", value);
		}
		#endregion


		#region SaveUserAndSendVerify
		[Given(@"I have this user data")]
		public void GivenIHaveThisUserData(Table table)
		{
			email = table.Rows[0]["Email"];
			password = table.Rows[0]["Password"];

			if (table.Header.Any(c => c == "Retype Password"))
				retypePassword = table.Rows[0]["Retype Password"];
		}

		[Given(@"I already have created this user")]
		public void GivenIAlreadyHaveCreatedThisUser()
		{
			otherUser = new User
			{
				Email = email,
				Password = password + "_diff",
			};

			var passwordForm = new PasswordForm(otherUser.Password, otherUser.Password);
			Service.Safe.SaveUserAndSendVerify(otherUser.Email, passwordForm, false, false, Defaults.CONFIG_LANGUAGE);

			var tokenActivate = DBHelper.GetLastTokenForUser(otherUser.Email, SecurityAction.UserVerification);
			Service.Safe.ActivateUser(tokenActivate);
		}



		[When(@"I try to save the user")]
		public void WhenITryToSaveTheUser()
		{
			try
			{
				var passwordForm = new PasswordForm(password, retypePassword);
				Service.Safe.SaveUserAndSendVerify(email, passwordForm, false, false, Defaults.CONFIG_LANGUAGE);
			}
			catch (DFMCoreException e)
			{
				Error = e;
			}
		}

		[Then(@"the user will not be saved")]
		public void ThenTheUserWillNotBeSaved()
		{
			ticket = null;
			Error = null;

			try
			{
				ticket = Service.Safe.ValidateUserAndCreateTicket(
					email, password, TicketKey, TicketType.Local
				);
			}
			catch (DFMCoreException e)
			{
				Error = e;
			}

			Assert.IsNull(ticket);
			Assert.IsNotNull(Error);
			Assert.AreEqual(DfMError.InvalidUser, Error.Type);
		}

		[Then(@"the user will not be changed")]
		public void ThenTheUserWillNotBeChanged()
		{
			var savedUser = GetSavedUser(otherUser.Email, otherUser.Password);

			Assert.IsNotNull(savedUser);
		}

		[Then(@"the user will be saved")]
		public void ThenTheUserWillBeSaved()
		{
			var tokenActivate = DBHelper.GetLastTokenForUser(email, SecurityAction.UserVerification);

			Service.Safe.ActivateUser(tokenActivate);

			var savedUser = GetSavedUser(email, password);

			Assert.IsNotNull(savedUser);
		}
		#endregion

		#region SendUserVerify
		/* Use same tests of others */
		#endregion

		#region SendPasswordReset
		[When(@"I try to send the e-mail of password reset")]
		public void WhenITryToSendTheEMailOfPasswordReset()
		{
			try
			{
				Service.Safe.SendPasswordReset(email);
			}
			catch (DFMCoreException e)
			{
				Error = e;
			}
		}
		#endregion

		#region ActivateUser
		[Given(@"I pass a token of password reset")]
		public void GivenIPassATokenOfPasswordReset()
		{
			Service.Safe.SendPasswordReset(User.Email);

			token = DBHelper.GetLastTokenForUser(User.Email, SecurityAction.PasswordReset);
		}

		[When(@"I try to activate the user")]
		public void WhenITryToActivateTheUser()
		{
			try
			{
				Service.Safe.ActivateUser(token);
			}
			catch (DFMCoreException e)
			{
				Error = e;
			}
		}

		[Then(@"the user will not be activated")]
		public void ThenTheUserWillNotBeActivated()
		{
			Error = null;

			try
			{
				Service.Safe.ValidateUserAndCreateTicket(
					email, password, TicketKey, TicketType.Local
				);
			}
			catch (DFMCoreException e)
			{
				Error = e;
			}

			Assert.IsNotNull(Error);
			Assert.AreEqual(DfMError.DisabledUser, Error.Type);
		}

		[Then(@"the user will be activated")]
		public void ThenTheUserWillBeActivated()
		{
			var ticketKey = Service.Safe.ValidateUserAndCreateTicket(
				email, password, TicketKey, TicketType.Local
			);

			User = Service.Safe.GetUserByTicket(ticketKey);

			Assert.IsTrue(User.Active);
		}
		#endregion

		#region ValidateUserAndGetTicket
		[Given(@"I activate the user")]
		public void GivenIActivateTheUser()
		{
			Service.Safe.SendUserVerify(email);

			var tokenToActivate = DBHelper.GetLastTokenForUser(email, SecurityAction.UserVerification);

			Service.Safe.ActivateUser(tokenToActivate);
		}

		[When(@"I try to get the ticket")]
		public void WhenITryToGetTheTicket()
		{
			ticket = null;
			Error = null;

			try
			{
				ticket = Service.Safe.ValidateUserAndCreateTicket(
					email, password, TicketKey, TicketType.Local
				);
			}
			catch (DFMCoreException e)
			{
				Error = e;
			}
		}

		[Given(@"I tried to get the ticket (\d+) times")]
		[When(@"I try to get the ticket (\d+) times")]
		public void WhenITryToGetTheTicketSomeTimes(Int32 times)
		{
			ticket = null;

			for (var t = 1; t < times; t++)
			{
				try
				{
					Service.Safe.ValidateUserAndCreateTicket(
						email, password, TicketKey, TicketType.Local
					);
				}
				catch (DFMCoreException) { }
			}

			try
			{
				Service.Safe.ValidateUserAndCreateTicket(
					email, password, TicketKey, TicketType.Local
				);
			}
			catch (DFMCoreException e)
			{
				Error = e;
			}
		}

		[Then(@"I will receive no ticket")]
		public void ThenIWillReceiveNoTicket()
		{
			Assert.IsNull(ticket);
		}

		[Then(@"I will receive the ticket")]
		public void ThenIWillReceiveTheTicket()
		{
			Assert.IsNotNull(ticket);

			var user = Service.Safe.GetUserByTicket(ticket);

			Assert.AreEqual(email, user.Email);
		}
		#endregion

		#region GetUserByTicket
		[When(@"I try to get the user")]
		public void WhenITryToGetTheUser()
		{
			User = null;

			try
			{
				User = Service.Safe.GetUserByTicket(ticket);
			}
			catch (DFMCoreException e)
			{
				Error = e;
			}
		}
		#endregion

		#region PasswordReset
		[Given(@"I pass a token of user verification")]
		public void GivenIPassATokenOfUserVerification()
		{
			token = DBHelper.GetLastTokenForUser(User.Email, SecurityAction.UserVerification);
		}

		[Given(@"I pass this password")]
		public void GivenIPassThisPassword(Table passwordData)
		{
			newPassword = passwordData.Rows[0]["Password"];
			retypePassword = passwordData.Rows[0]["Retype Password"];

			if (passwordData.Header.Any(c => c == "Current Password"))
				currentPassword = passwordData.Rows[0]["Current Password"];
		}

		[Given(@"I pass no password")]
		public void GivenIPassNoPassword()
		{
			newPassword = null;
		}


		[When(@"I try to reset the password")]
		public void WhenITryToResetThePassword()
		{
			try
			{
				var passwordForm = new PasswordForm(newPassword, retypePassword);
				Service.Safe.PasswordReset(token, passwordForm);
			}
			catch (DFMCoreException e)
			{
				Error = e;
			}
		}

		[Then(@"the password will not be changed")]
		public void ThenThePasswordWillNotBeChanged()
		{
			Error = null;

			try
			{
				var testLocalTicket = TK.New();

				Service.Safe.ValidateUserAndCreateTicket(
					email, password, testLocalTicket, TicketType.Local
				);
			}
			catch (DFMCoreException e)
			{
				if (e.Type != DfMError.DisabledUser)
					Error = e;
			}

			Assert.IsNull(Error);
		}

		[Then(@"the password will be changed")]
		public void ThenThePasswordWillBeChanged()
		{
			Service.Safe.SendUserVerify(email);
			token = DBHelper.GetLastTokenForUser(email, SecurityAction.UserVerification);
			Service.Safe.ActivateUser(token);

			var newTicket = Service.Safe.ValidateUserAndCreateTicket(
				email, newPassword, TicketKey, TicketType.Local
			);

			Assert.IsNotNull(newTicket);
		}
		#endregion

		#region TestSecurityToken
		[Given(@"I pass a token of ([A-Za-z]+) with action ([A-Za-z]+)")]
		public void GivenIPassATokenOfUserVerificationWithActionPasswordReset(String strTokenOf, String strAction)
		{
			var tokenOf = EnumX.Parse<SecurityAction>(strTokenOf);
			token = DBHelper.GetLastTokenForUser(email, tokenOf);

			action = EnumX.Parse<SecurityAction>(strAction);
		}

		[When(@"I test the token")]
		public void WhenITestTheToken()
		{
			try
			{
				Service.Safe.TestSecurityToken(token, action);
			}
			catch (DFMCoreException e)
			{
				Error = e;
			}
		}
		#endregion

		#region DisableToken
		[When(@"I try do disable the token")]
		public void WhenITryDoDisableTheToken()
		{
			try
			{
				Service.Safe.DisableToken(token);
			}
			catch (DFMCoreException e)
			{
				Error = e;
			}
		}

		[Then(@"the token will not be valid anymore")]
		public void ThenTheTokenWillNotBeValidAnymore()
		{
			try
			{
				Service.Safe.TestSecurityToken(token, action);
			}
			catch(DFMCoreException e)
			{
				Error = e;
			}

			Assert.IsNotNull(Error);
			Assert.AreEqual(DfMError.InvalidToken, Error.Type);
		}
		#endregion

		#region Disable Ticket
		[When(@"I try to disable the ticket")]
		public void WhenITryToDisableTheTicket()
		{
			try
			{
				Service.Safe.DisableTicket(ticket);
			}
			catch (DFMCoreException e)
			{
				Error = e;
			}
		}

		[Then(@"the ticket will not be valid anymore")]
		public void ThenTheTicketWillNotBeValidAnymore()
		{
			Error = null;

			try
			{
				Service.Safe.GetUserByTicket(ticket);
			}
			catch (DFMCoreException e)
			{
				Error = e;
			}

			Assert.IsNotNull(Error);
			Assert.AreEqual(DfMError.Uninvited, Error.Type);
		}

		[Then(@"the ticket will still be valid")]
		public void ThenTheTicketWillStillBeValid()
		{
			Error = null;

			try
			{
				Service.Safe.GetUserByTicket(ticket);
			}
			catch (DFMCoreException e)
			{
				Error = e;
			}

			Assert.IsNull(Error);
		}
		#endregion

		#region ListLogins
		[Given(@"I login the user")]
		public void GivenILoginTheUser()
		{
			Current.Set(USER_EMAIL, UserPassword, false);
		}

		[Given(@"I logoff the user")]
		public void GivenILogoffTheUser()
		{
			Current.Clear();
		}

		[When(@"I ask for current active logins")]
		public void WhenIAskForCurrentActiveLogins()
		{
			try
			{
				logins = Service.Safe.ListLogins();
			}
			catch (DFMCoreException e)
			{
				Error = e;
			}
		}

		[Then(@"they will be active")]
		public void ThenTheyWillBeActive()
		{
			foreach (var login in logins)
			{
				Assert.IsTrue(login.Active);
			}
		}

		[Then(@"they will not have sensible information")]
		public void ThenTheyWillNotHaveSensibleInformation()
		{
			foreach (var login in logins)
			{
				Assert.AreEqual(Defaults.TICKET_SHOWED_PART, login.Key.Length);
				Assert.AreEqual(0, login.ID);
				Assert.IsNull(login.User);
			}
		}
		#endregion

		#region ChangePassword
		[When(@"I try to change the password")]
		public void WhenITryToChangeThePassword()
		{
			try
			{
				var passwordForm = new PasswordForm(newPassword, retypePassword);
				Service.Safe.ChangePassword(currentPassword, passwordForm);
			}
			catch (DFMCoreException e)
			{
				Error = e;
			}
		}

		[Then(@"only the last ticket will be active")]
		public void ThenThereWillBeNoActiveLogins()
		{
			logins = Service.Safe.ListLogins();
			Assert.IsNotNull(logins);
			Assert.AreEqual(1, logins.Count);

			var safeTicketPart =
				TicketKey.Substring(0, Defaults.TICKET_SHOWED_PART);
            Assert.AreEqual(safeTicketPart, logins.First().Key);
		}
		#endregion ChangePassword

		#region UpdateEmail
		[Given(@"I pass this new e-mail and password")]
		public void GivenIPassThisNewEmailAndPassword(Table table)
		{
			currentPassword = table.Rows[0]["Current Password"];
			newEmail = table.Rows[0]["New E-mail"];
		}

		[When(@"I try to change the e-mail")]
		public void WhenITryToChangeTheEmail()
		{
			try
			{
				Service.Safe.UpdateEmail(currentPassword, newEmail);
			}
			catch (DFMCoreException e)
			{
				Error = e;
			}
		}

		[Then(@"the e-mail will not be changed")]
		public void ThenTheEmailWillNotBeChanged()
		{
			Error = null;
			var user = Service.Safe.GetUserByTicket(TicketKey);
			Assert.AreEqual(email, user.Email);
		}

		[Then(@"the e-mail will be changed")]
		public void ThenTheEmailWillBeChanged()
		{
			Error = null;
			var userEmail = DBHelper.GetUserEmailByTicket(TicketKey);
			Assert.AreEqual(newEmail, userEmail);

			//To next verification
			email = newEmail;
		}
		#endregion UpdateEmail



		#region MoreThanOne
		[Given(@"I have this user created")]
		public void GivenIHaveThisUserToCreate(Table table)
		{
			email = table.Rows[0]["Email"];
			password = table.Rows[0]["Password"];
			retypePassword = table.Rows[0]["Retype Password"];

			CreateUserIfNotExists(email, password);
		}

		[Given(@"I have this user created and activated")]
		public void GivenIHaveThisUserToCreateAndActivate(Table table)
		{
			email = table.Rows[0]["Email"];
			password = table.Rows[0]["Password"];

			CreateUserIfNotExists(email, password, true);
		}

		[Given(@"I have a token for its activation")]
		public void GivenIHaveATokenForItsActivation()
		{
			Service.Safe.SendUserVerify(email);
		}

		[Given(@"I have a token for its password reset")]
		public void GivenIHaveATokenForItsPasswordReset()
		{
			Service.Safe.SendPasswordReset(email);
		}

		[Given(@"I pass an invalid token")]
		public void GivenIPassAnInvalidToken()
		{
			token = TK.New();
		}

		[Given(@"I pass an e-mail that doesn't exist")]
		public void GivenIPassAnEMailThatDoesnTExist()
		{
			email = "dont_exist@dontflymoney.com";
		}

		[Given(@"I pass a ticket that doesn't exist")]
		public void GivenIPassATicketThatDoesnTExist()
		{
			ticket = TK.New();
		}

		[Given(@"I pass a null ticket")]
		public void GivenIPassANullTicket()
		{
			ticket = null;
		}

		[Given(@"I pass a ticket that is already disabled")]
		public void GivenIPassATicketThatIsAlreadyInvalid()
		{
			Service.Safe.DisableTicket(ticket);
		}

		[Given(@"I pass a ticket that is of this disabled user")]
		public void GivenIPassATicketThatIsOfThisDisabledUser()
		{
			ticket = Service.Safe.ValidateUserAndCreateTicket(
				email, password, TicketKey, TicketType.Local
			);
		}

		[Given(@"I pass a ticket that exist")]
		public void GivenIPassATicketThatExist()
		{
			ticket = DBHelper.GetLastTicketForUser(email);
		}

		[Given(@"I pass a valid ([A-Za-z]+) token")]
		public void GivenIPassTheValidToken(String strAction)
		{
			action = EnumX.Parse<SecurityAction>(strAction);
			token = DBHelper.GetLastTokenForUser(email, action);
		}

		[Given(@"I have a ticket of this user")]
		public void GivenIHaveATicketOfThisUser()
		{
			ticket = Service.Safe.ValidateUserAndCreateTicket(
				email, password, TicketKey, TicketType.Local
			);
		}


		[When(@"I try to send the e-mail of user verify")]
		public void WhenITryToSendTheEMailOfUserVerify()
		{
			try
			{
				Service.Safe.SendUserVerify(email);
			}
			catch (DFMCoreException e)
			{
				Error = e;
			}
		}


		[Then(@"I will receive no user")]
		public void ThenIWillReceiveNoUser()
		{
			Assert.IsNull(User);
		}

		[Then(@"I will receive the user")]
		public void ThenIWillReceiveTheUser()
		{
			Assert.IsNotNull(User);
			Assert.AreEqual(email, User.Email);
		}
		#endregion



		#region Contract
		[Given(@"I have a contract")]
		public void GivenIHaveAContract()
		{
			if (Service.Safe.GetContract() == null)
			{
				DBHelper.CreateContract("TestContract");
			}
		}

		[Given(@"I have accepted the contract")]
		public void GivenIHaveAcceptedTheContract()
		{
			Service.Safe.AcceptContract();
		}

		[Given(@"there is a new contract")]
		public void GivenICreateANewContract()
		{
			var scenarioName = ScenarioContext.Current.ScenarioInfo.Title;
            var contractVersion = scenarioName.Substring(0, 12);
            DBHelper.CreateContract(contractVersion);
		}

		[When(@"I try to get the contract")]
		public void WhenITryToGetTheContract()
		{
			try
			{
				Service.Safe.GetContract();
			}
			catch (DFMCoreException e)
			{
				Error = e;
			}
		}

		[When(@"I try to accept the contract")]
		public void WhenITryToAcceptTheContract()
		{
			try
			{
				Service.Safe.AcceptContract();
			}
			catch (DFMCoreException e)
			{
				Error = e;
			}
		}

		[When(@"I try to get the acceptance")]
		public void WhenITryToGetTheAcceptance()
		{
			try
			{
				accepted = Service.Safe.IsLastContractAccepted();
			}
			catch (DFMCoreException e)
			{
				Error = e;
			}
		}

		[Then(@"the contract status will be (not )?accepted")]
		public void ThenTheContractStatusWillBeAccepted(Boolean expectAccepted)
		{
			if (!accepted.HasValue)
			{
				accepted = Service.Safe.IsLastContractAccepted();
            }

			Assert.AreEqual(expectAccepted, accepted);
		}
		#endregion Contract



		#region TFA

		public class TFATable
		{
			public String Secret { get; set; }
			public String Code { get; set; }
			public String Password { get; set; }
		}

		public class UserTable
		{
			public String Email { get; set; }
			public String Password { get; set; }
		}

		[Given(@"I login this user")]
		public void GivenILoginThisUser(Table table)
		{
			var user = table.CreateInstance<UserTable>();

			ticket = Service.Safe.ValidateUserAndCreateTicket(
				user.Email, user.Password, TicketKey, TicketType.Local
			);
		}

		[Given(@"I have this two-factor data")]
		public void GivenIHaveThisTwoFactorData(Table table)
		{
			tfa = table.CreateInstance<TFATable>();

			if (tfa.Code == "{generated}")
			{
				tfa.Code = CodeGenerator.Generate(tfa.Secret);
			}
		}

		[Given(@"I set two-factor")]
		[When(@"I try to set two-factor")]
		public void WhenITryToSetTwoFactor()
		{
			try
			{
				Service.Safe.UpdateTFA(tfa.Secret, tfa.Code, tfa.Password);
			}
			catch (DFMCoreException e)
			{
				if (IsCurrent(ScenarioBlock.When))
					Error = e;
				else
					throw;
			}
		}

		[Given(@"I remove two-factor")]
		[When(@"I try to remove two-factor")]
		public void WhenITryToRemoveTwoFactor()
		{
			try
			{
				Service.Safe.RemoveTFA(tfa.Password);
			}
			catch (DFMCoreException e)
			{
				if (IsCurrent(ScenarioBlock.When))
					Error = e;
				else
					throw;
			}
		}

		[Then(@"the two-factor will be empty")]
		public void ThenTheTwoFactorWillStillBeEmpty()
		{
			var tfaSecret = DBHelper.GetTFAUser(Service.Current.User.Email);
			Assert.AreEqual(String.Empty, tfaSecret);
		}

		[Then(@"the two-factor will be \[(.*)\]")]
		public void ThenTheTwoFactorWillStillBe(String secret)
		{
			var tfaSecret = DBHelper.GetTFAUser(Service.Current.User.Email);
			Assert.AreEqual(secret, tfaSecret);
		}

		[Given(@"I have not valid ticket key")]
		public void GivenIHaveRandomTicketKey()
		{
			ticket = new Random().Next().ToString();
		}

		[Given(@"I have a ticket key")]
		public void GivenIHaveATicketKey()
		{
			ticket = Service.Safe.ValidateUserAndCreateTicket(
				email, password, TicketKey, TicketType.Local
			);
		}

		[Given(@"I validate the ticket two factor")]
		[When(@"I try to validate the ticket two factor")]
		public void ValidateTheTicketTwoFactor()
		{
			try
			{
				Service.Safe.ValidateTicketTFA(tfa.Code);
			}
			catch (DFMCoreException exception)
			{
				if (IsCurrent(ScenarioBlock.When))
					Error = exception;
				else
					throw;
			}
		}

		[When(@"I try to verify the ticket")]
		public void WhenITryToCheckTicket()
		{
			try
			{
				ticketVerified = Service.Safe.VerifyTicket();
			}
			catch (DFMCoreException exception)
			{
				Error = exception;
			}
		}

		[When(@"I try to verify the ticket type to be (\w+)")]
		public void WhenITryToCheckTicketType(TicketType type)
		{
			try
			{
				ticketVerified = Service.Safe.VerifyTicket(type);
			}
			catch (DFMCoreException exception)
			{
				Error = exception;
			}
		}

		[Then(@"the ticket will (not )?be verified")]
		public void ThenTheTicketWillBeVerified(Boolean verified)
		{
			var recordedVerified = ticketVerified == true;
			Assert.AreEqual(verified, recordedVerified);
		}

		[Then(@"the ticket will (not )?be valid")]
		public void ThenTheTicketWillBeValidated(Boolean valid)
		{
			var recordedValidated = Service.Safe.VerifyTicket();
			Assert.AreEqual(valid, recordedValidated);
		}
		#endregion TFA
	}
}
