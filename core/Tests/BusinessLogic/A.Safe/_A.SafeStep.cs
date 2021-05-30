﻿using System;
using System.Collections.Generic;
using System.Linq;
using DFM.BusinessLogic.Exceptions;
using DFM.BusinessLogic.Response;
using DFM.Entities;
using DFM.Entities.Bases;
using DFM.Entities.Enums;
using Keon.TwoFactorAuth;
using Keon.Util.Crypto;
using NUnit.Framework;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
using TK = Keon.Util.Extensions.Token;

namespace DFM.BusinessLogic.Tests.A.Safe
{
	[Binding]
	public class SafeStep : BaseStep
	{
		#region Variables
		private static String email
		{
			get => get<String>("Email");
			set => set("Email", value);
		}

		private static String newEmail
		{
			get => get<String>("NewEmail");
			set => set("NewEmail", value);
		}

		private static String ticket
		{
			get => get<String>("ticket");
			set => set("ticket", value);
		}

		private static String password
		{
			get => get<String>("Password");
			set => set("Password", value);
		}

		private static String retypePassword
		{
			get => get<String>("RetypePassword");
			set => set("RetypePassword", value);
		}

		private static String newPassword
		{
			get => get<String>("NewPassword");
			set => set("NewPassword", value);
		}

		private static String currentPassword
		{
			get => get<String>("CurrentPassword");
			set => set("CurrentPassword", value);
		}

		private static SecurityAction action
		{
			get => get<SecurityAction>("Action");
			set => set("Action", value);
		}

		private static IList<TicketInfo> logins
		{
			get => get<IList<TicketInfo>>("logins");
			set => set("logins", value);
		}

		private static Boolean? accepted
		{
			get => get<Boolean?>("accepted");
			set => set("accepted", value);
		}

		private static TFAInfo tfa
		{
			get => get<TFAInfo>("tfa");
			set => set("tfa", value);
		}

		private static Boolean? ticketVerified
		{
			get => get<Boolean?>("ticketVerified");
			set => set("ticketVerified", value);
		}
		#endregion


		#region SaveUserAndSendVerify
		[Given(@"I have this user data")]
		public void GivenIHaveThisUserData(Table table)
		{
			if (table.Header.Any(c => c == "Email"))
				email = table.Rows[0]["Email"]
					.Replace("{scenarioCode}", scenarioCode);

			if (table.Header.Any(c => c == "Password"))
				password = table.Rows[0]["Password"];

			if (table.Header.Any(c => c == "Retype Password"))
				retypePassword = table.Rows[0]["Retype Password"];
		}

		[When(@"I try to save the user")]
		public void WhenITryToSaveTheUser()
		{
			try
			{
				var info = new SignUpInfo
				{
					Email = email,
					Password = password,
					RetypePassword = retypePassword,
					Language = Defaults.ConfigLanguage,
				};

				service.Safe.SaveUser(info);
			}
			catch (CoreError e)
			{
				error = e;
			}
		}

		[Then(@"the user will not be saved")]
		public void ThenTheUserWillNotBeSaved()
		{
			var user = repos.User.GetByEmail(email);
			Assert.IsNull(user);
		}

		[Then(@"the user will not be changed")]
		public void ThenTheUserWillNotBeChanged()
		{
			var savedUser = repos.User.GetByEmail(email);
			Assert.IsNotNull(savedUser);

			var rightPassword = Crypt.Check(password, savedUser.Password);
			Assert.IsTrue(rightPassword);
		}

		[Then(@"the user will be saved")]
		public void ThenTheUserWillBeSaved()
		{
			var tokenActivate = getLastTokenForUser(
				email,
				SecurityAction.UserVerification
			);

			service.Safe.ActivateUser(tokenActivate);

			var savedUser = repos.User.GetByEmail(email);
			Assert.IsNotNull(savedUser);

			var rightPassword = Crypt.Check(password, savedUser.Password);
			Assert.IsTrue(rightPassword);
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
				service.Safe.SendPasswordReset(email);
			}
			catch (CoreError e)
			{
				error = e;
			}
		}

		[Then(@"there will be no active logins")]
		public void ThenThereWillBeNoActiveLogins()
		{
			var user = repos.User.GetByEmail(email);
			var loginForUser = repos.Ticket.List(user).ToList();

			Assert.LessOrEqual(0, loginForUser.Count);
		}
		#endregion

		#region ActivateUser
		[Given(@"I pass a token of password reset")]
		public void GivenIPassATokenOfPasswordReset()
		{
			service.Safe.SendPasswordReset(current.Email);

			token = getLastTokenForUser(
				current.Email,
				SecurityAction.PasswordReset
			);
		}

		[When(@"I try to activate the user")]
		public void WhenITryToActivateTheUser()
		{
			try
			{
				service.Safe.ActivateUser(token);
			}
			catch (CoreError e)
			{
				error = e;
			}
		}

		[Then(@"the user will (not )?be activated")]
		public void ThenTheUserWillNotBeActivated(Boolean active)
		{
			var user = repos.User.GetByEmail(email);
			Assert.AreEqual(active, user.Control.Active);
		}
		#endregion

		#region ValidateUserAndGetTicket
		[Given(@"I activate the user")]
		public void GivenIActivateTheUser()
		{
			service.Safe.SendUserVerify(email);

			var tokenToActivate = getLastTokenForUser(
				email,
				SecurityAction.UserVerification
			);

			service.Safe.ActivateUser(tokenToActivate);
		}

		[Given(@"I deactivate the user")]
		public void GivenIDeactivateTheUser()
		{
			var user = repos.User.GetByEmail(email);
			db.Execute(() => repos.Control.Deactivate(user));
		}

		[Given(@"I deactivate the user (.+)")]
		public void GivenIDeactivateTheUser(String email)
		{
			var user = repos.User.GetByEmail(email);
			db.Execute(() => repos.Control.Deactivate(user));
		}

		[When(@"I try to get the ticket")]
		public void WhenITryToGetTheTicket()
		{
			ticket = null;
			error = null;

			try
			{
				var info = new SignInInfo
				{
					Email = email,
					Password = password,
					TicketKey = ticketKey,
					TicketType = TicketType.Local,
				};

				ticket = service.Safe.CreateTicket(info);
			}
			catch (CoreError e)
			{
				error = e;
			}
		}

		[Given(@"I tried to get the ticket (\d+) times")]
		[When(@"I try to get the ticket (\d+) times")]
		public void WhenITryToGetTheTicketSomeTimes(Int32 times)
		{
			ticket = null;

			var info = new SignInInfo
			{
				Email = email,
				Password = password,
				TicketKey = ticketKey,
				TicketType = TicketType.Local
			};

			for (var t = 1; t < times; t++)
			{
				try
				{
					service.Safe.CreateTicket(info);
				}
				catch (CoreError) { }
			}

			try
			{
				service.Safe.CreateTicket(info);
			}
			catch (CoreError e)
			{
				error = e;
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

			var expectedSession = service.Safe.GetSession(ticket);

			Assert.IsNotNull(expectedSession);
		}
		#endregion

		#region GetUserByTicket
		[When(@"I try to get the session")]
		public void WhenITryToGetTheUser()
		{
			session = null;

			try
			{
				session = service.Safe.GetSession(ticket);
			}
			catch (CoreError e)
			{
				error = e;
			}
		}
		#endregion

		#region PasswordReset
		[Given(@"I pass a token of user verification")]
		public void GivenIPassATokenOfUserVerification()
		{
			token = getLastTokenForUser(
				current.Email,
				SecurityAction.UserVerification
			);
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
				var info = new PasswordResetInfo
				{
					Password = newPassword,
					RetypePassword = retypePassword,
					Token = token
				};

				service.Safe.ResetPassword(info);
			}
			catch (CoreError e)
			{
				error = e;
			}
		}

		[Then(@"the password will not be changed")]
		public void ThenThePasswordWillNotBeChanged()
		{
			var savedUser = repos.User.GetByEmail(email);
			Assert.IsNotNull(savedUser);

			var rightPassword = Crypt.Check(password, savedUser.Password);
			Assert.IsTrue(rightPassword);
		}

		[Then(@"the password will be changed")]
		public void ThenThePasswordWillBeChanged()
		{
			var savedUser = repos.User.GetByEmail(email);
			Assert.IsNotNull(savedUser);

			var rightPassword = Crypt.Check(newPassword, savedUser.Password);
			Assert.IsTrue(rightPassword);
		}
		#endregion

		#region TestSecurityToken
		[Given(@"I pass a token of ([A-Za-z]+) with action ([A-Za-z]+)")]
		public void GivenIPassATokenOfUserVerificationWithActionPasswordReset(SecurityAction tokenOf, SecurityAction actionOf)
		{
			token = getLastTokenForUser(email, tokenOf);
			action = actionOf;
		}

		[When(@"I test the token")]
		public void WhenITestTheToken()
		{
			try
			{
				service.Safe.TestSecurityToken(token, action);
			}
			catch (CoreError e)
			{
				error = e;
			}
		}
		#endregion

		#region DisableToken
		[When(@"I try do disable the token")]
		public void WhenITryDoDisableTheToken()
		{
			try
			{
				service.Safe.DisableToken(token);
			}
			catch (CoreError e)
			{
				error = e;
			}
		}

		[Then(@"the token will not be valid anymore")]
		public void ThenTheTokenWillNotBeValidAnymore()
		{
			try
			{
				service.Safe.TestSecurityToken(token, action);
			}
			catch(CoreError e)
			{
				error = e;
			}

			Assert.IsNotNull(error);
			Assert.AreEqual(Error.InvalidToken, error.Type);
		}
		#endregion

		#region Disable Ticket
		[When(@"I try to disable the ticket")]
		public void WhenITryToDisableTheTicket()
		{
			try
			{
				service.Safe.DisableTicket(ticket);
			}
			catch (CoreError e)
			{
				error = e;
			}
		}

		[Then(@"the ticket will not be valid anymore")]
		public void ThenTheTicketWillNotBeValidAnymore()
		{
			error = null;

			try
			{
				service.Safe.GetSession(ticket);
			}
			catch (CoreError e)
			{
				error = e;
			}

			Assert.IsNotNull(error);
			Assert.AreEqual(Error.Uninvited, error.Type);
		}

		[Then(@"the ticket will still be valid")]
		public void ThenTheTicketWillStillBeValid()
		{
			error = null;

			try
			{
				service.Safe.GetSession(ticket);
			}
			catch (CoreError e)
			{
				error = e;
			}

			Assert.IsNull(error);
		}
		#endregion

		#region ListLogins
		[Given(@"I login the user")]
		public void GivenILoginTheUser()
		{
			current.Set(userEmail, userPassword, false);
		}

		[Given(@"I logoff the user")]
		public void GivenILogoffTheUser()
		{
			current.Clear();
		}

		[When(@"I ask for current active logins")]
		public void WhenIAskForCurrentActiveLogins()
		{
			try
			{
				logins = service.Safe.ListLogins();
			}
			catch (CoreError e)
			{
				error = e;
			}
		}

		[Then(@"they will be active")]
		public void ThenTheyWillBeActive()
		{
			var currentTicket = repos.Ticket.GetByKey(
				service.Current.TicketKey
			);
			var user = currentTicket.User;

			foreach (var login in logins)
			{
				var loginDb = repos.Ticket.GetByPartOfKey(
					user, login.Key
				);

				Assert.IsTrue(loginDb.Active);
			}
		}

		[Then(@"they will not have sensible information")]
		public void ThenTheyWillNotHaveSensibleInformation()
		{
			foreach (var login in logins)
			{
				Assert.AreEqual(Defaults.TicketShowedPart, login.Key.Length);
			}
		}
		#endregion

		#region ChangePassword
		[When(@"I try to change the password")]
		public void WhenITryToChangeThePassword()
		{
			try
			{
				var info = new ChangePasswordInfo
				{
					CurrentPassword = currentPassword,
					Password = newPassword,
					RetypePassword = retypePassword
				};

				service.Safe.ChangePassword(info);
			}
			catch (CoreError e)
			{
				error = e;
			}
		}

		[Then(@"only the last login will be active")]
		public void ThenOnlyTheLastLoginWillBeActive()
		{
			var user = repos.User.GetByEmail(email);
			var loginForUser = repos.Ticket.List(user).ToList();

			Assert.AreEqual(1, loginForUser.Count);
			Assert.AreEqual(ticketKey, loginForUser[0].Key);
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
				service.Safe.UpdateEmail(currentPassword, newEmail);
			}
			catch (CoreError e)
			{
				error = e;
			}
		}

		[Then(@"the e-mail will not be changed")]
		public void ThenTheEmailWillNotBeChanged()
		{
			error = null;
			var user = service.Safe.GetSession(ticketKey);
			Assert.AreEqual(email, user.Email);
		}

		[Then(@"the e-mail will be changed")]
		public void ThenTheEmailWillBeChanged()
		{
			error = null;

			var actualTicket = repos.Ticket.GetByKey(ticketKey);
			var actualEmail = actualTicket?.User?.Email;
			Assert.AreEqual(newEmail, actualEmail);

			//To next verification
			email = newEmail;
		}
		#endregion UpdateEmail

		#region SaveAccess
		[When(@"I try to save the access")]
		public void WhenITryToSaveTheAccess()
		{
			try
			{
				service.Safe.SaveAccess();
			}
			catch (CoreError e)
			{
				error = e;
			}
		}

		[Then(@"the ticket access will( not)? be after test start time")]
		public void ThenTheTicketAccessWillBeAfterTestStartTime(Boolean after)
		{
			var ticketList = repos.Ticket.NewQuery()
				.Where(t => t.User, u => u.Email == userEmail)
				.List;

			Assert.AreNotEqual(0, ticketList.Count, $"no login for {userEmail}");

			var updated = ticketList.Count(
				t => t.LastAccess.ToUniversalTime() > testStart
			);

			var expectedUpdated = after ? 1 : 0;
			Assert.AreEqual(expectedUpdated, updated);
		}

		[Then(@"the user access will( not)? be null")]
		public void ThenTheUserAccessWillBeNull(Boolean isNull)
		{
			var user = repos.User.GetByEmail(userEmail);
			var lastAccess = user.Control.LastAccess?.ToUniversalTime();

			if (isNull)
				Assert.IsNull(lastAccess);
			else
				Assert.IsNotNull(lastAccess);
		}
		#endregion

		#region UseTFAAsPassword
		[When(@"I set to (not )?use TFA as password")]
		public void WhenISetToUseTFAAsPassword(Boolean use)
		{
			try
			{
				service.Safe.UseTFAAsPassword(use);
			}
			catch (CoreError e)
			{
				error = e;
			}
		}

		[Then(@"the TFA can (not )?be used as password")]
		public void ThenTheTFACanBeUsedAsPassword(Boolean canBe)
		{
			var worked = true;
			var code = CodeGenerator.Generate(tfa.Secret);

			try
			{
				var info = new SignInInfo
				{
					Email = email,
					Password = code,
					TicketKey = ticket,
					TicketType = TicketType.Local
				};

				service.Safe.CreateTicket(info);
			}
			catch (CoreError)
			{
				worked = false;
			}

			Assert.AreEqual(canBe, worked);
		}

		[Then(@"the TFA will (not )?be asked")]
		public void ThenTheTFAWillNotBeAsked(Boolean askTFA)
		{
			var currentTicket = repos.Ticket.GetByKey(ticket);
			Assert.AreEqual(askTFA, !currentTicket.ValidTFA);
		}

		[Then(@"I can still login using normal password")]
		public void ThenICanStillLoginUsingNormalPassword()
		{
			ticket = TK.New();

			var info = new SignInInfo
			{
				Email = email,
				Password = password,
				TicketKey = ticket,
				TicketType = TicketType.Local,
			};

			ticket = service.Safe.CreateTicket(info);
		}
		#endregion

		#region MoreThanOne
		[Given(@"I have (?:this user|these users) created")]
		public void GivenIHaveThisUserCreated(Table table)
		{
			foreach (var userData in table.Rows)
			{
				var email = userData["Email"]
					.Replace("{scenarioCode}", scenarioCode);

				var password = userData.ContainsKey("Password")
					? userData["Password"]
					: userPassword;

				var active = userData.ContainsKey("Active")
				    && userData["Active"] == "true";

				var signed = userData.ContainsKey("Signed")
				    && userData["Signed"] == "true";

				var timezone = userData.ContainsKey("Timezone")
					? Int32.Parse(userData["Timezone"])
					: default(Int32?);

				createUserIfNotExists(email, password, active, signed, timezone);
			}
		}

		[Given(@"I have a token for its activation")]
		public void GivenIHaveATokenForItsActivation()
		{
			service.Safe.SendUserVerify(email);
		}

		[Given(@"I have a token for its password reset")]
		public void GivenIHaveATokenForItsPasswordReset()
		{
			service.Safe.SendPasswordReset(email);
		}

		[Given(@"I pass an invalid token")]
		public void GivenIPassAnInvalidToken()
		{
			token = TK.New();
		}

		[Given(@"I pass an expired (UserVerification|PasswordReset|UnsubscribeMoveMail) token")]
		public void GivenIPassExpiredToken(SecurityAction action)
		{
			GivenIPassTheValidToken(action);
			var security = repos.Security.GetByToken(token);
			security.Expire = DateTime.Today.AddDays(-2);
			repos.Security.SaveOrUpdate(security);
		}

		[Given(@"I pass an inactive (UserVerification|PasswordReset|UnsubscribeMoveMail) token")]
		public void GivenIPassInactiveToken(SecurityAction action)
		{
			GivenIPassTheValidToken(action);
			var security = repos.Security.GetByToken(token);
			security.Active = false;
			repos.Security.SaveOrUpdate(security);
		}

		[Given(@"I pass an e-mail that doesn't exist")]
		public void GivenIPassAnEMailThatDoesNotExist()
		{
			email = "dont_exist@dontflymoney.com";
		}

		[Given(@"I pass a ticket that doesn't exist")]
		public void GivenIPassATicketThatDoesNotExist()
		{
			ticket = TK.New();
		}

		[Given(@"I pass a null ticket")]
		public void GivenIPassANullTicket()
		{
			ticket = null;
		}

		[Given(@"I pass an empty ticket")]
		public void GivenIPassAnEmptyTicket()
		{
			ticket = "";
		}

		[Given(@"I pass a ticket that is already disabled")]
		public void GivenIPassATicketThatIsAlreadyInvalid()
		{
			service.Safe.DisableTicket(ticket);
		}

		[Given(@"I pass a ticket that is of this disabled user")]
		public void GivenIPassATicketThatIsOfThisDisabledUser()
		{
			var info = new SignInInfo
			{
				Email = email,
				Password = password,
				TicketKey = ticketKey,
				TicketType = TicketType.Local,
			};

			ticket = service.Safe.CreateTicket(info);
		}

		[Given(@"I pass a ticket that exist")]
		public void GivenIPassATicketThatExist()
		{
			var userID = repos.User.GetByEmail(email).ID;

			ticket = repos.Ticket.Where(
				t => t.User.ID == userID
					&& t.Expiration == null
			).FirstOrDefault()?.Key;
		}

		[Given(@"I pass a valid (UserVerification|PasswordReset|UnsubscribeMoveMail) token")]
		public void GivenIPassTheValidToken(SecurityAction actionOf)
		{
			action = actionOf;

			var tokenEmail = email ?? userEmail;

			if (actionOf == SecurityAction.UnsubscribeMoveMail)
			{
				var user = repos.User.GetByEmail(tokenEmail);
				var security = repos.Security.Grab(
					user, SecurityAction.UnsubscribeMoveMail
				);
				token = security.Token;
			}
			else
			{
				token = getLastTokenForUser(tokenEmail, action);
			}
		}

		[Given(@"I have a ticket of this user")]
		public void GivenIHaveATicketOfThisUser()
		{
			var info = new SignInInfo
			{
				Email = email,
				Password = password,
				TicketKey = ticketKey,
				TicketType = TicketType.Local,
			};

			ticket = service.Safe.CreateTicket(info);
		}


		[When(@"I try to send the e-mail of user verify")]
		public void WhenITryToSendTheEMailOfUserVerify()
		{
			try
			{
				service.Safe.SendUserVerify(email);
			}
			catch (CoreError e)
			{
				error = e;
			}
		}


		[Then(@"I will receive no session")]
		public void ThenIWillReceiveNoSession()
		{
			Assert.IsNull(session);
		}

		[Then(@"I will receive the session")]
		public void ThenIWillReceiveTheSession()
		{
			Assert.IsNotNull(session);
			Assert.AreEqual(email, session.Email);
		}

		[Then(@"the TFA will (not )?be enabled")]
		public void ThenTheTFAWill_BeEnabled(Boolean enabled)
		{
			Assert.AreEqual(enabled, session.HasTFA);
		}
		#endregion



		#region Contract
		[Given(@"I have a contract")]
		public void GivenIHaveAContract()
		{
			if (service.Safe.GetContract() == null)
			{
				var contract = new Contract
				{
					BeginDate = DateTime.UtcNow,
					Version = "TestContract",
				};

				repos.Contract.SaveOrUpdate(contract);
			}
		}

		[Given(@"I have accepted the contract")]
		public void GivenIHaveAcceptedTheContract()
		{
			service.Safe.AcceptContract();
		}

		[Given(@"there is a new contract")]
		public void GivenICreateANewContract()
		{
            var contract = new Contract
            {
	            BeginDate = current.Now,
	            Version = scenarioCode,
            };

            repos.Contract.SaveOrUpdate(contract);
		}

		[When(@"I try to get the contract")]
		public void WhenITryToGetTheContract()
		{
			try
			{
				service.Safe.GetContract();
			}
			catch (CoreError e)
			{
				error = e;
			}
		}

		[When(@"I try to accept the contract")]
		public void WhenITryToAcceptTheContract()
		{
			try
			{
				service.Safe.AcceptContract();
			}
			catch (CoreError e)
			{
				error = e;
			}
		}

		[When(@"I try to get the acceptance")]
		public void WhenITryToGetTheAcceptance()
		{
			try
			{
				accepted = service.Safe.IsLastContractAccepted();
			}
			catch (CoreError e)
			{
				error = e;
			}
		}

		[Then(@"the contract status will be (not )?accepted")]
		public void ThenTheContractStatusWillBeAccepted(Boolean expectAccepted)
		{
			accepted ??= service.Safe.IsLastContractAccepted();

			Assert.AreEqual(expectAccepted, accepted);
		}
		#endregion Contract



		#region TFA
		public class UserTable
		{
			public String Email { get; set; }
			public String Password { get; set; }
		}

		[Given(@"I login this user")]
		public void GivenILoginThisUser(Table table)
		{
			var userTable = table.CreateInstance<UserTable>();
			userTable.Email = userTable.Email
				.Replace("{scenarioCode}", scenarioCode);

			var user = repos.User.GetByEmail(userTable.Email);

			if (user.Control is {Active: false})
				repos.Control.Activate(user);

			var info = new SignInInfo
			{
				Email = userTable.Email.Replace(
					"{scenarioCode}", scenarioCode
				),
				Password = userTable.Password,
				TicketKey = ticketKey,
				TicketType = TicketType.Local,
			};

			ticket = service.Safe.CreateTicket(info);
		}

		[Given(@"I have this two-factor data")]
		public void GivenIHaveThisTwoFactorData(Table table)
		{
			tfa = table.CreateInstance<TFAInfo>();

			if (tfa.Code == "{generated}")
			{
				tfa.Code = CodeGenerator.Generate(tfa.Secret);
			}

			if (tfa.Password == "{null}")
			{
				tfa.Password = null;
			}
		}

		[Given(@"I set two-factor")]
		[When(@"I try to set two-factor")]
		public void WhenITryToSetTwoFactor()
		{
			try
			{
				service.Safe.UpdateTFA(tfa);
			}
			catch (CoreError e)
			{
				if (isCurrent(ScenarioBlock.When))
					error = e;
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
				service.Safe.RemoveTFA(tfa.Password);
			}
			catch (CoreError e)
			{
				if (isCurrent(ScenarioBlock.When))
					error = e;
				else
					throw;
			}
		}

		[Then(@"the two-factor will be empty")]
		public void ThenTheTwoFactorWillStillBeEmpty()
		{
			var user = repos.User.GetByEmail(current.Email);
			Assert.IsNull(user?.TFASecret);
		}

		[Then(@"the two-factor will be \[(.*)\]")]
		public void ThenTheTwoFactorWillStillBe(String secret)
		{
			var user = repos.User.GetByEmail(current.Email);
			Assert.AreEqual(secret, user.TFASecret);
		}

		[Given(@"I have not valid ticket key")]
		public void GivenIHaveRandomTicketKey()
		{
			ticket = new Random().Next().ToString();
		}

		[Given(@"I have a ticket key")]
		public void GivenIHaveATicketKey()
		{
			var info = new SignInInfo
			{
				Email = email,
				Password = password,
				TicketKey = ticketKey,
				TicketType = TicketType.Local,
			};

			ticket = service.Safe.CreateTicket(info);
		}

		[Given(@"I validate the ticket two factor")]
		[When(@"I try to validate the ticket two factor")]
		public void ValidateTheTicketTwoFactor()
		{
			try
			{
				service.Safe.ValidateTicketTFA(tfa.Code);
			}
			catch (CoreError exception)
			{
				if (isCurrent(ScenarioBlock.When))
					error = exception;
				else
					throw;
			}
		}

		[When(@"I try to verify the ticket")]
		public void WhenITryToCheckTicket()
		{
			try
			{
				ticketVerified = service.Safe.VerifyTicketTFA();
			}
			catch (CoreError exception)
			{
				error = exception;
			}
		}

		[When(@"I try to verify the ticket type to be (\w+)")]
		public void WhenITryToCheckTicketType(TicketType type)
		{
			try
			{
				ticketVerified = service.Safe.VerifyTicketType(type);
			}
			catch (CoreError exception)
			{
				error = exception;
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
			var recordedValidated = service.Safe.VerifyTicketTFA();
			Assert.AreEqual(valid, recordedValidated);
		}
		#endregion TFA
	}
}
