using System;
using System.Collections.Generic;
using System.Linq;
using DFM.BusinessLogic.Exceptions;
using DFM.BusinessLogic.Response;
using DFM.Entities;
using DFM.Entities.Enums;
using DFM.Generic;
using DFM.Tests.BusinessLogic.Helpers;
using TechTalk.SpecFlow;
using NUnit.Framework;
using error = DFM.BusinessLogic.Exceptions.Error;

namespace DFM.Tests.BusinessLogic.D.Robot
{
	[Binding]
	public class RobotStep : BaseStep
	{
		#region Variables
		private static Int64 id
		{
			get => Get<Int64>("ID");
			set => Set("ID", value);
		}

		private static IList<ScheduleInfo> scheduleList
		{
			get => Get<IList<ScheduleInfo>>("scheduleList");
			set => Set("scheduleList", value);
		}
		#endregion



		#region SaveSchedule
		[Given(@"the schedule has this details")]
		public void GivenTheFutureMoveHasThisDetails(Table table)
		{
			foreach (var detailData in table.Rows)
			{
				var detail = GetDetailFromTable(detailData);
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
				if (scheduleInfo != null)
				{
					scheduleInfo.OutUrl = AccountOut?.Url;
					scheduleInfo.InUrl = AccountIn?.Url;
					scheduleInfo.CategoryName = CategoryName;
				}

				var schedule = Service.Robot.SaveSchedule(scheduleInfo);
				scheduleInfo.ID = schedule.ID;
			}
			catch (CoreError e)
			{
				Error = e;
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
			Service.Robot.RunSchedule();
		}

		[Given(@"I have no logged user \(logoff\)")]
		public void GivenIHaveNoLoggedUserLogoff()
		{
			Current.Clear();
		}

		[Given(@"its Date is (\-?\d+\.?\d*) (day|month|year)s? ago")]
		public void GivenItsDateIsDaysAgo(Int32 count, String frequency)
		{
			switch (frequency)
			{
				case "day": scheduleInfo.Date = Current.User.Now().AddDays(-count); break;
				case "month": scheduleInfo.Date = Current.User.Now().AddMonths(-count); break;
				case "year": scheduleInfo.Date = Current.User.Now().AddYears(-count); break;
			}
		}

		[Given(@"I run the scheduler")]
		[When(@"I try to run the scheduler")]
		public void WhenITryToRunTheScheduler()
		{
			try
			{
				Service.Robot.RunSchedule();
			}
			catch (CoreError e)
			{
				if (IsCurrent(ScenarioBlock.When))
					Error = e;
				else
					throw;
			}
		}

		[When(@"I try to run the scheduler with e-mail system out")]
		public void WhenITryToRunTheSchedulerWithEMailSystemOut()
		{
			ConfigHelper.ActivateMoveEmailForUser(Service);
			ConfigHelper.BreakTheEmailSystem();

			try
			{
				CurrentEmailStatus = Service.Robot.RunSchedule();
			}
			catch (CoreError e)
			{
				Error = e;
			}

			ConfigHelper.FixTheEmailSystem();
			ConfigHelper.DeactivateMoveEmailForUser(Service);
		}

		[When(@"I try to run the scheduler with e-mail system ok")]
		public void WhenITryToRunTheSchedulerWithEMailSystemOk()
		{
			ConfigHelper.ActivateEmailSystem();
			ConfigHelper.ActivateMoveEmailForUser(Service);

			try
			{
				CurrentEmailStatus = Service.Robot.RunSchedule();
			}
			catch (CoreError e)
			{
				Error = e;
			}

			ConfigHelper.DeactivateMoveEmailForUser(Service);
			ConfigHelper.DeactivateEmailSystem();
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
			Service.Robot.DisableSchedule(id);
		}

		[When(@"I try to disable the Schedule")]
		public void WhenITryToDisableTheSchedule()
		{
			try
			{
				Service.Robot.DisableSchedule(id);
			}
			catch (CoreError e)
			{
				Error = e;
			}
		}
		#endregion

		#region GetScheduleList
		[Given(@"I disable the schedule")]
		public void GivenICloseTheSchedule()
		{
			Service.Robot.DisableSchedule(id);
		}

		[When(@"ask for the schedule list")]
		public void WhenAskForAllTheScheduleList()
		{
			try
			{
				scheduleList = Service.Robot.GetScheduleList();
			}
			catch (CoreError e)
			{
				Error = e;
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
				scheduleInfo.Date = DateTime.Parse(scheduleData["Date"]);

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
			scheduleInfo.OutUrl = AccountOut?.Name;
			scheduleInfo.InUrl = AccountIn?.Name;
			scheduleInfo.CategoryName = CategoryName;

			var schedule = Service.Robot.SaveSchedule(scheduleInfo);

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
