using System;
using System.Collections.Generic;
using System.Linq;
using DFM.BusinessLogic.Exceptions;
using DFM.BusinessLogic.ObjectInterfaces;
using DFM.Entities;
using DFM.Entities.Enums;
using DFM.Multilanguage;
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

		private static String oldAccountUrl
		{
			get { return Get<String>("oldAccountUrl"); }
			set { Set("oldAccountUrl", value); }
		}

		private static String newAccountUrl
		{
			get { return Get<String>("newAccountUrl"); } 
			set { Set("newAccountUrl", value); }
		}

		private static Decimal accountTotal
		{
			get { return Get<Decimal>("accountTotal"); }
			set { Set("accountTotal", value); }
		}

		private static IList<Account> accountList
		{
			get { return Get<IList<Account>>("accountList"); }
			set { Set("accountList", value); }
		}

		private static IList<Category> categoryList
		{
			get { return Get<IList<Category>>("categoryList"); }
			set { Set("categoryList", value); }
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

			Service.Admin.CreateAccount(oldAccount);
		}

		[When(@"I try to save the account")]
		public void WhenITryToSaveTheAccount()
		{
			try
			{
				Service.Admin.CreateAccount(Account);
			}
			catch (DFMCoreException e)
			{
				Error = e;
			}
		}

		[Then(@"the account will not be changed")]
		public void ThenTheAccountWillNotBeChanged()
		{
			var account = Service.Admin.GetAccountByUrl(oldAccount.Url);

			Assert.AreEqual(oldAccount.Name, account.Name);
			Assert.AreEqual(oldAccount.Url, account.Url);
			Assert.AreEqual(oldAccount.RedLimit, account.RedLimit);
			Assert.AreEqual(oldAccount.YellowLimit, account.YellowLimit);
		}

		[Then(@"the account will not be saved")]
		public void ThenTheAccountWillNotBeSaved()
		{
			Error = null;
			var accountUrl = Account.Url;
			Account = null;

			try
			{
				Service.Admin.GetAccountByUrl(accountUrl);
			}
			catch (DFMCoreException e)
			{
				Error = e;
			}

			Assert.IsNull(Account);
			Assert.IsNotNull(Error);
			Assert.AreEqual(ExceptionPossibilities.InvalidAccount, Error.Type);
		}

		[Then(@"the account will be saved")]
		public void ThenTheAccountWillBeSaved()
		{
			Account = Service.Admin.GetAccountByUrl(Account.Url);

			Assert.IsNotNull(Account);
		}
		#endregion

		#region UpdateAccount
		[Given(@"I have this account")]
		public void GivenIHaveThisAccount(Table table)
		{
			var accountData = table.Rows[0];

			oldAccountUrl = accountData["Url"];

			Account = new Account
			{
				Name = accountData["Name"],
				Url = oldAccountUrl,
			};

			Service.Admin.CreateAccount(Account);
		}

		[Given(@"this account has moves")]
		public void ThisAccountHasMoves()
		{
			Move = new Move
			{
				Description = "Description",
				Date = Current.User.Now(),
				Nature = MoveNature.Out,
			};

			var newDetail = new Detail
			{
				Description = Move.Description,
				Amount = 1,
				Value = 10,
			};

			Move.DetailList.Add(newDetail);

			Category = GetOrCreateCategory(MAIN_CATEGORY_NAME);

			Service.Money.SaveOrUpdateMove(Move, Account.Url, null, Category.Name);

			accountTotal = Account.Total();
		}
		
		[When(@"I make this changes to the account")]
		public void WhenIMakeThisChangesToTheAccount(Table table)
		{
			var accountData = table.Rows[0];

			newAccountUrl = accountData["Url"];
			Account.Name = accountData["Name"];
		}

		[When(@"I try to update the account")]
		public void WhenITryToUpdateTheAccount()
		{
			try
			{
				Service.Admin.UpdateAccount(Account, newAccountUrl);
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

			if (Account.Url != oldAccountUrl)
			{
				try
				{
					account = Service.Admin.GetAccountByUrl(oldAccountUrl);
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
				account = Service.Admin.GetAccountByUrl(newAccountUrl);
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
			Assert.AreEqual(accountTotal, Account.Total());
		}
		#endregion

		#region CloseAccount
		[Given(@"I already have closed the account")]
		public void GivenICloseTheAccount()
		{
			Service.Admin.CloseAccount(AccountUrl);
		}

		[When(@"I try to close the account")]
		public void WhenITryToCloseTheAccount()
		{
			try
			{
				Service.Admin.CloseAccount(AccountUrl);
			}
			catch (DFMCoreException e)
			{
				Error = e;
			}
		}

		[Then(@"the account will not be closed")]
		public void ThenTheAccountWillNotBeClosed()
		{
			var account = Service.Admin.GetAccountByUrl(AccountUrl);
			Assert.IsTrue(account.IsOpen());
		}

		[Then(@"the account will be closed")]
		public void ThenTheAccountWillBeClosed()
		{
			var account = Service.Admin.GetAccountByUrl(AccountUrl);
			Assert.IsFalse(account.IsOpen());
		}
		#endregion

		#region DeleteAccount
		[Given(@"I already have deleted the account")]
		public void GivenIDeleteAnAccount()
		{
			Service.Admin.DeleteAccount(AccountUrl);
		}

		[Given(@"I delete the moves of ([\w ]+)")]
		public void GivenIDeleteTheMovesOf(String givenAccountUrl)
		{
			var account = Service.Admin.GetAccountByUrl(givenAccountUrl);

			foreach (var year in account.YearList)
			{
				foreach (var month in year.MonthList)
				{
					foreach (var move in month.InList)
					{
						Service.Money.DeleteMove(move.ID);
					}

					foreach (var move in month.OutList)
					{
						Service.Money.DeleteMove(move.ID);
					}
				}
			}
		}

		[When(@"I try to delete the account")]
		public void WhenITryToDeleteTheAccount()
		{
			try
			{
				Service.Admin.DeleteAccount(AccountUrl);
			}
			catch (DFMCoreException e)
			{
				Error = e;
			}
		}

		[Then(@"the account will not be deleted")]
		public void ThenTheAccountWillNotBeDeleted()
		{
			Account = Service.Admin.GetAccountByUrl(AccountUrl);
			
			Assert.IsNotNull(Account);
		}

		[Then(@"the account will be deleted")]
		public void ThenTheAccountWillBeDeleted()
		{
			Error = null;
			var accountUrl = Account.Url;
			Account = null;

			try
			{
				Account = Service.Admin.GetAccountByUrl(accountUrl);
			}
			catch (DFMCoreException e)
			{
				Error = e;
			}

			Assert.IsNull(Account);
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

			Service.Admin.CreateCategory(oldCategory);
		}

		[When(@"I try to save the category")]
		public void WhenITryToSaveTheCategory()
		{
			try
			{
				Service.Admin.CreateCategory(Category);
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
			var categoryName = Category.Name;
			Category = null;

			try
			{
				Category = Service.Admin.GetCategoryByName(categoryName);
			}
			catch (DFMCoreException e)
			{
				Error = e;
			}

			Assert.IsNotNull(Error);
			Assert.IsNull(Category);
		}
		
		[Then(@"the category will be saved")]
		public void ThenTheCategoryWillBeSaved()
		{
			Category = Service.Admin.GetCategoryByName(Category.Name);

			Assert.IsNotNull(Category);
		}
		#endregion

		#region GetCategoryByName
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
				Category = Service.Admin.GetCategoryByName(CategoryName);
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

			Service.Admin.CreateCategory(Category);
		}

		[When(@"I make this changes to the category")]
		public void WhenIMakeThisChangesToTheCategory(Table table)
		{
			var categoryData = table.Rows[0];

			newCategoryName = categoryData["Name"];
		}

		[When(@"I try to update the category")]
		public void WhenITryToUpdateTheCategory()
		{
			try
			{
				Service.Admin.UpdateCategory(Category, newCategoryName);
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
				Category = Service.Admin.GetCategoryByName(oldCategoryName);
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
				Category = Service.Admin.GetCategoryByName(newCategoryName);
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
		[Given(@"I give the enabled category ([\w ]+)")]
		public void GivenIGiveAnEnabledCategory(String givenCategoryName)
		{
			Service.Admin.CreateCategory(
				new Category { Name = givenCategoryName, User = User });

			Category = Service.Admin.GetCategoryByName(givenCategoryName);

			CategoryName = Category.Name;
		}

		[Given(@"I already have disabled the category")]
		public void GivenIDisableACategory()
		{
			Service.Admin.DisableCategory(CategoryName);
		}

		[When(@"I try to disable the category")]
		public void WhenITryToDisableTheCategory()
		{
			try
			{
				Service.Admin.DisableCategory(CategoryName);
			}
			catch (DFMCoreException e)
			{
				Error = e;
			}
		}

		[Then(@"the category will be disabled")]
		public void ThenTheCategoryWillBeDisabled()
		{
			Category = Service.Admin.GetCategoryByName(CategoryName);

			Assert.IsFalse(Category.Active);
		}
		#endregion

		#region EnableCategory
		[Given(@"I give the disabled category ([\w ]+)")]
		public void GivenIGiveADisabledCategory(String givenCategoryName)
		{
			Service.Admin.CreateCategory(
				new Category { Name = givenCategoryName, User = User });

			Category = Service.Admin.GetCategoryByName(givenCategoryName);

			CategoryName = Category.Name;

			Service.Admin.DisableCategory(CategoryName);
		}

		[Given(@"I already have enabled the category")]
		public void GivenIEnableACategory()
		{
			Service.Admin.EnableCategory(CategoryName);
		}

		[When(@"I try to enable the category")]
		public void WhenITryToEnableTheCategory()
		{
			try
			{
				Service.Admin.EnableCategory(CategoryName);
			}
			catch (DFMCoreException e)
			{
				Error = e;
			}
		}

		[Then(@"the category will be enabled")]
		public void ThenTheCategoryWillBeEnabled()
		{
			Category = Service.Admin.GetCategoryByName(CategoryName);

			Assert.IsTrue(Category.Active);
		}
		#endregion

		#region GetAccountByUrl
		[Given(@"I pass an url of account that doesn't exist")]
		public void GivenIPassAUrlOfAccountThatDoesnTExist()
		{
			AccountUrl = "Invalid_account_url";
		}

		[When(@"I try to get the account by its url")]
		public void WhenITryToGetTheAccountByItsUrl()
		{
			Account = null;

			try
			{
				Account = Service.Admin.GetAccountByUrl(AccountUrl);
			}
			catch (DFMCoreException e)
			{
				Error = e;
			}
		}
		#endregion

		#region UpdateConfig

		[Given(@"I disable Categories use")]
		[When(@"I try to disable Categories use")]
		public void GivenIDisableCategoriesUse()
		{
			try
			{
				var mainConfig = new MainConfig { UseCategories = false };
				Service.Admin.UpdateConfig(mainConfig);
			}
			catch (DFMCoreException e)
			{
				Error = e;
			}
		}

		[Given(@"I enable Categories use")]
		[When(@"I try to enable Categories use")]
		public void GivenIEnableCategoriesUse()
		{
			try
			{
				var mainConfig = new MainConfig { UseCategories = true };
				Service.Admin.UpdateConfig(mainConfig);
			}
			catch (DFMCoreException e)
			{
				Error = e;
			}
		}

		[Given(@"I disable move check")]
		[When(@"I try to disable move check")]
		public void GivenIDisableMoveCheck()
		{
			try
			{
				var mainConfig = new MainConfig { MoveCheck = false };
				Service.Admin.UpdateConfig(mainConfig);
			}
			catch (DFMCoreException e)
			{
				Error = e;
			}
		}

		[Given(@"I enable move check")]
		[When(@"I try to enable move check")]
		public void GivenIEnableMoveCheck()
		{
			try
			{
				var mainConfig = new MainConfig { MoveCheck = true };
				Service.Admin.UpdateConfig(mainConfig);
			}
			catch (DFMCoreException e)
			{
				Error = e;
			}
		}

		[When(@"I try to change the language to ([a-z]{2}-[A-Z]{2})")]
		public void WhenITryToChangeTheLanguageTo(String language)
		{
			try
			{
				var mainConfig = new MainConfig { Language = language };
				Service.Admin.UpdateConfig(mainConfig);
			}
			catch (DFMCoreException e)
			{
				Error = e;
			}
		}

		[Then(@"the translation will be")]
		public void ThenTheTranslationWillBe(Table table)
		{
			foreach (var tableRow in table.Rows)
			{
				var translated = PlainText.Dictionary["general", Service.Current.Language, tableRow["Key"]];
				Assert.AreEqual(tableRow["Translated"], translated);
			}
		}

		[When(@"I try to change the timezone to ([\w\s\.]+)")]
		public void WhenITryToChangeTheTimezoneTo(String timezone)
		{
			try
			{
				var mainConfig = new MainConfig { TimeZone = timezone };
				Service.Admin.UpdateConfig(mainConfig);
			}
			catch (DFMCoreException e)
			{
				Error = e;
			}
		}

		[Given(@"I disable move send e-mail")]
		[When(@"I try to disable move send e-mail")]
		public void GivenIDisableMoveSendEmail()
		{
			try
			{
				var mainConfig = new MainConfig { SendMoveEmail = false };
				Service.Admin.UpdateConfig(mainConfig);
			}
			catch (DFMCoreException e)
			{
				Error = e;
			}
		}

		[Given(@"I enable move send e-mail")]
		[When(@"I try to enable move send e-mail")]
		public void GivenIEnableMoveSendEmail()
		{
			try
			{
				var mainConfig = new MainConfig { SendMoveEmail = true };
				Service.Admin.UpdateConfig(mainConfig);
			}
			catch (DFMCoreException e)
			{
				Error = e;
			}
		}

		#endregion



		#region GetAccountList
		[Given(@"I close the account (.+)")]
		public void GivenICloseTheAccount(String accountUrl)
		{
			Service.Admin.CloseAccount(accountUrl);
		}

		[When(@"ask for the (not )?active account list")]
		public void WhenAskForTheActiveAccountList(Boolean active)
		{
			try
			{
				accountList = Service.Admin.GetAccountList(active);
			}
			catch (DFMCoreException e)
			{
				Error = e;
			}
		}

		[Then(@"the account list will (not )?have this")]
		public void ThenTheAccountListsWillBeThis(Boolean has, Table table)
		{
			var expectedList = new List<Account>();

			foreach (var accountData in table.Rows)
			{
				var account = new Account
				{
					Name = accountData["Name"],
					Url = accountData["Url"]
				};

				expectedList.Add(account);
			}

			foreach (var expected in expectedList)
			{
				var account = accountList.SingleOrDefault(
					a => a.Url == expected.Url
						&& a.Name == expected.Name
				);

				if (has)
				{
					Assert.IsNotNull(account);
				}
				else
				{
					Assert.IsNull(account);
				}
			}
		}
		#endregion



		#region GetCategoryList
		[Given(@"I disable the category (.+)")]
		public void GivenICloseTheCategory(String categoryName)
		{
			Service.Admin.DisableCategory(categoryName);
		}

		[When(@"ask for all the category list")]
		public void WhenAskForAllTheCategoryList()
		{
			try
			{
				categoryList = Service.Admin.GetCategoryList();
			}
			catch (DFMCoreException e)
			{
				Error = e;
			}
		}

		[When(@"ask for the (not )?active category list")]
		public void WhenAskForTheActiveCategoryList(Boolean active)
		{
			try
			{
				categoryList = Service.Admin.GetCategoryList(active);
			}
			catch (DFMCoreException e)
			{
				Error = e;
			}
		}

		[Then(@"the category list will (not )?have this")]
		public void ThenTheCategoryListsWillBeThis(Boolean has, Table table)
		{
			var expectedList = new List<Category>();

			foreach (var categoryData in table.Rows)
			{
				var category = new Category
				{
					Name = categoryData["Name"]
				};

				expectedList.Add(category);
			}

			foreach (var expected in expectedList)
			{
				var category = categoryList.SingleOrDefault(
					c => c.Name == expected.Name
				);

				if (has)
				{
					Assert.IsNotNull(category);
				}
				else
				{
					Assert.IsNull(category);
				}
			}
		}
		#endregion



		#region MoreThanOne
		[Given(@"I pass a url of account that doesn't exist")]
		public void GivenIPassAnNameOfAccountThatDoesnTExist()
		{
			AccountUrl = "account_that_doesnt_exist";
		}

		[Given(@"I pass a name of category that doesn't exist")]
		public void GivenIPassAnNameOfCategoryThatDoesnTExist()
		{
			CategoryName = "Invalid category";
		}

		[Given(@"I give a url of the account ([\w ]+) without moves")]
		public void GivenIGiveAnNameOfAnAccountWithoutMoves(String givenAccountUrl)
		{
			Account = new Account
			{
				Name = givenAccountUrl,
				Url = givenAccountUrl, 
			};

			Service.Admin.CreateAccount(Account);

			AccountUrl = Account.Url;
		}

		[Given(@"I give a url of the account ([\w ]+) with moves")]
		public void GivenIGiveAnIdOfAnAccountWithMoves(String givenAccountUrl)
		{
			Account = new Account
			{
				Name = givenAccountUrl,
				Url = givenAccountUrl,
			};

			Service.Admin.CreateAccount(Account);

			var move = new Move
			{
				Date = Current.User.Now(),
				Description = "Move for account test",
				Nature = MoveNature.Out,
				Value = 10
			};

			Service.Money.SaveOrUpdateMove(move, Account.Url, null, Category.Name);

			AccountUrl = Account.Url;
		}

		[Given(@"the account has a schedule")]
		public void GivenTheAccountHasSchedules()
		{
			Schedule = new Schedule
			{
				Date = Current.User.Now().AddDays(1),
				Description = "Schedule for account test",
				Nature = MoveNature.Out,
				Frequency = ScheduleFrequency.Daily,
				Boundless = false,
				Times = 1,
				Value = 10
			};

			Service.Robot.SaveOrUpdateSchedule(Schedule, Account.Url, null, Category.Name);
		}

		[Given(@"the account has a disabled schedule")]
		public void GivenTheAccountHasADisabledSchedule()
		{
			Schedule = new Schedule
			{
				Date = Current.User.Now().AddDays(1),
				Description = "Schedule for account test",
				Nature = MoveNature.Out,
				Frequency = ScheduleFrequency.Daily,
				Boundless = false,
				Times = 1,
				Value = 10
			};

			Service.Robot.SaveOrUpdateSchedule(Schedule, Account.Url, null, Category.Name);

			Service.Robot.DisableSchedule(Schedule.ID);
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

			if (AccountUrl != null)
			{
				Assert.AreEqual(AccountUrl, Account.Url);
			}
			else
			{
				throw new NotImplementedException();
			}
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

		[Then(@"the schedule will be disabled")]
		public void ThenTheScheduleWillBeDisabled()
		{
			Error = null;

			try
			{
				Service.Robot.DisableSchedule(Schedule.ID);
			}
			catch (DFMCoreException e)
			{
				Error = e;
			}

			Assert.IsNotNull(Error);
			Assert.AreEqual(Error.Type, ExceptionPossibilities.DisabledSchedule);
		}


		#endregion

	}
}
