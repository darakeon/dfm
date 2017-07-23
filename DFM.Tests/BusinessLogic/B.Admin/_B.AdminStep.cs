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
        private static Account oldAccount
        {
            get { return Get<Account>("oldAccount"); }
            set { Set("oldAccount", value); }
        }

        private static String oldAccountName
        {
            get { return Get<String>("oldAccountName"); }
            set { Set("oldAccountName", value); }
        }

        private static String newAccountName
        {
            get { return Get<String>("newAccountName"); } 
            set { Set("newAccountName", value); }
        }

        private static Double accountTotal
        {
            get { return Get<Double>("accountTotal"); } 
            set { Set("accountTotal", value); }
        }



        private static Category oldCategory
        {
            get { return Get<Category>("oldCategory"); }
            set { Set("oldCategory", value); }
        }

        private static String oldCategoryName
        {
            get { return Get<String>("oldCategoryName"); }
            set { Set("oldCategoryName", value); }
        }

        private static String newCategoryName
        {
            get { return Get<String>("newCategoryName"); }
            set { Set("newCategoryName", value); }
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
                              Url = accountData["Url"],
                              RedLimit = GetInt(accountData["Red"]),
                              YellowLimit = GetInt(accountData["Yellow"]),
                              User = User
                          };
        }

        [Given(@"I already have this account")]
        public void GivenIAlreadyHaveThisAccount(Table table)
        {
            var accountData = table.Rows[0];

            oldAccount = new Account
            {
                Name = accountData["Name"],
                Url = accountData["Url"],
                RedLimit = GetInt(accountData["Red"]),
                YellowLimit = GetInt(accountData["Yellow"]),
                User = User
            };

            SA.Admin.CreateAccount(oldAccount);
        }

        [When(@"I try to save the account")]
        public void WhenITryToSaveTheAccount()
        {
            try
            {
                SA.Admin.CreateAccount(Account);
            }
            catch (DFMCoreException e)
            {
                Error = e;
            }
        }

        [Then(@"the account will not be changed")]
        public void ThenTheAccountWillNotBeChanged()
        {
            var account = SA.Admin.SelectAccountByName(oldAccount.Name);

            Assert.AreEqual(oldAccount.Name, account.Name);
            Assert.AreEqual(oldAccount.RedLimit, account.RedLimit);
            Assert.AreEqual(oldAccount.YellowLimit, account.YellowLimit);
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
        [When(@"I try to get the account by its name")]
        public void WhenITryToGetTheAccountByItsName()
        {
            Account = null;

            try
            {
                Account = SA.Admin.SelectAccountByName(AccountName);
            }
            catch (DFMCoreException e)
            {
                Error = e;
            }
        }
        #endregion

        #region UpdateAccount
        [Given(@"I have this account")]
        public void GivenIHaveThisAccount(Table table)
        {
            var accountData = table.Rows[0];

            oldAccountName = accountData["Name"];

            Account = new Account
            {
                Name = oldAccountName,
                Url = accountData["Url"],
                User = User,
            };

            SA.Admin.CreateAccount(Account);
        }

        [Given(@"this account has moves")]
        public void ThisAccountHasMoves()
        {
            Move = new Move
            {
                Description = "Description",
                Date = DateTime.Now,
                Nature = MoveNature.Out,
            };

            var newDetail = new Detail
            {
                Description = Move.Description,
                Amount = 1,
                Value = 10,
            };

            Move.DetailList.Add(newDetail);

            Category = GetOrCreateCategory(MainCategoryName);

            SA.Money.SaveOrUpdateMove((Move)Move, Account.Name, null, Category.Name);

            accountTotal = Account.Sum();
        }

        [When(@"make this changes to the account")]
        public void WhenMakeThisChanges(Table table)
        {
            var accountData = table.Rows[0];

            newAccountName = accountData["Name"];
            Account.Url = accountData["Url"];
        }

        [When(@"I try to update the account")]
        public void WhenITryToUpdateTheAccount()
        {
            try
            {
                SA.Admin.UpdateAccount(Account, newAccountName);
            }
            catch (DFMCoreException e)
            {
                Error = e;
            }
        }

        [Then(@"the account will be changed")]
        public void ThenTheAccountWillBeChanged()
        {
            Account account = null;
            Error = null;

            if (Account.Name != oldAccountName)
            {
                try
                {
                    account = SA.Admin.SelectAccountByName(oldAccountName);
                }
                catch (DFMCoreException e)
                {
                    Error = e;
                }

                Assert.IsNull(account);
                Assert.IsNotNull(Error);
                Assert.AreEqual(Error.Type, ExceptionPossibilities.InvalidAccount);

                Error = null;
            }


            try
            {
                account = SA.Admin.SelectAccountByName(newAccountName);
            }
            catch (DFMCoreException e)
            {
                Error = e;
            }

            Assert.IsNotNull(account);
            Assert.IsNull(Error);

            Assert.AreEqual(Account.Url, account.Url);
        }

        [Then(@"the account value will not change")]
        public void TheAccountValueWillNotChange()
        {
            Assert.AreEqual(accountTotal, Account.Sum());
        }
        #endregion

        #region CloseAccount
        [Given(@"I already have closed the account")]
        public void GivenICloseTheAccount()
        {
            SA.Admin.CloseAccount(AccountName);
        }

        [When(@"I try to close the account")]
        public void WhenITryToCloseTheAccount()
        {
            try
            {
                SA.Admin.CloseAccount(AccountName);
            }
            catch (DFMCoreException e)
            {
                Error = e;
            }
        }

        [Then(@"the account will not be closed")]
        public void ThenTheAccountWillNotBeClosed()
        {
            var account = SA.Admin.SelectAccountByName(AccountName);
            Assert.IsTrue(account.IsOpen());
        }

        [Then(@"the account will be closed")]
        public void ThenTheAccountWillBeClosed()
        {
            var account = SA.Admin.SelectAccountByName(AccountName);
            Assert.IsFalse(account.IsOpen());
        }
        #endregion

        #region DeleteAccount
        [Given(@"I already have deleted the account")]
        public void GivenIDeleteAnAccount()
        {
            SA.Admin.DeleteAccount(AccountName);
        }

        [When(@"I try to delete the account")]
        public void WhenITryToDeleteTheAccount()
        {
            try
            {
                SA.Admin.DeleteAccount(AccountName);
            }
            catch (DFMCoreException e)
            {
                Error = e;
            }
        }

        [Then(@"the account will not be deleted")]
        public void ThenTheAccountWillNotBeDeleted()
        {
            Account = SA.Admin.SelectAccountByName(AccountName);
            
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

            oldCategory = new Category
            {
                Name = categoryData["Name"],
                User = User
            };

            SA.Admin.CreateCategory(oldCategory);
        }

        [When(@"I try to save the category")]
        public void WhenITryToSaveTheCategory()
        {
            try
            {
                SA.Admin.CreateCategory(Category);
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
        [Given(@"I pass a valid category name")]
        public void GivenIPassValidCategoryName()
        {
            CategoryName = Category.Name;
        }

        [When(@"I try to get the category by its name")]
        public void WhenITryToGetTheCategoryByItsName()
        {
            Category = null;

            try
            {
                Category = SA.Admin.SelectCategoryByName(CategoryName);
            }
            catch (DFMCoreException e)
            {
                Error = e;
            }
        }
        #endregion

        #region UpdateCategory
        [Given(@"I have this category")]
        public void GivenIHaveThisCategory(Table table)
        {
            var categoryData = table.Rows[0];

            oldCategoryName = categoryData["Name"];

            Category = new Category
            {
                Name = oldCategoryName,
                User = User
            };

            SA.Admin.CreateCategory(Category);
        }

        [When(@"make this changes to the category")]
        public void WhenMakeThisChangesToCategory(Table table)
        {
            var categoryData = table.Rows[0];

            newCategoryName = categoryData["Name"];
        }

        [When(@"I try to update the category")]
        public void WhenITryToUpdateTheCategory()
        {
            try
            {
                SA.Admin.UpdateCategory(Category, newCategoryName);
            }
            catch (DFMCoreException e)
            {
                Error = e;
            }
        }

        [Then(@"the category will be changed")]
        public void ThenTheCategoryWillBeChanged()
        {
            Category = null;
            Error = null;

            try
            {
                Category = SA.Admin.SelectCategoryByName(oldCategoryName);
            }
            catch (DFMCoreException e)
            {
                Error = e;
            }

            Assert.IsNull(Category);
            Assert.IsNotNull(Error);
            Assert.AreEqual(Error.Type, ExceptionPossibilities.InvalidCategory);

            Category = null;
            Error = null;

            try
            {
                Category = SA.Admin.SelectCategoryByName(newCategoryName);
            }
            catch (DFMCoreException e)
            {
                Error = e;
            }

            Assert.IsNotNull(Category);
            Assert.IsNull(Error);
        }
        #endregion

        #region DisableCategory
        [Given(@"I give an id of enabled category ([\w ]+)")]
        public void GivenIGiveAnIdOfEnabledCategory(String givenCategoryName)
        {
            SA.Admin.CreateCategory(
                new Category { Name = givenCategoryName, User = User });

            Category = SA.Admin.SelectCategoryByName(givenCategoryName);

            CategoryName = Category.Name;
        }

        [Given(@"I already have disabled the category")]
        public void GivenIDisableACategory()
        {
            SA.Admin.DisableCategory(CategoryName);
        }

        [When(@"I try to disable the category")]
        public void WhenITryToDisableTheCategory()
        {
            try
            {
                SA.Admin.DisableCategory(CategoryName);
            }
            catch (DFMCoreException e)
            {
                Error = e;
            }
        }

        [Then(@"the category will be disabled")]
        public void ThenTheCategoryWillBeDisabled()
        {
            Category = SA.Admin.SelectCategoryByName(CategoryName);

            Assert.IsFalse(Category.Active);
        }
        #endregion

        #region EnableCategory
        [Given(@"I give an id of disabled category ([\w ]+)")]
        public void GivenIGiveAnIdOfDisabledCategory(String givenCategoryName)
        {
            SA.Admin.CreateCategory(
                new Category { Name = givenCategoryName, User = User });

            Category = SA.Admin.SelectCategoryByName(givenCategoryName);

            CategoryName = Category.Name;

            SA.Admin.DisableCategory(CategoryName);
        }

        [Given(@"I already have enabled the category")]
        public void GivenIEnableACategory()
        {
            SA.Admin.EnableCategory(CategoryName);
        }

        [When(@"I try to enable the category")]
        public void WhenITryToEnableTheCategory()
        {
            try
            {
                SA.Admin.EnableCategory(CategoryName);
            }
            catch (DFMCoreException e)
            {
                Error = e;
            }
        }

        [Then(@"the category will be enabled")]
        public void ThenTheCategoryWillBeEnabled()
        {
            Category = SA.Admin.SelectCategoryByName(CategoryName);

            Assert.IsTrue(Category.Active);
        }
        #endregion



        #region MoreThanOne
        [Given(@"I pass a name of account that doesn't exist")]
        public void GivenIPassAnNameOfAccountThatDoesnTExist()
        {
            AccountName = "Invalid account";
        }

        [Given(@"I pass a name of category that doesn't exist")]
        public void GivenIPassAnNameOfCategoryThatDoesnTExist()
        {
            CategoryName = "Invalid category";
        }

        [Given(@"I give a name of the account ([\w ]+) without moves")]
        public void GivenIGiveAnNameOfAnAccountWithoutMoves(String givenAccountName)
        {
            Account = new Account
                          {
                              Name = givenAccountName,
                              Url = MakeUrlFromName(givenAccountName), 
                              User = User
                          };

            SA.Admin.CreateAccount(Account);

            AccountName = Account.Name;
        }

        [Given(@"I give a name of the account ([\w ]+) with moves")]
        public void GivenIGiveAnIdOfAnAccountWithMoves(String givenAccountName)
        {
            Account = new Account
                          {
                              Name = givenAccountName,
                              Url = MakeUrlFromName(givenAccountName), 
                              User = User
                          };

            SA.Admin.CreateAccount(Account);

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

            AccountName = Account.Name;
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

            if (AccountName != null)
                Assert.AreEqual(AccountName, Account.Name);
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

            if (CategoryName != null)
                Assert.AreEqual(CategoryName, Category.Name);
        }

        #endregion

    }
}
