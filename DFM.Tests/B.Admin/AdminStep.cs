using TechTalk.SpecFlow;

namespace DFM.Tests.B.Admin
{
    [Binding]
    public class AdminStep : BaseStep
    {
        #region SaveAccount
        [Given(@"I have this account to create")]
        public void GivenIHaveThisAccountToCreate(Table table)
        {
            ScenarioContext.Current.Pending();
        }

        [Given(@"I already have created this account")]
        public void GivenIAlreadyHaveCreatedThisAccount()
        {
            ScenarioContext.Current.Pending();
        }

        [When(@"I try to save the account")]
        public void WhenITryToSaveTheAccount()
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"the account will not be saved")]
        public void ThenTheAccountWillNotBeSaved()
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"the account will be saved")]
        public void ThenTheAccountWillBeSaved()
        {
            ScenarioContext.Current.Pending();
        }
        #endregion

        #region SelectAccountById
        [Given(@"I pass valid account ID")]
        public void GivenIPassValidAccountID()
        {
            ScenarioContext.Current.Pending();
        }

        [When(@"I try to get the account")]
        public void WhenITryToGetTheAccount()
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"I will receive no account")]
        public void ThenIWillReceiveNoAccount()
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"I will receive the account")]
        public void ThenIWillReceiveTheAccount()
        {
            ScenarioContext.Current.Pending();
        }
        #endregion

        #region CloseAccount
        [Given(@"I close an account")]
        public void GivenICloseAnAccount()
        {
            ScenarioContext.Current.Pending();
        }

        [Given(@"I pass its id to close again")]
        public void GivenIPassItsIdToCloseAgain()
        {
            ScenarioContext.Current.Pending();
        }

        [When(@"I try to close the account")]
        public void WhenITryToCloseTheAccount()
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"the account will not be closed")]
        public void ThenTheAccountWillNotBeClosed()
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"the account will be closed")]
        public void ThenTheAccountWillBeClosed()
        {
            ScenarioContext.Current.Pending();
        }
        #endregion

        #region DeleteAccount
        [Given(@"I delete an account")]
        public void GivenIDeleteAnAccount()
        {
            ScenarioContext.Current.Pending();
        }

        [Given(@"I pass its id to delete again")]
        public void GivenIPassItsIdToDeleteAgain()
        {
            ScenarioContext.Current.Pending();
        }

        [When(@"I try to delete the account")]
        public void WhenITryToDeleteTheAccount()
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"the account will not be deleted")]
        public void ThenTheAccountWillNotBeDeleted()
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"the account will be deleted")]
        public void ThenTheAccountWillBeDeleted()
        {
            ScenarioContext.Current.Pending();
        }
        #endregion

        #region SaveCategory
        [Given(@"I have this category to create")]
        public void GivenIHaveThisCategoryToCreate(Table table)
        {
            ScenarioContext.Current.Pending();
        }

        [Given(@"I already have created this category")]
        public void GivenIAlreadyHaveCreatedThisCategory()
        {
            ScenarioContext.Current.Pending();
        }

        [When(@"I try to save the category")]
        public void WhenITryToSaveTheCategory()
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"the category will not be saved")]
        public void ThenTheCategoryWillNotBeSaved()
        {
            ScenarioContext.Current.Pending();
        }
        
        [Then(@"the category will be saved")]
        public void ThenTheCategoryWillBeSaved()
        {
            ScenarioContext.Current.Pending();
        }
        #endregion

        #region SelectCategoryById
        [Given(@"I pass an id of Category that doesn't exist")]
        public void GivenIPassAnIdOfCategoryThatDoesnTExist()
        {
            ScenarioContext.Current.Pending();
        }

        [Given(@"I pass valid category ID")]
        public void GivenIPassValidCategoryID()
        {
            ScenarioContext.Current.Pending();
        }

        [When(@"I try to get the category")]
        public void WhenITryToGetTheCategory()
        {
            ScenarioContext.Current.Pending();
        }
        
        [Then(@"I will receive no category")]
        public void ThenIWillReceiveNoCategory()
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"I will receive the category")]
        public void ThenIWillReceiveTheCategory()
        {
            ScenarioContext.Current.Pending();
        }
        #endregion

        #region DisableCategory
        [Given(@"I disable a category")]
        public void GivenIDisableACategory()
        {
            ScenarioContext.Current.Pending();
        }

        [Given(@"I pass its id to disable again")]
        public void GivenIPassItsIdToDisableAgain()
        {
            ScenarioContext.Current.Pending();
        }

        [Given(@"I give an id of enabled category")]
        public void GivenIGiveAnIdOfAnEnabledCategory()
        {
            ScenarioContext.Current.Pending();
        }

        [When(@"I try to disable the category")]
        public void WhenITryToDisableTheCategory()
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"the category will be disabled")]
        public void ThenTheCategoryWillBeDisabled()
        {
            ScenarioContext.Current.Pending();
        }
        #endregion

        #region EnableCategory
        [Given(@"I enable a category")]
        public void GivenIEnableACategory()
        {
            ScenarioContext.Current.Pending();
        }

        [Given(@"I pass its id to enable again")]
        public void GivenIPassItsIdToEnableAgain()
        {
            ScenarioContext.Current.Pending();
        }

        [Given(@"I give an id of disabled category")]
        public void GivenIGiveAnIdOfAnDisabledCategory()
        {
            ScenarioContext.Current.Pending();
        }
        
        [When(@"I try to enable the category")]
        public void WhenITryToEnableTheCategory()
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"the category will be enabled")]
        public void ThenTheCategoryWillBeEnabled()
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

        [Given(@"I pass an id of account that doesn't exist")]
        public void GivenIPassAnIdOfAccountTheDoesnTExist()
        {
            ScenarioContext.Current.Pending();
        }

        [Given(@"I give an id of account without moves")]
        public void GivenIGiveAnIdOfAnAccountWithoutMoves()
        {
            ScenarioContext.Current.Pending();
        }

        [Given(@"I give an id of account with moves")]
        public void GivenIGiveAnIdOfAnAccountWithMoves()
        {
            ScenarioContext.Current.Pending();
        }


        [Given(@"I have a category")]
        public void GivenIHaveACategory()
        {
            ScenarioContext.Current.Pending();
        }

        [Given(@"I pass an id of category that doesn't exist")]
        public void GivenIPassAnIdOfCategoryTheDoesnTExist()
        {
            ScenarioContext.Current.Pending();
        }
        #endregion

    }
}
