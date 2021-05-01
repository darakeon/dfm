using System;
using System.Collections.Generic;
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
using Keon.Util.Extensions;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace DFM.BusinessLogic.Tests.D.Robot
{
	[Binding]
	public class RobotStep : BaseStep
	{
		#region Variables
		private static Guid guid
		{
			get => get<Guid>("ID");
			set => set("ID", value);
		}

		private static IList<ScheduleInfo> scheduleList
		{
			get => get<IList<ScheduleInfo>>("scheduleList");
			set => set("scheduleList", value);
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

		[Given(@"(.+) is a robot")]
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
			ConfigHelper.ActivateMoveEmailForUser(service);
			ConfigHelper.BreakTheEmailSystem();

			try
			{
				robotRunSchedule();
			}
			catch (CoreError e)
			{
				error = e;
			}

			ConfigHelper.FixTheEmailSystem();
			ConfigHelper.DeactivateMoveEmailForUser(service);
		}

		[When(@"run the scheduler with e-mail system ok")]
		public void WhenITryToRunTheSchedulerWithEMailSystemOk()
		{
			ConfigHelper.ActivateMoveEmailForUser(service);

			try
			{
				robotRunSchedule();
			}
			catch (CoreError e)
			{
				error = e;
			}

			ConfigHelper.DeactivateMoveEmailForUser(service);
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

			var users = repos.User.Where(
				u => u.Email != userEmail
			);

			db.Execute(() => {
				foreach (var user in users)
				{
					repos.Acceptance.Accept(user, contract);
				}
			});
		}

		[Given(@"the user creation was (\d+) days before")]
		public void GivenTheUserCreationWas(Int32 days)
		{	
			var user = repos.User.GetByEmail(userEmail);
			var control = user.Control;
			control.Creation = DateTime.UtcNow.AddDays(-days);

			db.Execute(() => repos.Control.SaveOrUpdate(control));
		}

		[When(@"robot cleanup abandoned users")]
		public void WhenRobotCleanupAbandonedUsers()
		{
			try
			{
				service.Robot.CleanupAbandonedUsers();
			}
			catch (CoreError e)
			{
				error = e;
			}
		}

		[Then(@"the user will still exist")]
		public void ThenTheUserWillStillExists()
		{
			var user = repos.User.GetByEmail(userEmail);
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
			var actualCount = Email.CountEmails(
				userEmail,
				EmailType.RemovalReason,
				testStart
			);
			Assert.AreEqual(expectedCount, actualCount);
		}

		[Then(@"and the user warning count will be (\d+)")]
		public void ThenAndTheUserWarningCountWillBe(Int32 count)
		{
			var user = repos.User.GetByEmail(userEmail);
			Assert.AreEqual(count, user.Control.RemovalWarningSent);
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
