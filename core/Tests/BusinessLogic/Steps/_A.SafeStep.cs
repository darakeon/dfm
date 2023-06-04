using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DFM.BusinessLogic.Exceptions;
using DFM.BusinessLogic.Response;
using DFM.Entities;
using DFM.Entities.Bases;
using DFM.Entities.Enums;
using Keon.Eml;
using Keon.TwoFactorAuth;
using Keon.Util.Crypto;
using NUnit.Framework;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
using TK = Keon.Util.Extensions.Token;

namespace DFM.BusinessLogic.Tests.Steps
{
	[Binding]
	public class SafeStep : BaseStep
	{
		public SafeStep(ScenarioContext context)
			: base(context) { }

		#region Variables
		private String newEmail
		{
			get => get<String>("NewEmail");
			set => set("NewEmail", value);
		}

		private String ticket
		{
			get => get<String>("ticket");
			set => set("ticket", value);
		}

		private String password
		{
			get => get<String>("Password");
			set => set("Password", value);
		}

		private String retypePassword
		{
			get => get<String>("RetypePassword");
			set => set("RetypePassword", value);
		}

		private String newPassword
		{
			get => get<String>("NewPassword");
			set => set("NewPassword", value);
		}

		private String currentPassword
		{
			get => get<String>("CurrentPassword");
			set => set("CurrentPassword", value);
		}

		private SecurityAction action
		{
			get => get<SecurityAction>("Action");
			set => set("Action", value);
		}

		private IList<TicketInfo> logins
		{
			get => get<IList<TicketInfo>>("logins");
			set => set("logins", value);
		}

		private Boolean? accepted
		{
			get => get<Boolean?>("accepted");
			set => set("accepted", value);
		}

		private TFAInfo tfa
		{
			get => get<TFAInfo>("tfa");
			set => set("tfa", value);
		}

		private Boolean? ticketVerified
		{
			get => get<Boolean?>("ticketVerified");
			set => set("ticketVerified", value);
		}

		private Misc misc
		{
			get => get<Misc>("Misc");
			set => set("Misc", value);
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
					Language = Defaults.SettingsLanguage,
				};

				service.Auth.SaveUser(info);
			}
			catch (CoreError e)
			{
				error = e;
			}
		}

		[Then(@"the user will not be changed")]
		public void ThenTheUserWillNotBeChanged()
		{
			var savedUser = repos.User.GetByEmail(email);
			Assert.IsNotNull(savedUser);

			var rightPassword = Crypt.Check(password, savedUser.Password);
			Assert.IsTrue(rightPassword);
		}

		[Then(@"the user will (not )?be saved")]
		public void ThenTheUserWillBeSaved(Boolean saved)
		{
			var savedUser = repos.User.GetByEmail(email);

			if (saved)
			{
				Assert.IsNotNull(savedUser);

				var rightPassword = Crypt.Check(password, savedUser.Password);
				Assert.IsTrue(rightPassword);
			}
			else
			{
				Assert.IsNull(savedUser);
			}
		}

		[Then(@"it will have a misc")]
		public void ThenItWillHaveAMisc()
		{
			var savedUser = repos.User.GetByEmail(email);
			Assert.NotNull(savedUser);
			Assert.NotZero(savedUser.Control.MiscDna);
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
				service.Outside.SendPasswordReset(email);
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
			service.Outside.SendPasswordReset(current.Email);

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
				service.Outside.ActivateUser(token);
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
			service.Outside.SendUserVerify(email);

			var tokenToActivate = getLastTokenForUser(
				email,
				SecurityAction.UserVerification
			);

			service.Outside.ActivateUser(tokenToActivate);
		}

		[Given(@"I deactivate the user")]
		public void GivenIDeactivateTheUser()
		{
			var user = repos.User.GetByEmail(email ?? userEmail);
			db.Execute(() => repos.Control.Deactivate(user));
		}

		[Given(@"I deactivate the user (.+)")]
		public void GivenIDeactivateTheUser(String email)
		{
			email = email.Replace("{scenarioCode}", scenarioCode);
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
					TicketType = TicketType.Tests,
				};

				ticket = service.Auth.CreateTicket(info);
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
				TicketType = TicketType.Tests
			};

			for (var t = 1; t < times; t++)
			{
				try
				{
					service.Auth.CreateTicket(info);
				}
				catch (CoreError) { }
			}

			try
			{
				service.Auth.CreateTicket(info);
			}
			catch (CoreError e)
			{
				error = e;
			}
		}

		[Then(@"I will (not )?receive the ticket")]
		public void ThenIWillReceiveTheTicket(Boolean receive)
		{
			if (receive)
			{
				Assert.IsNotNull(ticket);

				var expectedSession = service.Auth.GetSession(ticket);

				Assert.IsNotNull(expectedSession);
			}
			else
			{
				Assert.IsNull(ticket);
			}
		}
		#endregion

		#region GetUserByTicket
		[When(@"I try to get the session")]
		public void WhenITryToGetTheUser()
		{
			session = null;

			try
			{
				session = service.Auth.GetSession(ticket);
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

				service.Outside.ResetPassword(info);
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
				service.Outside.TestSecurityToken(token, action);
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
				service.Outside.DisableToken(token);
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
				service.Outside.TestSecurityToken(token, action);
			}
			catch (CoreError e)
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
				service.Auth.DisableTicket(ticket);
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
				service.Auth.GetSession(ticket);
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
				service.Auth.GetSession(ticket);
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

		[Given(@"I login the user again")]
		public void GivenILoginTheUserAgain()
		{
			resetTicket();
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
				logins = service.Auth.ListLogins();
			}
			catch (CoreError e)
			{
				error = e;
			}
		}

		[Then(@"there will be (\d+) logins")]
		public void ThenThereWillBeCountLogins(Int32 count)
		{
			Assert.AreEqual(count, logins.Count);
		}

		[Then(@"they will be active")]
		public void ThenTheyWillBeActive()
		{
			var user = service.Auth.GetCurrent();

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

		[Then(@"the current login and only it will have current flag")]
		public void ThenTheCurrentLoginAndOnlyItWillHaveCurrentFlag()
		{
			var user = service.Auth.GetCurrent();

			foreach (var login in logins)
			{
				var loginDb = repos.Ticket
					.GetByPartOfKey(user, login.Key);

				if (loginDb.Key == service.Current.TicketKey)
					Assert.IsTrue(login.Current);
				else
					Assert.IsFalse(login.Current);
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

				service.Auth.ChangePassword(info);
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

			newEmail = table.Rows[0]["New E-mail"]
				.Replace("{scenarioCode}", scenarioCode);
		}

		[When(@"I try to change the e-mail")]
		public void WhenITryToChangeTheEmail()
		{
			try
			{
				service.Auth.UpdateEmail(currentPassword, newEmail);
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
			var user = service.Auth.GetSession(ticketKey);
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
				service.Law.SaveAccess();
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
				.Where(
					t => t.User,
					User.Compare(userEmail)
				)
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
				service.Auth.UseTFAAsPassword(use);
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
					TicketType = TicketType.Tests
				};

				service.Auth.CreateTicket(info);
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
				TicketType = TicketType.Tests,
			};

			ticket = service.Auth.CreateTicket(info);
		}
		#endregion

		#region SendWipedUserCSV

		[Given(@"robot call wipe users")]
		public void WhenRobotWipeUsers()
		{
			robotRunWipe();
		}

		[When(@"ask wiped user csv")]
		public void WhenAskWipedUserCsv(Table table)
		{
			var row = table.Rows[0];
			var email = row["Email"]
				.Replace("{scenarioCode}", scenarioCode);
			var password = row["Password"];

			try
			{
				service.Outside.SendWipedUserCSV(email, password);
			}
			catch (CoreError e)
			{
				error = e;
			}
		}

		[Then(@"(\d+ )?emails? with csv will (not )?be sent")]
		public void ThenCSVWill_BeSent(Int32 emailCount, Boolean csvSent)
		{
			var inboxPath = Path.Combine(
				"..", "..", "..", "..", "..", "..", "inbox"
			);
			var inbox = new DirectoryInfo(inboxPath);

			var emails = inbox.GetFiles("*.eml")
				.Where(f => f.CreationTimeUtc >= testStart)
				.Select(f => EmlReader.FromFile(f.FullName))
				.Where(e => e.Headers["To"] == userEmail)
				.Where(e => e.Subject == "Dados exportados")
				.ToList();

			if (csvSent)
			{
				Assert.AreEqual(emailCount, emails.Count);

				foreach (var email in emails)
				{
					var countAttachments = email.Attachments.Count;

					Assert.AreEqual(1, countAttachments);

					var link = $"https://dontflymoney.com/>DeleteCsvData>{token}";

					Assert.That(
						email.Body.Contains(link),
						() => $"{link}\nnot found at\n{email.Body}"
					);
				}
			}
			else
			{
				Assert.AreEqual(0, emails.Count);
			}
		}

		#endregion SendWipedUserCSV

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

				var days = userData.ContainsKey("Days")
					? Int32.Parse(userData["Days"])
					: default(Int32?);

				createUserIfNotExists(email, password, active, signed, timezone, days);
			}
		}

		[Given(@"I have a token for its activation")]
		public void GivenIHaveATokenForItsActivation()
		{
			service.Outside.SendUserVerify(email);
		}

		[Given(@"I have a token for its password reset")]
		public void GivenIHaveATokenForItsPasswordReset()
		{
			service.Outside.SendPasswordReset(email);
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
			service.Auth.DisableTicket(ticket);
		}

		[Given(@"I pass a ticket that is of this disabled user")]
		public void GivenIPassATicketThatIsOfThisDisabledUser()
		{
			var info = new SignInInfo
			{
				Email = email,
				Password = password,
				TicketKey = ticketKey,
				TicketType = TicketType.Tests,
			};

			ticket = service.Auth.CreateTicket(info);
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
				TicketType = TicketType.Tests,
			};

			ticket = service.Auth.CreateTicket(info);
		}

		[Given(@"the token expires")]
		public void GivenTheTokenExpires()
		{
			var security = repos.Security.GetByToken(token);
			security.Expire = DateTime.Now.AddDays(-1);
			repos.Security.SaveOrUpdate(security);
		}

		[When(@"I try to send the e-mail of user verify")]
		public void WhenITryToSendTheEMailOfUserVerify()
		{
			try
			{
				service.Outside.SendUserVerify(email);
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
			var version = $"Contract {scenarioCode}";
			var contract = repos.Contract.GetContract();

			if (contract?.Version == version)
				return;

			contract = new Contract
			{
				BeginDate = DateTime.UtcNow,
				Version = version,
			};

			repos.Contract.SaveOrUpdate(contract);
		}

		[Given(@"I have accepted the contract")]
		public void GivenIHaveAcceptedTheContract()
		{
			service.Law.AcceptContract();
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
				service.Law.GetContract();
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
				service.Law.AcceptContract();
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
				accepted = service.Law.IsLastContractAccepted();
			}
			catch (CoreError e)
			{
				error = e;
			}
		}

		[Then(@"the contract status will be (not )?accepted")]
		public void ThenTheContractStatusWillBeAccepted(Boolean expectAccepted)
		{
			accepted ??= service.Law.IsLastContractAccepted();

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

			if (user.Control is { Active: false })
				repos.Control.Activate(user);

			var info = new SignInInfo
			{
				Email = userTable.Email.Replace(
					"{scenarioCode}", scenarioCode
				),
				Password = userTable.Password,
				TicketKey = ticketKey,
				TicketType = TicketType.Tests,
			};

			ticket = service.Auth.CreateTicket(info);
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
				service.Auth.UpdateTFA(tfa);
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
				service.Auth.RemoveTFA(tfa.Password);
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
				TicketType = TicketType.Tests,
			};

			ticket = service.Auth.CreateTicket(info);
		}

		[Given(@"I validate the ticket two factor")]
		[When(@"I try to validate the ticket two factor")]
		public void ValidateTheTicketTwoFactor()
		{
			try
			{
				service.Auth.ValidateTicketTFA(tfa.Code);
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
				ticketVerified = service.Auth.VerifyTicketTFA();
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
				ticketVerified = service.Auth.VerifyTicketType(type);
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
			var recordedValidated = service.Auth.VerifyTicketTFA();
			Assert.AreEqual(valid, recordedValidated);
		}
		#endregion TFA

		#region AskWipe
		[Given(@"the user not active after (\d+) days")]
		public void GivenTheUserNotActiveAfter(Int32 days)
		{
			var user = repos.User.GetByEmail(userEmail);
			db.Execute(() => repos.Control.Deactivate(user));

			user.Control.Creation = DateTime.UtcNow.AddDays(-days);
			db.Execute(() => repos.Control.SaveOrUpdate(user.Control));
		}

		[Given(@"data wipe was asked")]
		public void GivenDataWipeWasAsked()
		{
			var user = repos.User.GetByEmail(userEmail);
			db.Execute(() => repos.Control.RequestWipe(user));
		}

		[When(@"pass a password that is( not)? right")]
		public void PassAPasswordThatIs(Boolean rightPassword)
		{
			password = rightPassword ? userPassword : "wrong";
		}

		[When(@"ask data wipe")]
		public void WhenAskDataWipe()
		{
			try
			{
				service.Robot.AskWipe(password);
			}
			catch (CoreError e)
			{
				error = e;
			}
		}

		[Then(@"the user will (not )?be marked for deletion")]
		public void ThenTheUserWillBeMarkedForDeletion(Boolean marked)
		{
			var user = repos.User.GetByEmail(userEmail);

			if (marked)
				Assert.NotNull(user.Control.WipeRequest);
			else
				Assert.Null(user.Control.WipeRequest);
		}
		#endregion

		#region Misc
		[Given(@"it has a Misc")]
		public void GivenItHasAMisc()
		{
			misc = current.Misc;
		}

		[When(@"regenerate misc")]
		public void WhenRegenerateMisc()
		{
			try
			{
				service.Clip.ReMisc(password);
			}
			catch (CoreError e)
			{
				error = e;
			}
		}

		[Then(@"the user will (not )?have changed misc")]
		public void ThenTheUserWillHaveChangedMisc(Boolean changed)
		{
			var user = repos.User.GetByEmail(userEmail);

			if (changed)
				Assert.AreNotEqual(misc, user.GenerateMisc());
			else
				Assert.AreEqual(misc, user.GenerateMisc());
		}
		#endregion
	}
}
