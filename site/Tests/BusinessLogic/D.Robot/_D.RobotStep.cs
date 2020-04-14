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
using DFM.Tests.Util;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace DFM.BusinessLogic.Tests.D.Robot
{
	[Binding]
	public class RobotStep : BaseStep
	{
		#region Variables
		private static Int64 id
		{
			get => get<Int64>("ID");
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

					var schedule = service.Robot.SaveSchedule(scheduleInfo);
					scheduleInfo.ID = schedule.ID;
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
				Assert.AreEqual(0, scheduleInfo.ID);
		}

		[Then(@"the schedule will be saved")]
		public void ThenTheScheduleWillBeSaved()
		{
			Assert.AreNotEqual(0, scheduleInfo.ID);
		}
		#endregion

		#region RunSchedule
		[Given(@"I run the scheduler to cleanup older tests")]
		public void GivenIRunTheSchedulerToCleanupOlderTests()
		{
			service.Robot.RunSchedule();
		}

		[Given(@"I have no logged user \(logoff\)")]
		public void GivenIHaveNoLoggedUserLogoff()
		{
			current.Clear();
		}

		[Given(@"its Date is (\-?\d+\.?\d*) (day|month|year)s? ago")]
		public void GivenItsDateIsDaysAgo(Int32 count, String frequency)
		{
			scheduleInfo.SetDate(current.Now);
			scheduleInfo.AddByFrequency(frequency, -count);
		}

		[Given(@"I run the scheduler")]
		[When(@"I try to run the scheduler")]
		public void WhenITryToRunTheScheduler()
		{
			try
			{
				service.Robot.RunSchedule();
			}
			catch (CoreError e)
			{
				if (isCurrent(ScenarioBlock.When))
					error = e;
				else
					throw;
			}
		}

		[When(@"I try to run the scheduler with e-mail system out")]
		public void WhenITryToRunTheSchedulerWithEMailSystemOut()
		{
			ConfigHelper.ActivateMoveEmailForUser(service);
			ConfigHelper.BreakTheEmailSystem();

			try
			{
				currentEmailStatus = service.Robot.RunSchedule();
			}
			catch (CoreError e)
			{
				error = e;
			}

			ConfigHelper.FixTheEmailSystem();
			ConfigHelper.DeactivateMoveEmailForUser(service);
		}

		[When(@"I try to run the scheduler with e-mail system ok")]
		public void WhenITryToRunTheSchedulerWithEMailSystemOk()
		{
			ConfigHelper.ActivateMoveEmailForUser(service);

			try
			{
				currentEmailStatus = service.Robot.RunSchedule();
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
			id = 0;
		}

		[Given(@"I already have disabled the Schedule")]
		public void GivenIAlreadyHaveDisabledTheSchedule()
		{
			service.Robot.DisableSchedule(id);
		}

		[When(@"I try to disable the Schedule")]
		public void WhenITryToDisableTheSchedule()
		{
			try
			{
				service.Robot.DisableSchedule(id);
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
			service.Robot.DisableSchedule(id);
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

			id = schedule.ID;
			scheduleInfo.ID = schedule.ID;
		}

		[Then(@"the schedule will be disabled")]
		public void ThenTheScheduleWillBeDisabled()
		{
			var schedule = scheduleRepository.Get(scheduleInfo.ID);
			Assert.IsFalse(schedule.Active);
		}

		[Then(@"the schedule will be enabled")]
		public void ThenTheScheduleWillBeEnabled()
		{
			var schedule = scheduleRepository.Get(scheduleInfo.ID);
			Assert.IsTrue(schedule.Active);
		}

		[Then(@"the schedule last run will be (\d+)")]
		public void ThenTheScheduleLastRunWillBe(Int32 lastRun)
		{
			var schedule = scheduleRepository.Get(scheduleInfo.ID);
			Assert.AreEqual(lastRun, schedule.LastRun);
		}
		#endregion

	}
}
