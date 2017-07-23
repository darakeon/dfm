using System;
using DFM.BusinessLogic.Exceptions;
using DFM.Entities;
using DFM.Entities.Enums;
using DFM.Generic;
using TechTalk.SpecFlow;
using NUnit.Framework;

namespace DFM.Tests.D.Robot
{
    [Binding]
    public class RobotStep : BaseStep
    {
        #region Variables
        protected Schedule Schedule
        {
            get { return Get<Schedule>("Schedule"); }
            set { Set("Schedule", value); }
        }
        #endregion

        #region SaveSchedule
        [Given(@"I have this future move to create")]
        public void GivenIHaveThisMoveToCreate(Table table)
        {
            var moveData = table.Rows[0];

            Move = new FutureMove { Description = moveData["Description"] };

            if (!String.IsNullOrEmpty(moveData["Nature"]))
                Move.Nature = EnumX.Parse<MoveNature>(moveData["Nature"]);

            if (!String.IsNullOrEmpty(moveData["Date"]))
                Move.Date = DateTime.Parse(moveData["Date"]);

            // TODO: use this, delete above
            //if (moveData["Value"] != null)
            //    move.Value = Int32.Parse(moveData["Value"]);

            if (!String.IsNullOrEmpty(moveData["Value"]))
            {
                var detail = new Detail
                {
                    Description = Move.Description,
                    Amount = 1,
                    Value = Int32.Parse(moveData["Value"])
                };

                Move.DetailList.Add(detail);
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
                SA.Robot.SaveOrUpdateSchedule((FutureMove)Move, AccountOut, AccountIn, MoveCategory, Schedule);
            }
            catch (DFMCoreException e)
            {
                Error = e;
            }
        }

        [Then(@"the schedule will not be saved")]
        public void ThenTheScheduleWillNotBeSaved()
        {
            Assert.AreEqual(0, Move.ID);
        }

        [Then(@"the schedule will be saved")]
        public void ThenTheScheduleWillBeSaved()
        {
            Assert.AreNotEqual(0, Move.ID);
            Assert.AreNotEqual(0, Move.Schedule.ID);
        }

        [Then(@"the month-category-accountOut value will not change")]
        public void ThenTheMonthCategoryAccountOutValueWillNotChange()
        {
            AccountOut = GetOrCreateAccount(AccountOutName);

            var year = AccountOut[Move.Date.Year] ?? new Year();
            var month = year[Move.Date.Month] ?? new Month();

            var currentTotal = (month[Category.Name] ?? new Summary()).Out;

            Assert.AreEqual(MonthCategoryAccountOutTotal, currentTotal);
        }

        [Then(@"the year-category-accountOut value will not change")]
        public void ThenTheYearCategoryAccountOutValueWillNotChange()
        {
            AccountOut = GetOrCreateAccount(AccountOutName);

            var year = AccountOut[Move.Date.Year] ?? new Year();

            var currentTotal = (year[Category.Name] ?? new Summary()).Out;

            Assert.AreEqual(YearCategoryAccountOutTotal, currentTotal);
        }

        [Then(@"the month-category-accountIn value will not change")]
        public void ThenTheMonthCategoryAccountInValueWillNotChange()
        {
            AccountIn = GetOrCreateAccount(AccountInName);

            var year = AccountIn[Move.Date.Year] ?? new Year();
            var month = year[Move.Date.Month] ?? new Month();

            var currentTotal = (month[Category.Name] ?? new Summary()).In;

            Assert.AreEqual(MonthCategoryAccountInTotal, currentTotal);
        }

        [Then(@"the year-category-accountIn value will not change")]
        public void ThenTheYearCategoryAccountInValueWillNotChange()
        {
            AccountIn = GetOrCreateAccount(AccountInName);

            var year = AccountIn[Move.Date.Year] ?? new Year();

            var currentTotal = (year[Category.Name] ?? new Summary()).In;

            Assert.AreEqual(YearCategoryAccountInTotal, currentTotal);
        }

        #endregion

        #region RunSchedule
        [Given(@"I run the scheduler to cleanup older tests")]
        public void GivenIRunTheSchedulerToCleanupOlderTests()
        {
            SA.Robot.RunSchedule(User);
        }
        
        [Given(@"I have no logged user \(logoff\)")]
        public void GivenIHaveNoLoggedUserLogoff()
        {
            //TODO: change to logoff
            User = null;
        }

        [Given(@"its Date is (\d+) (\w+) ago")]
        public void GivenItsDateIsDaysAgo(Int32 count, String frequency)
        {
            switch (frequency)
            {
                case "days": Move.Date = DateTime.Today.AddDays(-count); break;
                case "months": Move.Date = DateTime.Today.AddMonths(-count); break;
                case "years": Move.Date = DateTime.Today.AddYears(-count); break;
            }
        }

        [Given(@"I save the move")]
        public void GivenISaveTheMove()
        {
            SA.Robot.SaveOrUpdateSchedule((FutureMove)Move, AccountOut, AccountIn, MoveCategory, Schedule);
        }

        [When(@"I try to run the scheduler")]
        public void WhenITryToRunTheScheduler()
        {
            try
            {
                SA.Robot.RunSchedule(User);
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

            Schedule = new Schedule();

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
