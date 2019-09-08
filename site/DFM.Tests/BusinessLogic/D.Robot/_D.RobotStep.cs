using System;
using System.Collections.Generic;
using System.Linq;
using DFM.BusinessLogic.Exceptions;
using DFM.Entities;
using DFM.Entities.Enums;
using DFM.Generic;
using DFM.Tests.BusinessLogic.Helpers;
using TechTalk.SpecFlow;
using NUnit.Framework;

namespace DFM.Tests.BusinessLogic.D.Robot
{
	[Binding]
	public class RobotStep : BaseStep
	{
		#region Variables
		private static Int64 id
		{
			get { return Get<Int64>("ID"); }
			set { Set("ID", value); }
		}

		private static IList<Schedule> scheduleList
		{
			get { return Get<IList<Schedule>>("scheduleList"); }
			set { Set("scheduleList", value); }
		}
		#endregion



		#region SaveSchedule
		[Given(@"the schedule has this details")]
		public void GivenTheFutureMoveHasThisDetails(Table table)
		{
			foreach (var detailData in table.Rows)
			{
				var detail = GetDetailFromTable(detailData);

				Schedule.DetailList.Add(detail);
			}
		}

		[Given(@"I have no schedule")]
		public void GivenIHaveNoSchedule()
		{
			Schedule = null;
		}

		[When(@"I try to save the schedule")]
		public void WhenITryToSaveTheSchedule()
		{
			try
			{
				var accountOutUrl = AccountOut?.Url;
				var accountInUrl = AccountIn?.Url;

				Service.Robot.SaveOrUpdateSchedule(Schedule, accountOutUrl, accountInUrl, CategoryName);
			}
			catch (DFMCoreException e)
			{
				Error = e;
			}
		}

		[Then(@"the schedule will not be saved")]
		public void ThenTheScheduleWillNotBeSaved()
		{
			if (Schedule != null)
				Assert.AreEqual(0, Schedule.ID);
		}

		[Then(@"the schedule will be saved")]
		public void ThenTheScheduleWillBeSaved()
		{
			Assert.AreNotEqual(0, Schedule.ID);
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
				case "day": Schedule.Date = Current.User.Now().AddDays(-count); break;
				case "month": Schedule.Date = Current.User.Now().AddMonths(-count); break;
				case "year": Schedule.Date = Current.User.Now().AddYears(-count); break;
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
			catch (DFMCoreException e)
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
			catch (DFMCoreException e)
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
			catch (DFMCoreException e)
			{
				Error = e;
			}

			ConfigHelper.DeactivateMoveEmailForUser(Service);
			ConfigHelper.DeactivateEmailSystem();
		}
		#endregion

		#region DisableSchedule
		[Given(@"I pass an id of Schedule that doesn't exist")]
		public void GivenIPassAnIdOfScheduleThatDoesnTExist()
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
			catch (DFMCoreException e)
			{
				Error = e;
			}
		}

		[Then(@"the Schedule will be disabled")]
		public void ThenTheScheduleWillBeDisabled()
		{
			Error = null;

			try
			{
				Service.Robot.DisableSchedule(id);
			}
			catch (DFMCoreException e)
			{
				Error = e;
			}

			Assert.IsNotNull(Error);
			Assert.AreEqual(DfMError.DisabledSchedule, Error.Type);
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
			catch (DFMCoreException e)
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

			Schedule = new Schedule { Description = scheduleData["Description"] };

			if (!String.IsNullOrEmpty(scheduleData["Nature"]))
				Schedule.Nature = EnumX.Parse<MoveNature>(scheduleData["Nature"]);

			if (!String.IsNullOrEmpty(scheduleData["Date"]))
				Schedule.Date = DateTime.Parse(scheduleData["Date"]);

			if (!String.IsNullOrEmpty(scheduleData["Value"]))
				Schedule.Value = Int32.Parse(scheduleData["Value"]);

			if (!String.IsNullOrEmpty(scheduleData["Times"]))
				Schedule.Times = Int16.Parse(scheduleData["Times"]);

			if (!String.IsNullOrEmpty(scheduleData["Boundless"]))
				Schedule.Boundless = Boolean.Parse(scheduleData["Boundless"]);

			if (!String.IsNullOrEmpty(scheduleData["Frequency"]))
				Schedule.Frequency = EnumX.Parse<ScheduleFrequency>(scheduleData["Frequency"]);

			if (!String.IsNullOrEmpty(scheduleData["ShowInstallment"]))
				Schedule.ShowInstallment = Boolean.Parse(scheduleData["ShowInstallment"]);
		}

		[Given(@"I save the schedule")]
		public void GivenISaveTheSchedule()
		{
			var accountOutName = AccountOut?.Name;
			var accountInName = AccountIn?.Name;

			var schedule = Service.Robot.SaveOrUpdateSchedule(Schedule, accountOutName, accountInName, CategoryName);

			id = schedule.ID;
		}
		#endregion

	}
}
