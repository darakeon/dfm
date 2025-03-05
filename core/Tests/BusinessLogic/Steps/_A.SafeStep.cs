using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Amazon.Runtime.Internal.Util;
using DFM.BusinessLogic.Exceptions;
using DFM.BusinessLogic.Response;
using DFM.BusinessLogic.Tests.Helpers;
using DFM.Entities;
using DFM.Entities.Bases;
using DFM.Entities.Enums;
using DFM.Generic;
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
			get => get<String>();
			set => set(value);
		}

		private String password
		{
			get => get<String>();
			set => set(value);
		}

		private String retypePassword
		{
			get => get<String>();
			set => set(value);
		}

		private String newPassword
		{
			get => get<String>();
			set => set(value);
		}

		private String currentPassword
		{
			get => get<String>();
			set => set(value);
		}

		private String tfaCode
		{
			get => get<String>();
			set => set(value);
		}

		private String language
		{
			get => get<String>();
			set => set(value);
		}
		
		private String timezone
		{
			get => get<String>();
			set => set(value);
		}

		private Boolean acceptedContract
		{
			get => get<Boolean>();
			set => set(value);
		}

		private SecurityAction action
		{
			get => get<SecurityAction>();
			set => set(value);
		}

		private IList<TicketInfo> logins
		{
			get => get<IList<TicketInfo>>();
			set => set(value);
		}

		private Boolean? accepted
		{
			get => get<Boolean?>();
			set => set(value);
		}

		private TFAInfo tfa
		{
			get => get<TFAInfo>();
			set => set(value);
		}

		private Boolean? ticketVerified
		{
			get => get<Boolean?>();
			set => set(value);
		}

		private Misc misc
		{
			get => get<Misc>();
			set => set(value);
		}

		private Plan plan
		{
			get => get<Plan>();
			set => set(value);
		}
		#endregion


		#region SaveUserAndSendVerify
		[Given(@"I have this user data")]
		public void GivenIHaveThisUserData(Table table)
		{
			if (table.Header.Any(c => c == "Email"))
				email = table.Rows[0]["Email"]
					.ForScenario(scenarioCode);

			if (table.Header.Any(c => c == "Password"))
				password = table.Rows[0]["Password"];

			if (table.Header.Any(c => c == "Retype Password"))
				retypePassword = table.Rows[0]["Retype Password"];

			if (table.Header.Any(c => c == "Language"))
				language = table.Rows[0]["Language"];

			if (table.Header.Any(c => c == "Timezone"))
				timezone = table.Rows[0]["Timezone"];

			if (table.Header.Any(c => c == "Accepted Contract"))
				acceptedContract = table.Rows[0]["Accepted Contract"] == "true";
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
					Language = language,
					TimeZone = timezone,
					AcceptedContract = acceptedContract
				};

				service.Auth.SaveUser(info);
			}
			catch (CoreError e)
			{
				testCoreError = e;
			}
		}

		[Then(@"the user will not be changed")]
		public void ThenTheUserWillNotBeChanged()
		{
			var savedUser = repos.User.GetByEmail(email);
			Assert.That(savedUser, Is.Not.Null);

			var rightPassword = Crypt.Check(password, savedUser.Password);
			Assert.That(rightPassword, Is.True);
		}

		[Then(@"the user will (not )?be saved")]
		public void ThenTheUserWillBeSaved(Boolean saved)
		{
			var savedUser = repos.User.GetByEmail(email);

			if (saved)
			{
				Assert.That(savedUser, Is.Not.Null);

				var rightPassword = Crypt.Check(password, savedUser.Password);
				Assert.That(rightPassword, Is.True);
			}
			else
			{
				Assert.That(savedUser, Is.Null);
			}
		}

		[Then(@"it will have a misc")]
		public void ThenItWillHaveAMisc()
		{
			var savedUser = repos.User.GetByEmail(email);
			Assert.That(savedUser, Is.Not.Null);
			Assert.That(savedUser.Control.MiscDna, Is.Not.Zero);
		}
		#endregion

		#region SendUserVerify
		/* Use same tests of others */
		#endregion

		#region SendPasswordReset
		[When(@"I try to send the e-mail of password reset")]
		public void WhenITryToSendTheEmailOfPasswordReset()
		{
			try
			{
				service.Outside.SendPasswordReset(email);
			}
			catch (CoreError e)
			{
				testCoreError = e;
			}
		}

		[Then(@"there will be no active logins")]
		public void ThenThereWillBeNoActiveLogins()
		{
			var user = repos.User.GetByEmail(email);
			var loginForUser = repos.Ticket.List(user).ToList();

			Assert.That(loginForUser.Count, Is.EqualTo(0));
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
				testCoreError = e;
			}
		}

		[Then(@"the user will (not )?be activated")]
		public void ThenTheUserWillNotBeActivated(Boolean active)
		{
			var user = repos.User.GetByEmail(email ?? userEmail);
			Assert.That(user.Control.Active, Is.EqualTo(active));
		}

		[Then(@"the password wrong attempts will be (\d+)")]
		public void ThenThePasswordWrongAttemptsWillBe(Int32 wrongAttempts)
		{
			var user = repos.User.GetByEmail(email ?? userEmail);
			Assert.That(user.Control.WrongLogin, Is.EqualTo(wrongAttempts));
		}

		[Then(@"the tfa wrong attempts will be (\d+)")]
		public void ThenTheTfaWrongAttemptsWillBe(Int32 wrongAttempts)
		{
			var user = repos.User.GetByEmail(email ?? userEmail);
			Assert.That(user.Control.WrongTFA, Is.EqualTo(wrongAttempts));
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
			email = email.ForScenario(scenarioCode);
			var user = repos.User.GetByEmail(email);
			db.Execute(() => repos.Control.Deactivate(user));
		}

		[When(@"I try to get the ticket")]
		public void WhenITryToGetTheTicket()
		{
			testTicketKey = null;

			try
			{
				var info = new SignInInfo
				{
					Email = email,
					Password = password,
					TicketKey = ticketKey,
					TicketType = TicketType.Tests,
				};

				testTicketKey = service.Auth.CreateTicket(info);
			}
			catch (CoreError e)
			{
				testCoreError = e;
			}
		}

		[Given(@"I tried to get the ticket (\d+) times")]
		[When(@"I try to get the ticket (\d+) times")]
		public void WhenITryToGetTheTicketSomeTimes(Int32 times)
		{
			testTicketKey = null;

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
				if (isCurrent(ScenarioBlock.When))
					testCoreError = e;
			}
		}

		[Then(@"I will (not )?receive the ticket")]
		public void ThenIWillReceiveTheTicket(Boolean receive)
		{
			if (receive)
			{
				Assert.That(testTicketKey, Is.Not.Null);

				var expectedSession = service.Auth.GetSession(testTicketKey);

				Assert.That(expectedSession, Is.Not.Null);
			}
			else
			{
				Assert.That(testTicketKey, Is.Null);
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
				session = service.Auth.GetSession(testTicketKey);
			}
			catch (CoreError e)
			{
				testCoreError = e;
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

		[Given(@"I pass this data to change password")]
		public void GivenIPassThisPassword(Table table)
		{
			var passwordData = table.Rows[0];

			newPassword = passwordData["Password"];
			retypePassword = passwordData["Retype Password"];

			currentPassword = passwordData.TryGetValue(
				"Current Password", currentPassword
			);

			tfaCode = passwordData.TryGetValue(
				"TFA Code", tfaCode
			).GenerateTFA(tfa?.Secret);
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
				testCoreError = e;
			}
		}

		[Then(@"the password will not be changed")]
		public void ThenThePasswordWillNotBeChanged()
		{
			var savedUser = repos.User.GetByEmail(email);
			Assert.That(savedUser, Is.Not.Null);

			var rightPassword = Crypt.Check(password, savedUser.Password);
			Assert.That(rightPassword, Is.True);
		}

		[Then(@"the password will be changed")]
		public void ThenThePasswordWillBeChanged()
		{
			var savedUser = repos.User.GetByEmail(email);
			Assert.That(savedUser, Is.Not.Null);

			var rightPassword = Crypt.Check(newPassword, savedUser.Password);
			Assert.That(rightPassword, Is.True);
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
				testCoreError = e;
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
				testCoreError = e;
			}
		}

		[Then(@"the token will not be valid anymore")]
		public void ThenTheTokenWillNotBeValidAnymore()
		{
			CoreError error = null;

			try
			{
				service.Outside.TestSecurityToken(token, action);
			}
			catch (CoreError e)
			{
				error = e;
			}

			Assert.That(error, Is.Not.Null);
			Assert.That(error.Type, Is.EqualTo(Error.InvalidToken));
		}
		#endregion

		#region Disable Ticket
		[When(@"I try to disable the ticket")]
		public void WhenITryToDisableTheTicket()
		{
			try
			{
				service.Auth.DisableTicket(testTicketKey);
			}
			catch (CoreError e)
			{
				testCoreError = e;
			}
		}

		[Then(@"the ticket will not be valid anymore")]
		public void ThenTheTicketWillNotBeValidAnymore()
		{
			CoreError error = null;

			try
			{
				service.Auth.GetSession(testTicketKey);
			}
			catch (CoreError e)
			{
				error = e;
			}

			Assert.That(error, Is.Not.Null);
			Assert.That(
				error.Type,
				Is.AnyOf(
					Error.Uninvited,
					Error.UserDeleted,
					Error.UserAskedWipe
				)
			);
		}

		[Then(@"the ticket will still be valid")]
		public void ThenTheTicketWillStillBeValid()
		{
			CoreError error = null;

			try
			{
				service.Auth.GetSession(testTicketKey);
			}
			catch (CoreError e)
			{
				error = e;
			}

			Assert.That(error, Is.Null);
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
				testCoreError = e;
			}
		}

		[Then(@"there will be (\d+) logins")]
		public void ThenThereWillBeCountLogins(Int32 count)
		{
			Assert.That(logins.Count, Is.EqualTo(count));
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

				Assert.That(loginDb.Active, Is.True);
			}
		}

		[Then(@"they will not have sensible information")]
		public void ThenTheyWillNotHaveSensibleInformation()
		{
			foreach (var login in logins)
			{
				Assert.That(login.Key.Length, Is.EqualTo(Defaults.TicketShowedPart));
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
					Assert.That(login.Current, Is.True);
				else
					Assert.That(login.Current, Is.False);
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
					RetypePassword = retypePassword,
					TFACode = tfaCode,
				};

				service.Auth.ChangePassword(info);
			}
			catch (CoreError e)
			{
				testCoreError = e;
			}
		}

		[Then(@"only the last login will be active")]
		public void ThenOnlyTheLastLoginWillBeActive()
		{
			var user = repos.User.GetByEmail(email);
			var loginForUser = repos.Ticket.List(user).ToList();

			Assert.That(loginForUser.Count, Is.EqualTo(1));
			Assert.That(loginForUser[0].Key, Is.EqualTo(ticketKey));
		}
		#endregion ChangePassword

		#region UpdateEmail
		[Given(@"I pass this new e-mail and password")]
		public void GivenIPassThisNewEmailAndPassword(Table table)
		{
			var userData = table.Rows[0];

			currentPassword = userData["Current Password"];

			newEmail = userData["New E-mail"]
				.ForScenario(scenarioCode);

			tfaCode = userData.TryGetValue("TFA Code", tfaCode)
				.GenerateTFA(tfa?.Secret);
		}

		[When(@"I try to change the e-mail")]
		public void WhenITryToChangeTheEmail()
		{
			try
			{
				var info = new UpdateEmailInfo
				{
					Email = newEmail,
					Password = currentPassword,
					TFACode = tfaCode,
				};

				service.Auth.UpdateEmail(info);
			}
			catch (CoreError e)
			{
				testCoreError = e;
			}
		}

		[Then(@"the e-mail will not be changed")]
		public void ThenTheEmailWillNotBeChanged()
		{
			var ticket = repos.Ticket.GetByKey(ticketKey);
			Assert.That(ticket.User.Email, Is.EqualTo(email));
		}

		[Then(@"the e-mail will be changed")]
		public void ThenTheEmailWillBeChanged()
		{
			var actualTicket = repos.Ticket.GetByKey(ticketKey);
			var actualEmail = actualTicket?.User?.Email;
			Assert.That(actualEmail, Is.EqualTo(newEmail));

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
				testCoreError = e;
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

			Assert.That(ticketList.Count, Is.Not.EqualTo(0), $"no login for {userEmail}");

			var updated = ticketList.Count(
				t => t.LastAccess.ToUniversalTime() > testStart
			);

			var expectedUpdated = after ? 1 : 0;
			Assert.That(updated, Is.EqualTo(expectedUpdated));
		}

		[Then(@"the user access will( not)? be null")]
		public void ThenTheUserAccessWillBeNull(Boolean isNull)
		{
			var user = repos.User.GetByEmail(userEmail);
			var lastAccess = user.Control.LastAccess?.ToUniversalTime();

			if (isNull)
				Assert.That(lastAccess, Is.Null);
			else
				Assert.That(lastAccess, Is.Not.Null);
		}
		#endregion

		#region Get Plan
		[When(@"get the plan")]
		public void WhenGetThePlan()
		{
			try
			{
				plan = service.Law.GetPlan();
			}
			catch (CoreError e)
			{
				testCoreError = e;
			}
		}

		[Then(@"the plan will be")]
		public void ThenThePlanWillBe(Table table)
		{
			var expectedPlan = table.CreateInstance<Plan>();

			Assert.That(plan.Name, Is.EqualTo(expectedPlan.Name));
			Assert.That(plan.Price, Is.EqualTo(expectedPlan.Price));
			Assert.That(plan.AccountOpened, Is.EqualTo(expectedPlan.AccountOpened));
			Assert.That(plan.CategoryEnabled, Is.EqualTo(expectedPlan.CategoryEnabled));
			Assert.That(plan.ScheduleActive, Is.EqualTo(expectedPlan.ScheduleActive));
			Assert.That(plan.AccountMonthMove, Is.EqualTo(expectedPlan.AccountMonthMove));
			Assert.That(plan.MoveDetail, Is.EqualTo(expectedPlan.MoveDetail));
			Assert.That(plan.ArchiveMonthUpload, Is.EqualTo(expectedPlan.ArchiveMonthUpload));
			Assert.That(plan.ArchiveLine, Is.EqualTo(expectedPlan.ArchiveLine));
			Assert.That(plan.ArchiveSize, Is.EqualTo(expectedPlan.ArchiveSize));
			Assert.That(plan.OrderMonth, Is.EqualTo(expectedPlan.OrderMonth));
			Assert.That(plan.OrderMove, Is.EqualTo(expectedPlan.OrderMove));
		}
		#endregion

		#region UseTFAAsPassword
		[When(@"I set to (not )?use TFA as password")]
		public void WhenISetToUseTFAAsPassword(Boolean use)
		{
			try
			{
				service.Auth.UseTFAAsPassword(use, tfa);
			}
			catch (CoreError e)
			{
				testCoreError = e;
			}
		}

		[Then(@"the TFA can (not )?be used as password")]
		public void ThenTheTFACanBeUsedAsPassword(Boolean canBe)
		{
			var currentEmail = email ?? userEmail;
			var user = repos.User.GetByEmail(currentEmail);

			var code = user.TFASecret == null
				? null
				: CodeGenerator.Generate(user.TFASecret);

			var worked = true;

			try
			{
				var info = new SignInInfo
				{
					Email = currentEmail,
					Password = code,
					TicketKey = testTicketKey,
					TicketType = TicketType.Tests
				};

				service.Auth.CreateTicket(info);
			}
			catch (CoreError)
			{
				worked = false;
			}

			Assert.That(worked, Is.EqualTo(canBe));

			Assert.That(user.TFAPassword, Is.EqualTo(canBe));
		}

		[Then(@"the TFA will (not )?be asked")]
		public void ThenTheTFAWillNotBeAsked(Boolean askTFA)
		{
			var currentTicket = repos.Ticket.GetByKey(testTicketKey);
			Assert.That(!currentTicket.ValidTFA, Is.EqualTo(askTFA));
		}

		[Then(@"I can still login using normal password")]
		public void ThenICanStillLoginUsingNormalPassword()
		{
			testTicketKey = TK.New();

			var info = new SignInInfo
			{
				Email = email,
				Password = password,
				TicketKey = testTicketKey,
				TicketType = TicketType.Tests,
			};

			testTicketKey = service.Auth.CreateTicket(info);
		}
		#endregion

		#region SendWipedUserCSV

		[Given(@"robot call wipe users")]
		public void WhenRobotWipeUsers()
		{
			robotRunWipe();
		}

		[Given(@"I wipe the file")]
		public void GivenIWipeTheFile()
		{
			service.Outside.WipeCsv(token);
		}

		[When(@"ask wiped user csv")]
		public void WhenAskWipedUserCsv(Table table)
		{
			var row = table.Rows[0];
			var email = row["Email"]
				.ForScenario(scenarioCode);
			var password = row["Password"];

			try
			{
				service.Outside.SendWipedUserCSV(email, password);
			}
			catch (CoreError e)
			{
				testCoreError = e;
			}
		}

		[Then(@"(\d+ )?emails? with csv will (not )?be sent( with a link to delete it)?")]
		public void ThenCSVWill_BeSent(Int32 emailCount, Boolean csvSent, String hasLink)
		{
			var inboxPath = Path.Combine(outputsPath, "inbox");
			var inbox = new DirectoryInfo(inboxPath);

			var emails = inbox.GetFiles("*.eml")
				.Where(f => f.CreationTimeUtc >= testStart)
				.Select(f => EmlReader.FromFile(f.FullName))
				.Where(e => e.Headers["To"] == userEmail)
				.Where(e => e.Subject == "Dados exportados")
				.ToList();

			if (csvSent)
			{
				Assert.That(emails.Count, Is.EqualTo(emailCount));

				foreach (var email in emails)
				{
					var countAttachments = email.Attachments.Count;

					Assert.That(countAttachments, Is.EqualTo(1));

					if (hasLink != "")
					{
						var link = $"https://dontflymoney.com/>DeleteCsvData>{token}";

						Assert.That(
							email.Body.Contains(link),
							() => $"{link}\nnot found at\n{email.Body}"
						);
					}
				}
			}
			else
			{
				Assert.That(emails.Count, Is.EqualTo(0));
			}
		}

		#endregion SendWipedUserCSV

		#region WipeCSV
		[Given(@"the user was wiped once")]
		public void GivenIHaveAWipedUser()
		{
			var email = $"{scenarioCode}@dontflymoney.com";
			var user = repos.User.GetByEmail(email);
			var wipe = Wipe.FromUser(user);

			wipe.S3 = $"{scenarioCode}.csv";

			repos.Wipe.SaveOrUpdate(wipe);

			var path = Path.Combine(
				Cfg.S3.Directory,
				StoragePurpose.Wipe.ToString(),
				wipe.S3
			);
			File.WriteAllText(path, "hey, listen!");
		}

		[When(@"I try to wipe the file")]
		public void WhenITryToWipeTheFile()
		{
			try
			{
				service.Outside.WipeCsv(token);
			}
			catch (CoreError e)
			{
				testCoreError = e;
			}
		}

		[Then(@"the file will (not )?be wiped")]
		public void ThenTheFileWill_BeWiped(Boolean wiped)
		{
			var path = Path.Combine(
				Cfg.S3.Directory,
				StoragePurpose.Wipe.ToString(),
				$"{scenarioCode}.csv"
			);
			
			Assert.That(
				wiped,
				Is.EqualTo(!File.Exists(path)),
				$"File {(wiped?"not ":"")}found"
			);
		}
		#endregion

		#region MoreThanOne
		[Given(@"I have (?:this user|these users) created")]
		public void GivenIHaveThisUserCreated(Table table)
		{
			foreach (var userData in table.Rows)
			{
				var email = userData["Email"]
					.ForScenario(scenarioCode);

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

				var theme = userData.ContainsKey("Theme")
					? EnumX.Parse<Theme>(userData["Theme"])
					: default(Theme?);

				var language = userData.ContainsKey("Language")
					? userData["Language"]
					: null;

				var days = userData.ContainsKey("Days")
					? Int32.Parse(userData["Days"])
					: -27;

				createUserIfNotExists(email, password, active, signed, timezone, theme, language, days);
			}
		}

		[Given(@"I have a token for its activation")]
		public void GivenIHaveATokenForItsActivation()
		{
			service.Outside.SendUserVerify(email ?? userEmail);
		}

		[Given(@"I have a token for its password reset")]
		public void GivenIHaveATokenForItsPasswordReset()
		{
			service.Outside.SendPasswordReset(email);
		}

		[Given(@"I have a token for its tfa removal")]
		public void GivenIHaveATokenForItsTFARemoval()
		{
			service.Auth.AskRemoveTFA(userPassword);
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
			security.Expire = DateTime.UtcNow.AddDays(-2);
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
		public void GivenIPassAnEmailThatDoesNotExist()
		{
			email = "dont_exist@dontflymoney.com";
		}

		[Given(@"I pass a ticket that doesn't exist")]
		public void GivenIPassATicketThatDoesNotExist()
		{
			testTicketKey = TK.New();
		}

		[Given(@"I pass a null ticket")]
		public void GivenIPassANullTicket()
		{
			testTicketKey = null;
		}

		[Given(@"I pass an empty ticket")]
		public void GivenIPassAnEmptyTicket()
		{
			testTicketKey = "";
		}

		[Given(@"I pass a ticket that is already disabled")]
		public void GivenIPassATicketThatIsAlreadyInvalid()
		{
			service.Auth.DisableTicket(testTicketKey);
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

			testTicketKey = service.Auth.CreateTicket(info);
		}

		[Given(@"I pass a ticket that exist")]
		public void GivenIPassATicketThatExist()
		{
			var userID = repos.User.GetByEmail(email).ID;

			testTicketKey = repos.Ticket.Where(
				t => t.User.ID == userID
					&& t.Expiration == null
			).FirstOrDefault()?.Key;
		}

		[Given(@"I pass a valid (UserVerification|PasswordReset|UnsubscribeMoveMail|DeleteCsvData|RemoveTFA) token")]
		public void GivenIPassTheValidToken(SecurityAction action)
		{
			this.action = action;

			var tokenEmail = email ?? userEmail;

			switch (action)
			{
				case SecurityAction.UnsubscribeMoveMail:
				{
					var user = repos.User.GetByEmail(tokenEmail);
					var security = repos.Security.Grab(
						user, SecurityAction.UnsubscribeMoveMail
					);
					token = security.Token;
					break;
				}
				case SecurityAction.DeleteCsvData:
				{
					var wipes = repos.Wipe.GetByUser(tokenEmail, userPassword);
					var security = repos.Security.Create(wipes[0]);
					token = security.Token;
					break;
				}
				default:
				{
					token = getLastTokenForUser(tokenEmail, action);
					break;
				}
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

			testTicketKey = service.Auth.CreateTicket(info);
		}

		[Given(@"the token expires")]
		public void GivenTheTokenExpires()
		{
			var security = repos.Security.GetByToken(token);
			security.Expire = DateTime.UtcNow.AddDays(-1);
			repos.Security.SaveOrUpdate(security);
		}

		[When(@"I try to send the e-mail of user verify")]
		public void WhenITryToSendTheEmailOfUserVerify()
		{
			try
			{
				service.Outside.SendUserVerify(email);
			}
			catch (CoreError e)
			{
				testCoreError = e;
			}
		}


		[Then(@"I will receive no session")]
		public void ThenIWillReceiveNoSession()
		{
			Assert.That(session, Is.Null);
		}

		[Then(@"I will receive the session")]
		public void ThenIWillReceiveTheSession()
		{
			Assert.That(session, Is.Not.Null);
			Assert.That(session.Email, Is.EqualTo(email));
		}

		[Then(@"the TFA will (not )?be enabled")]
		public void ThenTheTFAWill_BeEnabled(Boolean enabled)
		{
			Assert.That(session.HasTFA, Is.EqualTo(enabled));
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
				testCoreError = e;
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
				testCoreError = e;
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
				testCoreError = e;
			}
		}

		[Then(@"the contract status will be (not )?accepted")]
		public void ThenTheContractStatusWillBeAccepted(Boolean expectAccepted)
		{
			if (!current.IsAuthenticated)
			{
				current.Set(userEmail, userPassword, false);
			}

			accepted ??= service.Law.IsLastContractAccepted();

			Assert.That(accepted, Is.EqualTo(expectAccepted));
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
				.ForScenario(scenarioCode);

			var user = repos.User.GetByEmail(userTable.Email);

			if (user.Control is { Active: false })
				repos.Control.Activate(user);

			var info = new SignInInfo
			{
				Email = userTable.Email.ForScenario(scenarioCode),
				Password = userTable.Password,
				TicketKey = ticketKey,
				TicketType = TicketType.Tests,
			};

			testTicketKey = service.Auth.CreateTicket(info);
		}

		[Given(@"I have this two-factor data")]
		public void GivenIHaveThisTwoFactorData(Table table)
		{
			tfa = table.CreateInstance<TFAInfo>();
			var secret = tfa.Secret
			    ?? repos.User.GetByEmail(userEmail).TFASecret;

			tfa.TFACode = tfa.TFACode.GenerateTFA(secret);

			if (tfa.Password == "{null}")
			{
				tfa.Password = null;
			}
		}

		[Given(@"I set to (not )?use TFA as password")]
		public void GivenISetToUseTFAAsPassword(Boolean use)
		{
			service.Auth.UseTFAAsPassword(use, tfa);
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
					testCoreError = e;
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
				service.Auth.RemoveTFA(tfa);
			}
			catch (CoreError e)
			{
				if (isCurrent(ScenarioBlock.When))
					testCoreError = e;
				else
					throw;
			}
		}

		[Then(@"the two-factor will be empty")]
		public void ThenTheTwoFactorWillStillBeEmpty()
		{
			var user = repos.User.GetByEmail(userEmail);
			Assert.That(user?.TFASecret, Is.Null);
		}

		[Then(@"the two-factor will be \[(.*)\]")]
		public void ThenTheTwoFactorWillStillBe(String secret)
		{
			var user = repos.User.GetByEmail(userEmail);
			Assert.That(user.TFASecret, Is.EqualTo(secret));
		}

		[Given(@"I have not valid ticket key")]
		public void GivenIHaveNotValidTicketKey()
		{
			testTicketKey = new Random().Next().ToString();
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

			testTicketKey = service.Auth.CreateTicket(info);
		}

		[Given(@"I validate the ticket two factor")]
		[When(@"I try to validate the ticket two factor")]
		public void ValidateTheTicketTwoFactor()
		{
			try
			{
				service.Auth.ValidateTicketTFA(tfa.TFACode);
			}
			catch (CoreError exception)
			{
				if (isCurrent(ScenarioBlock.When))
					testCoreError = exception;
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
				testCoreError = exception;
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
				testCoreError = exception;
			}
		}

		[Then(@"the ticket will (not )?be verified")]
		public void ThenTheTicketWillBeVerified(Boolean verified)
		{
			var recordedVerified = ticketVerified == true;
			Assert.That(recordedVerified, Is.EqualTo(verified));
		}

		[Then(@"the ticket will (not )?be valid")]
		public void ThenTheTicketWillBeValidated(Boolean valid)
		{
			var recordedValidated = service.Auth.VerifyTicketTFA();
			Assert.That(recordedValidated, Is.EqualTo(valid));
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
				service.Attendant.AskWipe(password);
			}
			catch (CoreError e)
			{
				testCoreError = e;
			}
		}

		[Then(@"the user will (not )?be marked for deletion")]
		public void ThenTheUserWillBeMarkedForDeletion(Boolean marked)
		{
			var user = repos.User.GetByEmail(userEmail);

			if (marked)
				Assert.That(user.Control.WipeRequest, Is.Not.Null);
			else
				Assert.That(user.Control.WipeRequest, Is.Null);
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
				testCoreError = e;
			}
		}

		[Then(@"the user will (not )?have changed misc")]
		public void ThenTheUserWillHaveChangedMisc(Boolean changed)
		{
			var user = repos.User.GetByEmail(userEmail);

			if (changed)
				Assert.That(user.GenerateMisc(), Is.Not.EqualTo(misc));
			else
				Assert.That(user.GenerateMisc(), Is.EqualTo(misc));
		}
		#endregion

		#region AskRemoveTFA
		[When(@"I ask to remove two-factor")]
		public void WhenIAskToRemoveTwo_Factor()
		{
			try
			{
				service.Auth.AskRemoveTFA(tfa.Password);
			}
			catch (CoreError e)
			{
				testCoreError = e;
			}
		}

		[Then(@"a token for remove TFA will (not )?be generated")]
		public void ThenATokenForRemoveTFAWill_BeGenerated(Boolean generated)
		{
			var user = repos.User.GetByEmail(userEmail);
			var security = repos.Security.Get(user, SecurityAction.RemoveTFA);

			if (generated)
			{
				Assert.That(security, Is.Not.Null);
				token = security.Token;
			}
			else
			{
				Assert.That(security, Is.Null);
			}
		}

		[Then(@"email with a link to remove two-factor will (not )?be sent")]
		public void ThenEmailWill_BeSentWithALinkToRemoveTwoFactor(Boolean sent)
		{
			var inboxPath = Path.Combine(outputsPath, "inbox");
			var inbox = new DirectoryInfo(inboxPath);

			var emails = inbox.GetFiles("*.eml")
				.Where(f => f.CreationTimeUtc >= testStart)
				.Select(f => EmlReader.FromFile(f.FullName))
				.Where(e => e.Headers["To"] == userEmail)
				.Where(e => e.Subject == "Desativar Login mais Seguro")
				.ToList();

			if (sent)
			{
				Assert.That(emails.Count, Is.EqualTo(1));

				var emailSent = emails.Single();

				var link = $"https://dontflymoney.com/>RemoveTFA>{token}";

				Assert.That(
					emailSent.Body.Contains(link),
					() => $"{link}\nnot found at\n{emailSent.Body}"
				);
			}
			else
			{
				Assert.That(emails.Count, Is.EqualTo(0));
			}
		}
		#endregion

		#region RemoveTFAByToken
		[When(@"remove tfa by token")]
		public void WhenRemoveTfaByToken()
		{
			try
			{
				service.Auth.RemoveTFAByToken(token);
			}
			catch (CoreError e)
			{
				testCoreError = e;
			}
		}

		[Then(@"an email warning tfa removal will be sent")]
		public void ThenAnEmailWarningTfaRemovalWillBeSent()
		{
			var inboxPath = Path.Combine(outputsPath, "inbox");
			var inbox = new DirectoryInfo(inboxPath);

			var emlReaders = inbox.GetFiles("*.eml")
				.Where(f => f.CreationTimeUtc >= testStart)
				.Select(f => EmlReader.FromFile(f.FullName))
				.Where(e => e.Headers["To"] == userEmail);
			var emails = emlReaders
				.Where(e => e.Subject == "Login mais Seguro desativado")
				.ToList();

			Assert.That(emails.Count, Is.EqualTo(1));
		}
		#endregion
	}
}
