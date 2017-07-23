using System;
using DFM.BusinessLogic.Exceptions;
using DFM.Entities;
using DFM.Entities.Enums;
using DFM.Entities.Extensions;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace DFM.Tests.BusinessLogic.B.Admin
{
    [Binding]
    public class AdminStep : BaseStep
    {
        #region Variables
        private static String name
        {
            get { return Get<String>("name"); }
            set { Set("name", value); }
        }

        private static String accountName
        {
            get { return Get<String>("AccountName"); }
            set { Set("AccountName", value); }
        }

        private static Account olderAccount
        {
            get { return Get<Account>("OlderAccount"); }
            set { Set("OlderAccount", value); }
        }

        private static String categoryName
        {
            get { return Get<String>("CategoryName"); }
            set { Set("CategoryName", value); }
        }

        private static Category olderCategory
        {
            get { return Get<Category>("OlderCategory"); }
            set { Set("OlderCategory", value); }
        }
        #endregion

        #region SaveAccount
        [Given(@"I have this account to create")]
        public void GivenIHaveThisAccountToCreate(Table table)
        {
            var accountData = table.Rows[0];

            Account = new Account
                          {
                              Name = accountData["Name"],
                              RedLimit = GetInt(accountData["Red"]),
                              YellowLimit = GetInt(accountData["Yellow"]),
                              User = User
                          };
        }

        [Given(@"I already have this account")]
        public void GivenIAlreadyHaveThisAccount(Table table)
        {
            var accountData = table.Rows[0];

            olderAccount = new Account
            {
                Name = accountData["Name"],
                RedLimit = GetInt(accountData["Red"]),
                YellowLimit = GetInt(accountData["Yellow"]),
                User = User
            };

            SA.Admin.SaveOrUpdateAccount(olderAccount);
        }

        [When(@"I try to save the account")]
        public void WhenITryToSaveTheAccount()
        {
            try
            {
                SA.Admin.SaveOrUpdateAccount(Account);
            }
            catch (DFMCoreException e)
            {
                Error = e;
            }
        }

        [Then(@"the account will not be changed")]
        public void ThenTheAccountWillNotBeChanged()
        {
            var account = SA.Admin.SelectAccountByName(olderAccount.Name);

            Assert.AreEqual(olderAccount.Name, account.Name);
            Assert.AreEqual(olderAccount.RedLimit, account.RedLimit);
            Assert.AreEqual(olderAccount.YellowLimit, account.YellowLimit);

            Assert.AreNotEqual(Account.RedLimit, account.RedLimit);
            Assert.AreNotEqual(Account.YellowLimit, account.YellowLimit);
        }

        [Then(@"the account will not be saved")]
        public void ThenTheAccountWillNotBeSaved()
        {
            Error = null;

            try
            {
                SA.Admin.SelectAccountByName(Account.Name);
            }
            catch (DFMCoreException e)
            {
                Error = e;
            }

            Assert.IsNotNull(Error);
            Assert.AreEqual(ExceptionPossibilities.InvalidAccount, Error.Type);
        }

        [Then(@"the account will be saved")]
        public void ThenTheAccountWillBeSaved()
        {
            Account = SA.Admin.SelectAccountByName(Account.Name);

            Assert.IsNotNull(Account);
        }
        #endregion

        #region SelectAccountByName
        [Given(@"I pass a name of account that doesn't exist")]
        public void GivenIPassAnNameOfAccountThatDoesnTExist()
        {
            accountName = "Invalid account";
        }

        [Given(@"I pass valid account Name")]
        public void GivenIPassValidAccountName()
        {
            accountName = Account.Name;
        }

        [When(@"I try to get the account by its Name")]
        public void WhenITryToGetTheAccountByItsName()
        {
            Account = null;

            try
            {
                Account = SA.Admin.SelectAccountByName(accountName);
            }
            catch (DFMCoreException e)
            {
                Error = e;
            }
        }
        #endregion

        #region SelectAccountById
        [Given(@"I pass valid account ID")]
        public void GivenIPassValidAccountID()
        {
            name = Account.Name;
        }

        [When(@"I try to get the account")]
        public void WhenITryToGetTheAccount()
        {
            Account = null;

            try
            {
                Account = SA.Admin.SelectAccountByName(name);
            }
            catch (DFMCoreException e)
            {
                Error = e;
            }
        }
        #endregion

        #region CloseAccount
        [Given(@"I already have closed the account")]
        public void GivenICloseTheAccount()
        {
            SA.Admin.CloseAccount(name);
        }

        [When(@"I try to close the account")]
        public void WhenITryToCloseTheAccount()
        {
            try
            {
                SA.Admin.CloseAccount(name);
            }
            catch (DFMCoreException e)
            {
                Error = e;
            }
        }

        [Then(@"the account will not be closed")]
        public void ThenTheAccountWillNotBeClosed()
        {
            var account = SA.Admin.SelectAccountByName(name);
            Assert.IsTrue(account.IsOpen());
        }

        [Then(@"the account will be closed")]
        public void ThenTheAccountWillBeClosed()
        {
            var account = SA.Admin.SelectAccountByName(name);
            Assert.IsFalse(account.IsOpen());
        }
        #endregion

        #region DeleteAccount
        [Given(@"I already have deleted the account")]
        public void GivenIDeleteAnAccount()
        {
            SA.Admin.DeleteAccount(name);
        }

        [When(@"I try to delete the account")]
        public void WhenITryToDeleteTheAccount()
        {
            try
            {
                SA.Admin.DeleteAccount(name);
            }
            catch (DFMCoreException e)
            {
                Error = e;
            }
        }

        [Then(@"the account will not be deleted")]
        public void ThenTheAccountWillNotBeDeleted()
        {
            Account = SA.Admin.SelectAccountByName(name);
            
            Assert.IsNotNull(Account);
        }

        [Then(@"the account will be deleted")]
        public void ThenTheAccountWillBeDeleted()
        {
            Error = null;

            try
            {
                SA.Admin.SelectAccountByName(Account.Name);
            }
            catch (DFMCoreException e)
            {
                Error = e;
            }

            Assert.IsNotNull(Error);
            Assert.AreEqual(ExceptionPossibilities.InvalidAccount, Error.Type);
        }
        #endregion

        #region SaveCategory
        [Given(@"I have this category to create")]
        public void GivenIHaveThisCategoryToCreate(Table table)
        {
            var categoryData = table.Rows[0];

            Category = new Category
            {
                Name = categoryData["Name"],
                User = User
            };
        }

        [Given(@"I already have this category")]
        public void GivenIAlreadyHaveCreatedThisCategory(Table table)
        {
            var categoryData = table.Rows[0];

            olderCategory = new Category
            {
                Name = categoryData["Name"],
                User = User
            };

            SA.Admin.SaveOrUpdateCategory(olderCategory);
        }

        [When(@"I try to save the category")]
        public void WhenITryToSaveTheCategory()
        {
            try
            {
                SA.Admin.SaveOrUpdateCategory(Category);
            }
            catch (DFMCoreException e)
            {
                Error = e;
            }
        }

        [Then(@"the category will not be saved")]
        public void ThenTheCategoryWillNotBeSaved()
        {
            Error = null;

            try
            {
                SA.Admin.SelectCategoryByName(Category.Name);
            }
            catch (DFMCoreException e)
            {
                Error = e;
            }

            Assert.IsNotNull(Error);
            Assert.AreEqual(ExceptionPossibilities.InvalidCategory, Error.Type);
        }
        
        [Then(@"the category will be saved")]
        public void ThenTheCategoryWillBeSaved()
        {
            Category = SA.Admin.SelectCategoryByName(Category.Name);

            Assert.IsNotNull(Category);
        }
        #endregion

        #region SelectCategoryByName
        [Given(@"I pass a name of category that doesn't exist")]
        public void GivenIPassAnNameOfCategoryThatDoesnTExist()
        {
            categoryName = "Invalid category";
        }

        [Given(@"I pass valid category Name")]
        public void GivenIPassValidCategoryName()
        {
            categoryName = Category.Name;
        }

        [When(@"I try to get the category by its Name")]
        public void WhenITryToGetTheCategoryByItsName()
        {
            Category = null;

            try
            {
                Category = SA.Admin.SelectCategoryByName(categoryName);
            }
            catch (DFMCoreException e)
            {
                Error = e;
            }
        }
        #endregion

        #region SelectCategoryById
        [Given(@"I pass valid category ID")]
        public void GivenIPassValidCategoryID()
        {
            name = Category.Name;
        }

        [When(@"I try to get the category")]
        public void WhenITryToGetTheCategory()
        {
            Category = null;

            try
            {
                Category = SA.Admin.SelectCategoryByName(name);
            }
            catch (DFMCoreException e)
            {
                Error = e;
            }
        }
        #endregion

        #region DisableCategory
        [Given(@"I give an id of enabled category ([\w ]+)")]
        public void GivenIGiveAnIdOfEnabledCategory(String givenCategoryName)
        {
            SA.Admin.SaveOrUpdateCategory(
                new Category { Name = givenCategoryName, User = User });

            Category = SA.Admin.SelectCategoryByName(givenCategoryName);

            name = Category.Name;
        }

        [Given(@"I already have disabled the category")]
        public void GivenIDisableACategory()
        {
            SA.Admin.DisableCategory(name);
        }

        [When(@"I try to disable the category")]
        public void WhenITryToDisableTheCategory()
        {
            try
            {
                SA.Admin.DisableCategory(name);
            }
            catch (DFMCoreException e)
            {
                Error = e;
            }
        }

        [Then(@"the category will be disabled")]
        public void ThenTheCategoryWillBeDisabled()
        {
            Category = SA.Admin.SelectCategoryByName(name);

            Assert.IsFalse(Category.Active);
        }
        #endregion

        #region EnableCategory
        [Given(@"I give an id of disabled category ([\w ]+)")]
        public void GivenIGiveAnIdOfDisabledCategory(String givenCategoryName)
        {
            SA.Admin.SaveOrUpdateCategory(
                new Category { Name = givenCategoryName, User = User });

            Category = SA.Admin.SelectCategoryByName(givenCategoryName);

            name = Category.Name;

            SA.Admin.DisableCategory(name);
        }

        [Given(@"I already have enabled the category")]
        public void GivenIEnableACategory()
        {
            SA.Admin.EnableCategory(name);
        }

        [When(@"I try to enable the category")]
        public void WhenITryToEnableTheCategory()
        {
            try
            {
                SA.Admin.EnableCategory(name);
            }
            catch (DFMCoreException e)
            {
                Error = e;
            }
        }

        [Then(@"the category will be enabled")]
        public void ThenTheCategoryWillBeEnabled()
        {
            Category = SA.Admin.SelectCategoryByName(name);

            Assert.IsTrue(Category.Active);
        }
        #endregion



        #region MoreThanOne
        [Given(@"I pass an id of account that doesn't exist")]
        public void GivenIPassAnIdOfAccountTheDoesnTExist()
        {
            name = "inexistent";
        }

        [Given(@"I give an id of the account ([\w ]+) without moves")]
        public void GivenIGiveAnIdOfAnAccountWithoutMoves(String givenAccountName)
        {
            Account = new Account {Name = givenAccountName, User = User};

            SA.Admin.SaveOrUpdateAccount(Account);

            Account = SA.Admin.SelectAccountByName(givenAccountName);

            name = Account.Name;
        }

        [Given(@"I give an id of the account ([\w ]+) with moves")]
        public void GivenIGiveAnIdOfAnAccountWithMoves(String givenAccountName)
        {
            Account = new Account { Name = givenAccountName, User = User };
            SA.Admin.SaveOrUpdateAccount(Account);
            Account = SA.Admin.SelectAccountByName(givenAccountName);

            var move = new Move
                           {
                               Date = DateTime.Now,
                               Description = "Move for account test",
                               Nature = MoveNature.Out
                           };

            // TODO: Remove this, put Value
            var detail = new Detail { Amount = 1, Description = move.Description, Value = 10 };

            move.DetailList.Add(detail);

            SA.Money.SaveOrUpdateMove(move, Account.Name, null, Category.Name);

            name = Account.Name;
        }

        [Then(@"I will receive no account")]
        public void ThenIWillReceiveNoAccount()
        {
            Assert.IsNull(Account);
        }

        [Then(@"I will receive the account")]
        public void ThenIWillReceiveTheAccount()
        {
            Assert.IsNotNull(Account);

            if (accountName != null)
                Assert.AreEqual(accountName, Account.Name);
        }




        [Given(@"I pass an id of category that doesn't exist")]
        public void GivenIPassAnIdOfCategoryThatDoesnTExist()
        {
            name = "inexistent";
        }

        [Then(@"I will receive no category")]
        public void ThenIWillReceiveNoCategory()
        {
            Assert.IsNull(Category);
        }

        [Then(@"I will receive the category")]
        public void ThenIWillReceiveTheCategory()
        {
            Assert.IsNotNull(Category);

            if (categoryName != null)
                Assert.AreEqual(categoryName, Category.Name);
        }

        #endregion

    }
}
