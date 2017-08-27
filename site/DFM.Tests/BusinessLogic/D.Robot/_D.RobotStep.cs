using System;
using DFM.BusinessLogic.Exceptions;
using DFM.Entities;
using DFM.Entities.Enums;
using DFM.Entities.Extensions;
using DFM.Generic;
using TechTalk.SpecFlow;
using NUnit.Framework;

namespace DFM.Tests.BusinessLogic.D.Robot
{
    [Binding]
    public class RobotStep : BaseStep
    {
        #region SaveSchedule
        [Given(@"I have this future move to create")]
        public void GivenIHaveThisMoveToCreate(Table table)
        {
            var moveData = table.Rows[0];

            Schedule = new Schedule { Description = moveData["Description"] };

            if (!String.IsNullOrEmpty(moveData["Nature"]))
                Schedule.Nature = EnumX.Parse<MoveNature>(moveData["Nature"]);

            if (!String.IsNullOrEmpty(moveData["Date"]))
                Schedule.Date = DateTime.Parse(moveData["Date"]);

            // TODO: use this, delete above
            //if (moveData["Value"] != null)
            //    move.Value = Int32.Parse(moveData["Value"]);

            if (!String.IsNullOrEmpty(moveData["Value"]))
            {
                var detail = new Detail
                {
                    Description = Schedule.Description,
                    Amount = 1,
                    Value = Int32.Parse(moveData["Value"])
                };

                Schedule.DetailList.Add(detail);
            }
        }

        [Given(@"the future move has this details")]
        public void GivenTheFutureMoveHasThisDetails(Table table)
        {
            foreach (var detailData in table.Rows)
            {
                var detail = GetDetailFromTable(detailData);

                Schedule.DetailList.Add(detail);
            }
        }

        [Given(@"the move has no schedule")]
        public void GivenTheMoveHasNoSchedule()
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

        [Given(@"its Date is (\-?\d+) (\w+) ago")]
        public void GivenItsDateIsDaysAgo(Int32 count, String frequency)
        {
            switch (frequency)
            {
                case "day": case "days": Schedule.Date = Current.User.Now().AddDays(-count); break;
                case "month":case "months": Schedule.Date = Current.User.Now().AddMonths(-count); break;
                case "year": case "years": Schedule.Date = Current.User.Now().AddYears(-count); break;
            }
        }

        [Given(@"I save the schedule")]
        public void GivenISaveTheSchedule()
        {
            var accountOutName = AccountOut == null ? null : AccountOut.Name;
            var accountInName = AccountIn == null ? null : AccountIn.Name;

            SA.Robot.SaveOrUpdateSchedule(Schedule, accountOutName, accountInName, CategoryName);
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
        #endregion

        #region MoreThanOne
        [Given(@"the move has this schedule")]
        public void GivenTheMoveHasThisSchedule(Table table)
        {
            var scheduleData = table.Rows[0];

            if (!String.IsNullOrEmpty(scheduleData["Times"]))
                Schedule.Times = Int16.Parse(scheduleData["Times"]);

            if (!String.IsNullOrEmpty(scheduleData["Boundless"]))
                Schedule.Boundless = Boolean.Parse(scheduleData["Boundless"]);

            if (!String.IsNullOrEmpty(scheduleData["Frequency"]))
                Schedule.Frequency = EnumX.Parse<ScheduleFrequency>(scheduleData["Frequency"]);

            if (!String.IsNullOrEmpty(scheduleData["ShowInstallment"]))
                Schedule.ShowInstallment = Boolean.Parse(scheduleData["ShowInstallment"]);
        }
        #endregion

    }
}
