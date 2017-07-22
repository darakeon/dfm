using System;
using TechTalk.SpecFlow;

namespace DFM.Tests.C.Money
{
    [Binding]
    public class MoneyStep
    {
        #region SaveMove
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


    }
}
