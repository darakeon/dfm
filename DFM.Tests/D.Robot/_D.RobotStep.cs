using System;
using TechTalk.SpecFlow;

namespace DFM.Tests.D.Robot
{
    [Binding]
    public class RobotStep
    {
        #region SaveSchedule
        [Given(@"the move has no schedule")]
        public void GivenTheMoveHasNoSchedule()
        {
            ScenarioContext.Current.Pending();
        }

        [When(@"I try to save the schedule")]
        public void WhenITryToSaveTheMove()
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"the schedule will not be saved")]
        public void ThenTheScheduleWillNotBeSaved()
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"the schedule will be saved")]
        public void ThenTheScheduleWillBeSaved()
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"the month-category-accountOut value will not change")]
        public void ThenTheMonthCategoryAccountOutValueWillNotChange()
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"the year-category-accountOut value will not change")]
        public void ThenTheYearCategoryAccountOutValueWillNotChange()
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"the month-category-accountIn value will not change")]
        public void ThenTheMonthCategoryAccountInValueWillNotChange()
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"the year-category-accountIn value will not change")]
        public void ThenTheYearCategoryAccountInValueWillNotChange()
        {
            ScenarioContext.Current.Pending();
        }

        #endregion

        #region RunSchedule
        [Given(@"I have no logged user \(logoff\)")]
        public void GivenIHaveNoLoggedUserLogoff()
        {
            ScenarioContext.Current.Pending();
        }

        [When(@"I try to run the scheduler")]
        public void WhenITryToRunTheScheduler()
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"the user amount money will be kept")]
        public void ThenTheUserAmountMoneyWillBeKept()
        {
            ScenarioContext.Current.Pending();
        }

        [Given(@"its Date is (\d+) days ago")]
        public void GivenItsDateIsDaysAgo(Int32 days)
        {
            ScenarioContext.Current.Pending();
        }

        [Given(@"I save the move")]
        public void GivenISaveTheMove()
        {
            ScenarioContext.Current.Pending();
        }

        [When(@"I run the scheduler")]
        public void WhenIRunTheScheduler()
        {
            ScenarioContext.Current.Pending();
        }
        #endregion

        #region MoreThanOne
        [Given(@"the move has this schedule")]
        public void GivenTheMoveHasThisSchedule(Table table)
        {
            ScenarioContext.Current.Pending();
        }
        #endregion

    }
}
