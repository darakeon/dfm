using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DFM.BusinessLogic.Exceptions;
using DFM.BusinessLogic.Response;
using DFM.BusinessLogic.Tests.Helpers;
using DFM.Entities;
using DFM.Entities.Bases;
using DFM.Entities.Enums;
using DFM.Generic;
using DFM.Language.Emails;
using DFM.Tests.Util;
using Keon.Util.Crypto;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace DFM.BusinessLogic.Tests.D.Robot
{
	[Binding]
	public class RobotStep : BaseStep
	{
		public RobotStep(ScenarioContext context)
			: base(context) { }

		#region Variables
		private Guid guid
		{
			get => get<Guid>("ID");
			set => set("ID", value);
		}

		private IList<ScheduleInfo> scheduleList
		{
			get => get<IList<ScheduleInfo>>("scheduleList");
			set => set("scheduleList", value);
		}

		private IList<String> csv
		{
			get => get<IList<String>>("csv");
			set => set("csv", value);
		}

		private Boolean hasSchedule
		{
			get => get<Boolean>("hasSchedule");
			set => set("hasSchedule", value);
		}
		#endregion

		#region SaveSchedule
		[Given(@"the schedule has this details")]
		public void GivenTheFutureMoveHasThisDetails(Table table)
		{
			foreach (var detailData in table.Rows)
			{
				var detail = getDetailFromTable(detailData);
				scheduleInfo.DetailList.Add(detail);
			}
		}

		[Given(@"I have no schedule")]
		public void GivenIHaveNoSchedule()
		{
			scheduleInfo = null;
		}

		[When(@"I try to save the schedule")]
		public void WhenITryToSaveTheSchedule()
		{
			try
			{
				if (scheduleInfo == null)
				{
					service.Robot.SaveSchedule(null);
				}
				else
				{
					scheduleInfo.OutUrl = accountOut?.Url;
					scheduleInfo.InUrl = accountIn?.Url;
					scheduleInfo.CategoryName = categoryName;

					scheduleResult = service.Robot.SaveSchedule(scheduleInfo);
					scheduleInfo.Guid = scheduleResult.Guid;
				}
			}
			catch (CoreError e)
			{
				error = e;
			}
		}

		[Then(@"the schedule will not be saved")]
		public void ThenTheScheduleWillNotBeSaved()
		{
			if (scheduleInfo != null)
				Assert.AreEqual(Guid.Empty, scheduleInfo.Guid);
		}

		[Then(@"the schedule will be saved")]
		public void ThenTheScheduleWillBeSaved()
		{
			Assert.AreNotEqual(Guid.Empty, scheduleInfo.Guid);
		}

		[Then(@"the schedule value will be (\d+\.?\d*)")]
		public void ThenTheScheduleValueWillBe(Decimal value)
		{
			var schedule = repos.Schedule.Get(scheduleResult.Guid);
			Assert.AreEqual(value, schedule.Value);
		}

		[Then(@"the next robot schedule run will check the user")]
		public void ThenTheNextRobotScheduleRunWillCheckTheUser()
		{
			var user = repos.User.GetByEmail(userEmail);
			var nextCheck = user.Control.RobotCheck.ToUniversalTime();
			Assert.Less(testStart, nextCheck);
			Assert.Greater(DateTime.UtcNow, nextCheck);
		}
		#endregion

		#region RunSchedule
		[Given(@"I have no logged user \(logoff\)")]
		public void GivenIHaveNoLoggedUserLogoff()
		{
			current.Clear();
			resetTicket();
		}

		[Given(@"its Date is (\-?\d+\.?\d*) (day|month|year)s? ago")]
		public void GivenItsDateIsDaysAgo(Int32 count, String frequency)
		{
			scheduleInfo.SetDate(current.Now);
			scheduleInfo.AddByFrequency(frequency, -count);
		}

		[Given(@"I have a bugged schedule")]
		public void GivenIHaveABuggedSchedule()
		{
			var user = repos.User.GetByEmail(current.Email);

			repos.Schedule.SaveOrUpdate(
				new Schedule
				{
					Description = "",
					User = user,
				}
			);
		}

		[Given(@"robot run the scheduler")]
		public void GivenRobotRunTheScheduler()
		{
			robotRunSchedule();
		}

		[Given(@"a schedule is created by (.+)")]
		public void GivenTheCreatedUserHasASchedule(String email)
		{
			email = email.Replace("{scenarioCode}", scenarioCode);

			resetTicket();
			current.Set(email, userPassword, false);

			getOrCreateAccount(accountOutUrl);

			var info = new ScheduleInfo
			{
				Description = "Schedule",
				Day = 23,
				Month = 4,
				Year = 2021,
				OutUrl = accountOutUrl,
				Frequency = ScheduleFrequency.Monthly,
				Nature = MoveNature.Out,
				Times = 1,
				Value = 8,
			};

			service.Robot.SaveSchedule(info);
		}

		[Given(@"robot already ran for (.+)")]
		public void GivenRobotAlreadyRanFor(String email)
		{
			db.Execute(() =>
			{
				var user = repos.User.GetByEmail(email);
				user.SetRobotCheckDay();
				repos.User.SaveOrUpdate(user);
			});
		}

		[Given(@"the user is a robot")]
		public void GivenTheUserIsARobot()
		{
			var user = repos.User.GetByEmail(userEmail);
			user.Control.IsRobot = true;
			db.Execute(() => repos.Control.SaveOrUpdate(user.Control));
		}

		[Given(@"(.+\@.+) is a robot")]
		public void GivenIsARobot(String email)
		{
			var user = repos.User.GetByEmail(email);
			user.Control.IsRobot = true;
			db.Execute(() => repos.Control.SaveOrUpdate(user.Control));
		}

		[When(@"run the scheduler")]
		public void WhenITryToRunTheScheduler()
		{
			try
			{
				var errors = service.Robot.RunSchedule();

				error = errors
					.SingleOrDefault(
						e => e.Key == userEmail
					).Value?
					.FirstOrDefault();
			}
			catch (CoreError e)
			{
				error = e;
			}
		}

		[When(@"run the scheduler with e-mail system out")]
		public void WhenITryToRunTheSchedulerWithEMailSystemOut()
		{
			TestSettings.ActivateMoveEmailForUser(service);
			TestSettings.BreakTheEmailSystem();

			try
			{
				robotRunSchedule();
			}
			catch (CoreError e)
			{
				error = e;
			}

			TestSettings.FixTheEmailSystem();
			TestSettings.DeactivateMoveEmailForUser(service);
		}

		[When(@"run the scheduler with e-mail system ok")]
		public void WhenITryToRunTheSchedulerWithEMailSystemOk()
		{
			TestSettings.ActivateMoveEmailForUser(service);

			try
			{
				robotRunSchedule();
			}
			catch (CoreError e)
			{
				error = e;
			}

			TestSettings.DeactivateMoveEmailForUser(service);
		}

		[Then(@"the user (.+) will still have no moves")]
		public void ThenTheUserWillStillHaveNoMoves(String email)
		{
			var user = repos.User.GetByEmail(email);
			var accounts = repos.Account.Get(user, true);

			foreach (var account in accounts)
			{
				var moves = repos.Move.ByAccount(account);
				Assert.IsEmpty(moves);
			}
		}
		#endregion

		#region DisableSchedule
		[Given(@"I pass an id of Schedule that doesn't exist")]
		public void GivenIPassAnIdOfScheduleThatDoesNotExist()
		{
			guid = Guid.NewGuid();
		}

		[Given(@"I already have disabled the Schedule")]
		public void GivenIAlreadyHaveDisabledTheSchedule()
		{
			service.Robot.DisableSchedule(guid);
		}

		[When(@"I try to disable the Schedule")]
		public void WhenITryToDisableTheSchedule()
		{
			try
			{
				service.Robot.DisableSchedule(guid);
			}
			catch (CoreError e)
			{
				error = e;
			}
		}
		#endregion

		#region GetScheduleList
		[Given(@"I disable the schedule")]
		public void GivenICloseTheSchedule()
		{
			service.Robot.DisableSchedule(guid);
		}

		[When(@"ask for the schedule list")]
		public void WhenAskForAllTheScheduleList()
		{
			try
			{
				scheduleList = service.Robot.GetScheduleList();
			}
			catch (CoreError e)
			{
				error = e;
			}
		}

		[Then(@"the schedule list will (not )?have this")]
		public void ThenTheScheduleListsWillBeThis(Boolean has, Table table)
		{
			var expectedList = new List<Schedule>();

			foreach (var scheduleData in table.Rows)
			{
				var schedule = new Schedule
				{
					Description = scheduleData["Name"]
				};

				expectedList.Add(schedule);
			}

			foreach (var expected in expectedList)
			{
				var schedule = scheduleList.SingleOrDefault(
					s => s.Description == expected.Description
				);

				if (has)
				{
					Assert.IsNotNull(schedule);
				}
				else
				{
					Assert.IsNull(schedule);
				}
			}
		}
		#endregion

		#region CleanupAbandonedUsers
		[Given(@"the user last access was (\d+) days before")]
		public void GivenTheUserLastAccessWas(Int32 days)
		{
			var control = repos.User.GetByEmail(userEmail).Control;
			control.LastAccess = DateTime.UtcNow.AddDays(-days);

			db.Execute(() => repos.Control.SaveOrUpdate(control));
		}

		[Given(@"the user have being warned (once|twice)")]
		public void GivenTheUserHaveBeingWarned(Int32 times)
		{
			var user = repos.User.GetByEmail(userEmail);
			user.Control.RemovalWarningSent = times;
			db.Execute(() => repos.Control.SaveOrUpdate(user.Control));
		}

		[Given(@"a contract from (\d+) days before")]
		public void GivenIHaveAContract(Int32 days)
		{
			var contract = new Contract
			{
				BeginDate = DateTime.UtcNow.AddDays(-days),
				Version = $"Contract {scenarioCode}",
			};

			db.Execute(() =>
				repos.Contract.SaveOrUpdate(contract)
			);


		}

		[Given(@"the user creation was (\d+) days before")]
		public void GivenTheUserCreationWas(Int32 days)
		{
			var user = repos.User.GetByEmail(userEmail);
			var control = user.Control;
			control.Creation = DateTime.UtcNow.AddDays(-days);

			db.Execute(() => repos.Control.SaveOrUpdate(control));
		}

		[Given(@"the user (.*)have")]
		public void GivenTheUserHave(String email, Table table)
		{
			email = String.IsNullOrWhiteSpace(email)
				? userEmail
				: email.Trim();

			var user = repos.User.GetByEmail(email);
			foreach (var row in table.Rows)
			{
				createFor(user, row["System Stuff"]);
			}
		}

		[Given(@"user(.*) language is (pt-BR|en-US)")]
		[When(@"user(.*) language is (pt-BR|en-US)")]
		public void GivenUserLanguageIsPt_BR(String email, String language)
		{
			email = email.Trim();

			if (email == "")
				email = userEmail;

			if (email == "robot")
			{
				createLogoffLoginRobot();
				email = robotEmail;
			}

			var user = repos.User.GetByEmail(email);

			user.Settings.Language = language;

			db.Execute(() => repos.Settings.Update(user.Settings));
		}

		[When(@"call wipe users")]
		public void WhenRobotWipeUsers()
		{
			try
			{
				service.Robot.WipeUsers(path =>
				{
					csv = File.ReadAllLines(path);
				});
			}
			catch (CoreError e)
			{
				error = e;
			}
		}

		[Then(@"the user (.*)will still exist")]
		public void ThenTheUserWillStillExists(String email)
		{
			email = String.IsNullOrWhiteSpace(email)
				? userEmail
				: email.Trim();

			var user = repos.User.GetByEmail(email);
			Assert.IsNotNull(user);
		}

		[Then(@"the user won't exist")]
		public void ThenTheUserWillNotExists()
		{
			var user = repos.User.GetByEmail(userEmail);
			Assert.IsNull(user);
		}

		[Then(@"the count of warnings sent will be (\d+)")]
		public void ThenTheCountOfWarningsSendWillBe(Int32 expectedCount)
		{
			var actualCount = EmlHelper.CountEmails(
				userEmail,
				EmailType.RemovalReason,
				testStart
			);
			Assert.AreEqual(expectedCount, actualCount);
		}

		[Then(@"there will be a wipe notice sent")]
		public void ThenThereWillBeAWipeNoticeSent()
		{
			var actualCount = EmlHelper.CountEmails(
				userEmail,
				EmailType.WipeNotice,
				testStart
			);
			Assert.AreEqual(1, actualCount);
		}

		[Then(@"and the user warning count will be (\d+)")]
		public void ThenAndTheUserWarningCountWillBe(Int32 count)
		{
			var user = repos.User.GetByEmail(userEmail);
			Assert.AreEqual(count, user.Control.RemovalWarningSent);
		}

		[Then(@"there will be an export file with this content")]
		public void ThenThereWillBeAnExportFileWithThisContent(Table table)
		{
			Assert.AreEqual(table.ToCsv(), csv);
		}

		[Then(@"there will no be an export file")]
		public void ThenThereWillNoBeAnExportFile()
		{
			Assert.Null(csv);
		}

		[Then(@"it will be registered at wipe table with reason (\w+)")]
		public void ThenItWillBeRegisteredAtWipeTable(RemovalReason reason)
		{
			var wipe = repos.Wipe.NewQuery()
				.OrderBy(w => w.When, false)
				.FirstOrDefault;

			Assert.NotNull(wipe);

			Assert.IsTrue(Crypt.Check(userEmail, wipe.HashedEmail));

			Assert.AreEqual(userEmail.Substring(0, 2), wipe.UsernameStart);
			Assert.AreEqual("don", wipe.DomainStart);

			Assert.Less(testStart, wipe.When.ToUniversalTime());
			Assert.AreEqual(reason, wipe.Why);

			if (csv != null)
			{
				Assert.NotNull(wipe.S3);
				Assert.True(
					wipe.S3.StartsWith(
						wipe.HashedEmail.ToBase64()
					)
				);
			}
			else
			{
				Assert.Null(wipe.S3);
			}

			Assert.True(Crypt.Check("password", wipe.Password));
		}

		[Then(@"it will not be registered at wipe table")]
		public void ThenItWillNotBeRegisteredAtWipeTable()
		{
			var wipes = repos.Wipe.GetAll();

			foreach (var wipe in wipes)
			{
				Assert.IsFalse(
					Crypt.Check(userEmail, wipe.HashedEmail)
				);
			}
		}

		[Then(@"the e-mail subject will be ""(.*)""")]
		public void ThenTheE_MailSubjectWillBe(String subject)
		{
			var email = EmlHelper.ByEmail(userEmail);
			Assert.AreEqual(subject, email.Subject);
		}

		[Then(@"the e-mail body will contain ""(.*)""")]
		public void ThenTheE_MailBodyWillContain(String bodyPart)
		{
			var email = EmlHelper.ByEmail(userEmail);
			var body = email.Body
				.Replace("\n", "")
				.Replace("\t", "");

			Assert.True(
				body.Contains(bodyPart),
				@$"
					Expected: {bodyPart}
					Actual: {body}
				"
			);
		}
		#endregion

		#region HasSchedules
		[When(@"ask if the user has Schedules")]
		public void WhenAskIfTheUserHasSchedules()
		{
			try
			{
				hasSchedule = service.Robot.HasSchedule();
			}
			catch (CoreError e)
			{
				error = e;
			}
		}

		[Then(@"the answer is (Yes|No)")]
		public void ThenTheAnswerIsYes(Boolean hasSchedule)
		{
			Assert.AreEqual(hasSchedule, this.hasSchedule);
		}
		#endregion

		#region MoreThanOne
		[Given(@"I have this schedule to create")]
		public void GivenIHaveThisMoveToCreate(Table table)
		{
			var scheduleData = table.Rows[0];

			scheduleInfo = new ScheduleInfo
			{
				Description = scheduleData["Description"]
			};

			if (!String.IsNullOrEmpty(scheduleData["Nature"]))
				scheduleInfo.Nature = EnumX.Parse<MoveNature>(scheduleData["Nature"]);

			if (!String.IsNullOrEmpty(scheduleData["Date"]))
				scheduleInfo.SetDate(DateTime.Parse(scheduleData["Date"]));

			if (!String.IsNullOrEmpty(scheduleData["Value"]))
				scheduleInfo.Value = Int32.Parse(scheduleData["Value"]);

			if (!String.IsNullOrEmpty(scheduleData["Times"]))
				scheduleInfo.Times = Int16.Parse(scheduleData["Times"]);

			if (!String.IsNullOrEmpty(scheduleData["Boundless"]))
				scheduleInfo.Boundless = Boolean.Parse(scheduleData["Boundless"]);

			if (!String.IsNullOrEmpty(scheduleData["Frequency"]))
				scheduleInfo.Frequency = EnumX.Parse<ScheduleFrequency>(scheduleData["Frequency"]);

			if (!String.IsNullOrEmpty(scheduleData["ShowInstallment"]))
				scheduleInfo.ShowInstallment = Boolean.Parse(scheduleData["ShowInstallment"]);
		}

		[Given(@"I save the schedule")]
		public void GivenISaveTheSchedule()
		{
			scheduleInfo.OutUrl = accountOut?.Name;
			scheduleInfo.InUrl = accountIn?.Name;
			scheduleInfo.CategoryName = categoryName;

			var schedule = service.Robot.SaveSchedule(scheduleInfo);

			guid = schedule.Guid;
			scheduleInfo.Guid = schedule.Guid;
		}

		[Given(@"I have schedules of")]
		public void GivenIHaveSchedulesOf(Table table)
		{
			foreach (var row in table.Rows)
			{
				Int16.TryParse(row["Times"], out var times);

				scheduleInfo = new ScheduleInfo
				{
					Description = row["Description"],
					Nature = EnumX.Parse<MoveNature>(row["Nature"]),
					Value = Int32.Parse(row["Value"]),
					Times = times,
					Boundless = Boolean.Parse(row["Boundless"]),
					Frequency = EnumX.Parse<ScheduleFrequency>(row["Frequency"]),
					ShowInstallment = Boolean.Parse(row["ShowInstallment"])
				};

				scheduleInfo.SetDate(DateTime.Parse(row["Date"]));

				if (row.ContainsKey("Category"))
					scheduleInfo.CategoryName = row["Category"];

				var scenarioAccountUrl = $"{mainAccountUrl}_{scenarioCode}";

				if (scheduleInfo.Nature != MoveNature.In)
					scheduleInfo.OutUrl = scenarioAccountUrl;

				if (scheduleInfo.Nature != MoveNature.Out)
					scheduleInfo.InUrl = scenarioAccountUrl;

				service.Robot.SaveSchedule(scheduleInfo);
			}
		}

		[Then(@"the schedule will be disabled")]
		public void ThenTheScheduleWillBeDisabled()
		{
			var schedule = repos.Schedule.Get(scheduleInfo.Guid);
			Assert.IsFalse(schedule.Active);
		}

		[Then(@"the schedule will be enabled")]
		public void ThenTheScheduleWillBeEnabled()
		{
			var schedule = repos.Schedule.Get(scheduleInfo.Guid);
			Assert.IsTrue(schedule.Active);
		}

		[Then(@"the schedule last run will be (\d+)")]
		public void ThenTheScheduleLastRunWillBe(Int32 lastRun)
		{
			var schedule = repos.Schedule.Get(scheduleInfo.Guid);
			Assert.AreEqual(lastRun, schedule.LastRun);
		}
		#endregion
	}
}
