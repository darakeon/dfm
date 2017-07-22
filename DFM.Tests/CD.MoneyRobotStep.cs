using System;
using TechTalk.SpecFlow;

namespace DFM.Tests
{
    [Binding]
    public class MoneyRobotStep
    {
        [Given(@"I have two accounts")]
        public void GivenIHaveTwoAccounts()
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

    }
}
