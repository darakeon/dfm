using System;
using TechTalk.SpecFlow;

namespace DFM.Tests.Steps
{
    [Binding]
    public class MoneyStep
    {
        #region SaveMove
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

        [Then(@"the move will not be saved")]
        public void ThenTheMoveWillNotBeSaved()
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"the move will be saved")]
        public void ThenTheMoveWillBeSaved()
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

        [Then(@"the month-category-accountIn value will increase in (\d+)")]
        public void ThenTheMonthCategoryAccountInValueWillIncreaseIn(Double increase)
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"the year-category-accountIn value will increase in (\d+)")]
        public void ThenTheYearCategoryAccountInValueWillIncreaseIn(Double increase)
        {
            ScenarioContext.Current.Pending();
        }
        #endregion

        #region SelectMoveById
        [When(@"I try to get the move")]
        public void WhenITryToGetTheMove()
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"I will receive no move")]
        public void ThenIWillReceiveNoMove()
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"I will receive the move")]
        public void ThenIWillReceiveTheMove()
        {
            ScenarioContext.Current.Pending();
        }
        #endregion

        #region SelectDetailById
        [Given(@"I have a detail")]
        public void GivenIHaveADetail()
        {
            ScenarioContext.Current.Pending();
        }

        [Given(@"I pass an id od Detail that doesn't exist")]
        public void GivenIPassAnIdOdDetailThatDoesnTExist()
        {
            ScenarioContext.Current.Pending();
        }

        [Given(@"I pass valid Detail ID")]
        public void GivenIPassValidDetailID()
        {
            ScenarioContext.Current.Pending();
        }

        [When(@"I try to get the detail")]
        public void WhenITryToGetTheDetail()
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"I will receive no detail")]
        public void ThenIWillReceiveNoDetail()
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"I will receive the detail")]
        public void ThenIWillReceiveTheDetail()
        {
            ScenarioContext.Current.Pending();
        }
        #endregion

        #region DeleteMove
        [When(@"I try to delete the move")]
        public void WhenITryToDeleteTheMove()
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"the move will be deleted")]
        public void ThenTheMoveWillBeDeleted()
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"the move will not be deleted")]
        public void ThenTheMoveWillNotBeDeleted()
        {
            ScenarioContext.Current.Pending();
        }
        #endregion



        #region MoreThanOne
        [Given(@"I have a move")]
        public void GivenIHaveAMove()
        {
            ScenarioContext.Current.Pending();
        }

        [Given(@"I pass an id of Move that doesn't exist")]
        public void GivenIPassAnIdOfMoveThatDoesnTExist()
        {
            ScenarioContext.Current.Pending();
        }

        [Given(@"I pass valid Move ID")]
        public void GivenIPassValidMoveID()
        {
            ScenarioContext.Current.Pending();
        }
        #endregion


        #region
        #endregion

    }
}
