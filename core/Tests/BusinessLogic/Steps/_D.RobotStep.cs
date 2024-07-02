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
using Keon.Eml;
using Keon.Util.Crypto;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace DFM.BusinessLogic.Tests.Steps
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

		private Boolean hasSchedule
		{
			get => get<Boolean>("hasSchedule");
			set => set("hasSchedule", value);
		}

		protected String csvName
		{
			get => get<String>("csvName");
			set => set("csvName", value);
		}

		protected String csvContent
		{
			get => get<String>("csvContent");
			set => set("csvContent", value);
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
				Assert.That(scheduleInfo.Guid, Is.EqualTo(Guid.Empty));
		}

		[Then(@"the schedule will be saved")]
		public void ThenTheScheduleWillBeSaved()
		{
			Assert.That(scheduleInfo.Guid, Is.Not.EqualTo(Guid.Empty));
		}

		[Then(@"the schedule value will be (\d+\.?\d*)")]
		public void ThenTheScheduleValueWillBe(Decimal value)
		{
			var schedule = repos.Schedule.Get(scheduleResult.Guid);
			Assert.That(schedule.Value, Is.EqualTo(value));
		}

		[Then(@"the next robot schedule run will check the user")]
		public void ThenTheNextRobotScheduleRunWillCheckTheUser()
		{
			var user = repos.User.GetByEmail(userEmail);
			var nextCheck = user.Control.RobotCheck.ToUniversalTime();

			Assert.That(nextCheck, Is.GreaterThan(testStart));
			Assert.That(nextCheck, Is.LessThan(DateTime.UtcNow));
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
			email = email.ForScenario(scenarioCode);

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
				email = email.ForScenario(scenarioCode);
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
			email = email.ForScenario(scenarioCode);
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
			email = email.ForScenario(scenarioCode);
			var user = repos.User.GetByEmail(email);
			var accounts = repos.Account.Get(user, true);

			foreach (var account in accounts)
			{
				var moves = repos.Move.ByAccount(account);
				Assert.That(moves, Is.Empty);
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
					Assert.That(schedule, Is.Not.Null);
				}
				else
				{
					Assert.That(schedule, Is.Null);
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
				: email.Trim().ForScenario(scenarioCode);

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
				service.Robot.WipeUsers();
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
				: email.Trim().ForScenario(scenarioCode);

			var user = repos.User.GetByEmail(email);
			Assert.That(user, Is.Not.Null);
		}

		[Then(@"the user won't exist")]
		public void ThenTheUserWillNotExists()
		{
			var user = repos.User.GetByEmail(userEmail);
			Assert.That(user, Is.Null);
		}

		[Then(@"the count of warnings sent will be (\d+)")]
		public void ThenTheCountOfWarningsSendWillBe(Int32 expectedCount)
		{
			var actualCount = EmlHelper.CountEmails(
				userEmail,
				EmailType.RemovalReason,
				testStart
			);
			Assert.That(actualCount, Is.EqualTo(expectedCount));
		}

		[Then(@"there will be a wipe notice sent")]
		public void ThenThereWillBeAWipeNoticeSent()
		{
			var actualCount = EmlHelper.CountEmails(
				userEmail,
				EmailType.WipeNotice,
				testStart
			);
			Assert.That(actualCount, Is.EqualTo(1));
		}

		[Then(@"and the user warning count will be (\d+)")]
		public void ThenAndTheUserWarningCountWillBe(Int32 count)
		{
			var user = repos.User.GetByEmail(userEmail);
			Assert.That(user.Control.RemovalWarningSent, Is.EqualTo(count));
		}

		[Then(@"there will be an export file with this content")]
		public void ThenThereWillBeAnExportFileWithThisContent(Table table)
		{
			var expected = table.ToCsv()
				.Select(r => r.ForScenario(scenarioCode));

			var file = Directory
				.GetFiles(Cfg.S3.Directory, "*.csv")
				.Where(f => f.Contains("_"))
				.OrderByDescending(f => f.Split("_")[1])
				.First();

			var content = File.ReadAllLines(file);

			Assert.That(content, Is.EqualTo(expected));
		}

		[Then(@"it will be registered at wipe table")]
		public void ThenItWillBeRegisteredAtWipeTable(Table table)
		{
			var row = table.Rows[0];
			var reason = EnumX.Parse<RemovalReason>(row["Reason"]);
			var hasCSV = row["CSV file"] == "Yes";
			var theme = EnumX.Parse<Theme>(row["Theme"]);
			var language = row["Language"];

			var wipe = repos.Wipe.NewQuery()
				.OrderBy(w => w.When, false)
				.FirstOrDefault;

			Assert.That(wipe, Is.Not.Null);

			Assert.That(Crypt.Check(userEmail, wipe.HashedEmail), Is.True);

			Assert.That(wipe.UsernameStart, Is.EqualTo(userEmail[..2]));
			Assert.That(wipe.DomainStart, Is.EqualTo("don"));

			Assert.That(wipe.When.ToUniversalTime(), Is.GreaterThan(testStart));
			Assert.That(wipe.Why, Is.EqualTo(reason));

			Assert.That(wipe.Theme, Is.EqualTo(theme));
			Assert.That(wipe.Language, Is.EqualTo(language));

			if (hasCSV)
			{
				Assert.That(wipe.S3, Is.Not.Null);
				Assert.That(
					wipe.S3.StartsWith(
						wipe.HashedEmail.ToBase64()
					),
					Is.True
				);
			}
			else
			{
				Assert.That(wipe.S3, Is.Null);
			}

			Assert.That(Crypt.Check("password", wipe.Password), Is.True);
		}

		[Then(@"it will not be registered at wipe table")]
		public void ThenItWillNotBeRegisteredAtWipeTable()
		{
			var wipes = repos.Wipe.GetAll();

			foreach (var wipe in wipes)
			{
				Assert.That(
					Crypt.Check(userEmail, wipe.HashedEmail),
					Is.False
				);
			}
		}

		[Then(@"the e-mail subject will be ""(.*)""")]
		public void ThenTheE_MailSubjectWillBe(String subject)
		{
			var email = EmlHelper.ByEmail(userEmail);
			Assert.That(email.Subject, Is.EqualTo(subject));
		}

		[Then(@"the e-mail body will contain ""(.*)""")]
		public void ThenTheE_MailBodyWillContain(String bodyPart)
		{
			var email = EmlHelper.ByEmail(userEmail);
			var body = email.Body
				.Replace("\n", "")
				.Replace("\t", "");

			Assert.That(
				body.Contains(bodyPart),
				Is.True,
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
			Assert.That(this.hasSchedule, Is.EqualTo(hasSchedule));
		}
		#endregion

		#region ImportMovesFile
		[Given(@"a moves file with this content")]
		public void GivenAMovesFileWithThisContent(Table table)
		{
			var lines = new List<List<String>>
			{
				table.Header.ToList()
			};

			lines.AddRange(
				table.Rows.Select(
					r => r.Values.ToList()
				).ToList()
			);

			if (table.RowCount == 1)
			{
				accountInUrl = table.Rows[0]["In"].IntoUrl();
				accountOutUrl = table.Rows[0]["Out"].IntoUrl();

				categoryName = table.Rows[0]["Category"];

				var parsed = DateTime.TryParse(
					table.Rows[0]["Date"],
					out var date
				);

				if (parsed)
					summaryDate = date;
			}

			csvName = $"{scenarioCode}.csv";

			csvContent = String.Join(
				"\n",
				lines.Select(
					l => String.Join(",", l)
				)
			).ForScenario(scenarioCode);
		}

		[Given(@"a moves file with ([\w ]+)")]
		public void GivenAMovesFile(String filename)
		{
			csvName = $"{filename.Replace(" ", "_")}.csv";

			csvContent = File.ReadAllText(
				Path.Combine("..", "..", "..", "CSVExamples", csvName)
			);
		}

		[When(@"import moves file")]
		public void WhenImportMovesFile()
		{
			try
			{
				service.Robot.ImportMovesFile(csvName, csvContent);
			}
			catch (CoreError e)
			{
				error = e;
			}
		}

		[Then(@"the pre-import data will be recorded")]
		public void ThenThePreImportDataWillBeRecorded()
		{
			var user = repos.User.GetByEmail(current.Email);

			var archive = repos.Archive
				.SingleOrDefault(a => a.User == user);

			Assert.That(archive, Is.Not.Null);

			var csvLines = csvContent.Trim().Split("\n");
			var hasConversion = csvLines[0].Contains(",Conversion,");

			Assert.That(archive.LineList.Count, Is.EqualTo(csvLines.Length - 1));

			for (var l = 0; l < archive.LineList.Count; l++)
			{
				var line = archive.LineList[l];
				var actual = CSVHelper.ToCsv(line, hasConversion);

				var expected = csvLines[l + 1];
				while (expected.EndsWith(",,,"))
					expected = expected.Substring(0, expected.Length - 3);

				Assert.That(actual, Is.EqualTo(expected));
			}
		}

		[Then(@"the lines will be queued")]
		public void ThenTheLinesWillBeQueued()
		{
			var user = repos.User.GetByEmail(current.Email);

			var archive = repos.Archive
				.SingleOrDefault(a => a.User == user);

			var lineIds = archive.LineList
				.Select(l => l.ID)
				.ToList();

			KeyValuePair<String, Line>? item;

			do
			{
				var task = queueService.Dequeue();

				task.Wait();

				item = task.Result;

				if (item.HasValue)
				{
					queueService.Delete(item.Value.Key);
					lineIds.Remove(item.Value.Value.ID);
				}
			} while (item != null);

			Assert.That(lineIds, Is.Empty);
		}

		[Then(@"the pre-import data will not be recorded")]
		public void ThenThePreImportDataWillNotBeRecorded()
		{
			var user = repos.User.GetByEmail(current.Email);

			var archive = repos.Archive
				.SingleOrDefault(a => a.User == user);

			Assert.That(archive, Is.Null);
		}

		[Then(@"the lines will not be queued")]
		public void ThenTheLinesWillNotBeQueued()
		{
			var task = queueService.Dequeue();
			task.Wait();

			var line = task.Result;

			Assert.That(line, Is.Null);
		}

		[Given("sent emails are cleared")]
		public void GivenSentEmailsAreCleared()
		{
			var inboxPath = Path.Combine(
				"..", "..", "..", "..", "..", "..", "outputs", "inbox"
			);
			var inbox = new DirectoryInfo(inboxPath);

			inbox.GetFiles("*.eml")
				.Where(f => f.CreationTimeUtc >= testStart)
				.Where(f => EmlReader.FromFile(f.FullName).Headers["To"] == userEmail)
				.ToList()
				.ForEach(e => File.Delete(e.FullName));
		}

		[Then("no email will be sent")]
		public void ThenNoEmailWillBeSent()
		{
			var inboxPath = Path.Combine(
				"..", "..", "..", "..", "..", "..", "outputs", "inbox"
			);
			var inbox = new DirectoryInfo(inboxPath);

			var emails = inbox.GetFiles("*.eml")
				.Where(f => f.CreationTimeUtc >= testStart)
				.Select(f => EmlReader.FromFile(f.FullName))
				.Where(e => e.Headers["To"] == userEmail)
				.ToList();

			Assert.That(emails.Count, Is.EqualTo(0));
		}
		#endregion

		#region MakeMoveFromLine
		[Given(@"the moves file was imported")]
		public void GivenTheMovesFileWasImported()
		{
			service.Robot.ImportMovesFile(csvName, csvContent);
		}

		[Given(@"the account (.+) is deleted")]
		public void GivenTheAccountIsDeleted(String account)
		{
			service.Admin.DeleteAccount(account.IntoUrl());
		}

		[When(@"make move from imported")]
		public void WhenMakeMoveFromImported()
		{
			try
			{
				var result = service.Robot.MakeMoveFromImported();
				result.Wait();

				moveResult = result.Result;
			}
			catch (AggregateException e)
			{
				var inner = e.InnerExceptions;

				if (inner.Count == 1 && inner[0] is CoreError realError)
				{
					error = realError;
				}
				else
				{
					throw;
				}
			}
		}

		[Then("the line status will change to (Success|Error)")]
		public void ThenTheLineStatusWillChangeTo(ImportStatus status)
		{
			var user = repos.User.GetByEmail(userEmail);
			var archive = repos.Archive.SingleOrDefault(a => a.User == user);
			var line = repos.Line.SingleOrDefault(l => l.Archive == archive);

			Assert.That(line.Status, Is.EqualTo(status));
		}

		[Then("the lines will be dequeued")]
		public void ThenTheLinesWillBeDequeued()
		{
			var task = queueService.Dequeue();
			task.Wait();
			Assert.That(task.Result, Is.Null);
		}
		#endregion

		#region FinishArchives
		[Given(@"robot made move from imported")]
		public void GivenRobotMadeMoveFromImported()
		{
			try
			{
				robotRunMakeMoves();
			}
			catch (AggregateException) { }
		}

		[When(@"finish imported archives")]
		public void WhenFinishImportedArchives()
		{
			try
			{
				service.Robot.FinishArchives();
			}
			catch (CoreError e)
			{
				error = e;
			}
		}

		[Then(@"the archive status will change to (Pending|Success|Error)")]
		public void ThenTheArchiveStatusWillChangeTo(ImportStatus status)
		{
			var user = repos.User.GetByEmail(userEmail);
			var archive = repos.Archive.SingleOrDefault(a => a.User == user);

			Assert.That(archive.Status, Is.EqualTo(status));
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
			scheduleInfo.OutUrl = accountOut?.Url;
			scheduleInfo.InUrl = accountIn?.Url;
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

				var conversion =
					row.ContainsKey("Conversion") && row["Conversion"] != ""
						? Int32.Parse(row["Conversion"])
						: default(Int32?);

				scheduleInfo = new ScheduleInfo
				{
					Description = row["Description"].ForScenario(scenarioCode),
					Nature = EnumX.Parse<MoveNature>(row["Nature"]),
					Value = Int32.Parse(row["Value"]),
					Conversion = conversion,
					Times = times,
					Boundless = Boolean.Parse(row["Boundless"]),
					Frequency = EnumX.Parse<ScheduleFrequency>(row["Frequency"]),
					ShowInstallment = Boolean.Parse(row["ShowInstallment"])
				};

				scheduleInfo.SetDate(DateTime.Parse(row["Date"]));

				if (row.ContainsKey("Category"))
					scheduleInfo.CategoryName = row["Category"];

				var scenarioAccountUrl =
					$"{mainAccountUrl}_{scenarioCode}".IntoUrl();

				if (scheduleInfo.Nature == MoveNature.Out)
				{
					scheduleInfo.OutUrl = scenarioAccountUrl;
				}
				else
				{
					if (scheduleInfo.Nature == MoveNature.Transfer)
					{
						scheduleInfo.OutUrl = accountOutUrl;
					}

					scheduleInfo.InUrl = scenarioAccountUrl;
				}

				service.Robot.SaveSchedule(scheduleInfo);
			}
		}

		[Then(@"the schedule will be disabled")]
		public void ThenTheScheduleWillBeDisabled()
		{
			var schedule = repos.Schedule.Get(scheduleInfo.Guid);
			Assert.That(schedule.Active, Is.False);
		}

		[Then(@"the schedule will be enabled")]
		public void ThenTheScheduleWillBeEnabled()
		{
			var schedule = repos.Schedule.Get(scheduleInfo.Guid);
			Assert.That(schedule.Active, Is.True);
		}

		[Then(@"the schedule last run will be (\d+)")]
		public void ThenTheScheduleLastRunWillBe(Int32 lastRun)
		{
			var schedule = repos.Schedule.Get(scheduleInfo.Guid);
			Assert.That(schedule.LastRun, Is.EqualTo(lastRun));
		}
		#endregion
	}
}
