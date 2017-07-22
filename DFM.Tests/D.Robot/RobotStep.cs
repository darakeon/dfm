using System;
using TechTalk.SpecFlow;

namespace DFM.Tests.D.Robot
{
    [Binding]
    public class RobotStep
    {
        /*
        #region SaveSchedule
        [Given(@"I have two accounts")]
        public void GivenIHaveTwoAccounts()
        {
            ScenarioContext.Current.Pending();
        }

        [Given(@"I have a category")]
        public void GivenIHaveACategory()
        {
            ScenarioContext.Current.Pending();
        }

        [Given(@"I have this move to create")]
        public void GivenIHaveThisMoveToCreate(Table table)
        {
            ScenarioContext.Current.Pending();
        }

        [Given(@"it has no Details")]
        public void GivenItHasNoDetails()
        {
            ScenarioContext.Current.Pending();
        }

        [Given(@"the move has this details")]
        public void GivenTheMoveHasThisDetails(Table table)
        {
            ScenarioContext.Current.Pending();
        }

        [Given(@"the move has no schedule")]
        public void GivenTheMoveHasNoSchedule()
        {
            ScenarioContext.Current.Pending();
        }

        [Given(@"the move has this schedule")]
        public void GivenTheMoveHasThisSchedule(Table table)
        {
            ScenarioContext.Current.Pending();
        }

        [Given(@"it has a Category")]
        public void GivenItHasACategory()
        {
            ScenarioContext.Current.Pending();
        }

        [Given(@"it has no Category")]
        public void GivenItHasNoCategory()
        {
            ScenarioContext.Current.Pending();
        }

        [Given(@"it has an unknown Category")]
        public void GivenItHasAnUnknownCategory()
        {
            ScenarioContext.Current.Pending();
        }

        [Given(@"it has an Account Out")]
        public void GivenItHasAnAccountOut()
        {
            ScenarioContext.Current.Pending();
        }

        [Given(@"it has no Account Out")]
        public void GivenItHasNoAccountOut()
        {
            ScenarioContext.Current.Pending();
        }

        [Given(@"it has an unknown Account Out")]
        public void GivenItHasAnUnknownAccountOut()
        {
            ScenarioContext.Current.Pending();
        }

        [Given(@"it has an Account In")]
        public void GivenItHasAnAccountIn()
        {
            ScenarioContext.Current.Pending();
        }

        [Given(@"it has no Account In")]
        public void GivenItHasNoAccountIn()
        {
            ScenarioContext.Current.Pending();
        }

        [Given(@"it has an unknown Account In")]
        public void GivenItHasAnUnknownAccountIn()
        {
            ScenarioContext.Current.Pending();
        }

        [Given(@"it has an Account In equal to Out")]
        public void GivenItHasAnAccountInEqualToOut()
        {
            ScenarioContext.Current.Pending();
        }

        [When(@"I try to save the move")]
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
        */

        #region RunSchedule
        [Given(@"I have two accounts")]
        public void GivenIHaveTwoAccounts()
        {
            ScenarioContext.Current.Pending();
        }

        [Given(@"I have a category")]
        public void GivenIHaveACategory()
        {
            ScenarioContext.Current.Pending();
        }

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

        [Given(@"I have this move to create")]
        public void GivenIHaveThisMoveToCreate(Table table)
        {
            ScenarioContext.Current.Pending();
        }

        [Given(@"it has no Details")]
        public void GivenItHasNoDetails()
        {
            ScenarioContext.Current.Pending();
        }

        [Given(@"the move has this details")]
        public void GivenTheMoveHasThisDetails(Table table)
        {
            ScenarioContext.Current.Pending();
        }

        [Given(@"the move has this schedule")]
        public void GivenTheMoveHasThisSchedule(Table table)
        {
            ScenarioContext.Current.Pending();
        }

        [Given(@"it has a Category")]
        public void GivenItHasACategory()
        {
            ScenarioContext.Current.Pending();
        }

        [Given(@"it has an Account Out")]
        public void GivenItHasAnAccountOut()
        {
            ScenarioContext.Current.Pending();
        }

        [Given(@"it has no Account In")]
        public void GivenItHasNoAccountIn()
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

        [Then(@"the month-category-accountOut value will decrease in (\d+)")]
        public void ThenTheMonthCategoryAccountOutValueWillDecreaseIn(Double decrease)
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"the year-category-accountOut value will decrease in (\d+)")]
        public void ThenTheYearCategoryAccountOutValueWillDecreaseIn(Double decrease)
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"the month-category-accountOut value will decrease in (\d+) plus the months until now")]
        public void ThenTheMonthCategoryAccountOutValueWillDecreaseInPlusTheMonthsUntilNow(Double decrease)
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"the year-category-accountOut value will decrease in (\d+) plus the months until now")]
        public void ThenTheYearCategoryAccountOutValueWillDecreaseInPlusTheMonthsUntilNow(Double decrease)
        {
            ScenarioContext.Current.Pending();
        }



        #endregion

        #region
        #endregion



        #region
        #endregion

    }
}
