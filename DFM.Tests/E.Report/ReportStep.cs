using TechTalk.SpecFlow;

namespace DFM.Tests.E.Report
{
    [Binding]
    public class ReportStep
    {
        #region GetMonthReport
        [When(@"I try to get the month report")]
        public void WhenITryToGetTheMonthReport()
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"I will receive no month report")]
        public void ThenIWillReceiveNoMonthReport()
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"I will receive the month report")]
        public void ThenIWillReceiveTheMonthReport()
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"its sum value will be equal to its moves sum value")]
        public void ThenItsSumValueWillBeEqualToItsMovesSumValue()
        {
            ScenarioContext.Current.Pending();
        }
        #endregion

        #region GetYearReport
        [When(@"I try to get the year report")]
        public void WhenITryToGetTheYearReport()
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"I will receive no year report")]
        public void ThenIWillReceiveNoYearReport()
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"I will receive the year report")]
        public void ThenIWillReceiveTheYearReport()
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"its sum value will be equal to its months sum value")]
        public void ThenItsSumValueWillBeEqualToItsMonthsSumValue()
        {
            ScenarioContext.Current.Pending();
        }
        #endregion



        #region MoreThanOne
        [Given(@"I have an account")]
        public void GivenIHaveAnAccount()
        {
            ScenarioContext.Current.Pending();
        }

        [Given(@"I have moves of")]
        public void GivenIHaveMovesOf(Table table)
        {
            ScenarioContext.Current.Pending();
        }

        [Given(@"I pass an invalid account ID")]
        public void GivenIPassAnInvalidAccountID()
        {
            ScenarioContext.Current.Pending();
        }

        [Given(@"I pass a valid account ID")]
        public void GivenIPassAValidAccountID()
        {
            ScenarioContext.Current.Pending();
        }

        [Given(@"I pass this date")]
        public void GivenIPassThisDate(Table table)
        {
            ScenarioContext.Current.Pending();
        }

        #endregion


    }
}
