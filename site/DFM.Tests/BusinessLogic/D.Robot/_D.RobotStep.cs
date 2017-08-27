using System;
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
        private static Int32 id
        {
            get { return Get<Int32>("ID"); }
            set { Set("ID", value); }
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
                var accountOutUrl = AccountOut == null ? null : AccountOut.Url;
                var accountInUrl = AccountIn == null ? null : AccountIn.Url;

                SA.Robot.SaveOrUpdateSchedule(Schedule, accountOutUrl, accountInUrl, CategoryName);
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
            SA.Robot.RunSchedule();
        }
        
        [Given(@"I have no logged user \(logoff\)")]
        public void GivenIHaveNoLoggedUserLogoff()
        {
            Current.Clean();
        }

        [Given(@"its Date is (\-?\d+\.?\d*) (\w+) ago")]
        public void GivenItsDateIsDaysAgo(Int32 count, String frequency)
        {
            switch (frequency)
            {
                case "day": case "days": Schedule.Date = Current.User.Now().AddDays(-count); break;
                case "month":case "months": Schedule.Date = Current.User.Now().AddMonths(-count); break;
                case "year": case "years": Schedule.Date = Current.User.Now().AddYears(-count); break;
            }
        }

        [When(@"I try to run the scheduler")]
        public void WhenITryToRunTheScheduler()
        {
            try
            {
                SA.Robot.RunSchedule();
            }
            catch (DFMCoreException e)
            {
                Error = e;
            }
        }

        [When(@"I try to run the scheduler with e-mail system out")]
        public void WhenITryToRunTheSchedulerWithEMailSystemOut()
        {
            ConfigHelper.ActivateMoveEmailForUser(SA);
            ConfigHelper.BreakTheEmailSystem();

            try
            {
                CurrentEmailStatus = SA.Robot.RunSchedule();
            }
            catch (DFMCoreException e)
            {
                Error = e;
            }

            ConfigHelper.FixTheEmailSystem();
            ConfigHelper.DeactivateMoveEmailForUser(SA);
        }

        [When(@"I try to run the scheduler with e-mail system ok")]
        public void WhenITryToRunTheSchedulerWithEMailSystemOk()
        {
            ConfigHelper.ActivateEmailSystem();
            ConfigHelper.ActivateMoveEmailForUser(SA);

            try
            {
                CurrentEmailStatus = SA.Robot.RunSchedule();
            }
            catch (DFMCoreException e)
            {
                Error = e;
            }

            ConfigHelper.DeactivateMoveEmailForUser(SA);
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
            SA.Robot.DisableSchedule(id);
        }

        [When(@"I try to disable the Schedule")]
        public void WhenITryToDisableTheSchedule()
        {
            try
            {
                SA.Robot.DisableSchedule(id);
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
                SA.Robot.DisableSchedule(id);
            }
            catch (DFMCoreException e)
            {
                Error = e;
            }

            Assert.IsNotNull(Error);
            Assert.AreEqual(ExceptionPossibilities.DisabledSchedule, Error.Type);
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
            var accountOutName = AccountOut == null ? null : AccountOut.Name;
            var accountInName = AccountIn == null ? null : AccountIn.Name;

            var schedule = SA.Robot.SaveOrUpdateSchedule(Schedule, accountOutName, accountInName, CategoryName);

            id = schedule.ID;
        }
        #endregion

    }
}
