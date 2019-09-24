using System;
using System.Collections.Generic;
using System.Linq;
using DFM.BusinessLogic.Exceptions;
using DFM.BusinessLogic.Response;
using DFM.Entities;
using DFM.Entities.Enums;
using DFM.Language;
using NUnit.Framework;
using TechTalk.SpecFlow;
using error = DFM.BusinessLogic.Exceptions.Error;

namespace DFM.Tests.BusinessLogic.B.Admin
{
	[Binding]
	public class AdminStep : BaseStep
	{
		#region Variables
		private static AccountInfo oldAccount
		{
			get => Get<AccountInfo>("oldAccount");
			set => Set("oldAccount", value);
		}

		private static Decimal accountTotal
		{
			get => Get<Decimal>("accountTotal");
			set => Set("accountTotal", value);
		}

		private static IList<AccountListItem> accountList
		{
			get => Get<IList<AccountListItem>>("accountList");
			set => Set("accountList", value);
		}

		private static IList<CategoryListItem> categoryList
		{
			get => Get<IList<CategoryListItem>>("categoryList");
			set => Set("categoryList", value);
		}



		private static CategoryInfo oldCategory
		{
			get => Get<CategoryInfo>("oldCategory");
			set => Set("oldCategory", value);
		}


		private static BootstrapTheme theme
		{
			get => Get<BootstrapTheme>("theme");
			set => Set("theme", value);
		}

		#endregion



		#region SaveAccount
		[Given(@"I have this account to create")]
		public void GivenIHaveThisAccountToCreate(Table table)
		{
			var accountData = table.Rows[0];

			Account = new AccountInfo
			{
				Name = accountData["Name"],
				Url = accountData["Url"],
				RedLimit = GetInt(accountData["Red"]),
				YellowLimit = GetInt(accountData["Yellow"]),
			};
		}

		[Given(@"I already have this account")]
		public void GivenIAlreadyHaveThisAccount(Table table)
		{
			var accountData = table.Rows[0];

			oldAccount = new AccountInfo
			{
				Name = accountData["Name"],
				Url = accountData["Url"],
			};

			if (accountData.ContainsKey("Red"))
				oldAccount.RedLimit = GetInt(accountData["Red"]);

			if (accountData.ContainsKey("Yellow"))
				oldAccount.YellowLimit = GetInt(accountData["Yellow"]);

			Service.Admin.CreateAccount(oldAccount);
		}

		[When(@"I try to save the account")]
		public void WhenITryToSaveTheAccount()
		{
			try
			{
				Service.Admin.CreateAccount(Account);
			}
			catch (CoreError e)
			{
				Error = e;
			}
		}

		[Then(@"the account will not be changed")]
		public void ThenTheAccountWillNotBeChanged()
		{
			var account = Service.Admin.GetAccountByUrl(oldAccount.Url);

			Assert.AreEqual(oldAccount.Name, account.Name);
			Assert.AreEqual(oldAccount.Url.ToLower(), account.Url);
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
			catch (CoreError e)
			{
				Error = e;
			}

			Assert.IsNull(Account);
			Assert.IsNotNull(Error);
			Assert.AreEqual(error.InvalidAccount, Error.Type);
		}

		[Then(@"the account will be saved")]
		public void ThenTheAccountWillBeSaved()
		{
			Account = Service.Admin.GetAccountByUrl(Account.Url);

			Assert.IsNotNull(Account);
		}
		#endregion

		#region GetAccountByUrl
		[Given(@"I pass an url of account that doesn't exist")]
		public void GivenIPassAUrlOfAccountThatDoesNotExist()
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
			catch (CoreError e)
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

			Account = new AccountInfo
			{
				Name = accountData["Name"],
				Url = accountData["Url"],
			};

			Service.Admin.CreateAccount(Account);
		}

		[Given(@"this account has moves")]
		public void ThisAccountHasMoves()
		{
			moveInfo = new MoveInfo
			{
				Description = "Description",
				Date = Current.Now,
				Nature = MoveNature.Out,
				OutUrl = Account.Url,
			};

			var newDetail = new DetailInfo
			{
				Description = moveInfo.Description,
				Amount = 1,
				Value = 10,
			};

			moveInfo.DetailList.Add(newDetail);

			Service.Money.SaveMove(moveInfo);

			var user = userRepository.GetByEmail(Current.Email);
			var account = accountRepository.GetByUrl(Account.Url, user);
			accountTotal = summaryRepository.GetTotal(account);
		}

		[When(@"I make this changes to the account")]
		public void WhenIMakeThisChangesToTheAccount(Table table)
		{
			var accountData = table.Rows[0];

			Account = new AccountInfo
			{
				OriginalUrl = (Account ?? oldAccount).Url,
				Url = accountData["Url"],
				Name = accountData["Name"],
			};
		}

		[When(@"I try to update the account")]
		public void WhenITryToUpdateTheAccount()
		{
			try
			{
				Service.Admin.UpdateAccount(Account);
			}
			catch (CoreError e)
			{
				Error = e;
			}
		}

		[Then(@"the account will be changed")]
		public void ThenTheAccountWillBeChanged()
		{
			AccountInfo account = null;
			Error = null;

			if (Account.Url != Account.OriginalUrl)
			{
				try
				{
					account = Service.Admin.GetAccountByUrl(Account.OriginalUrl);
				}
				catch (CoreError e)
				{
					Error = e;
				}

				Assert.IsNull(account);
				Assert.IsNotNull(Error);
				Assert.AreEqual(Error.Type, error.InvalidAccount);

				Error = null;
			}

			try
			{
				account = Service.Admin.GetAccountByUrl(Account.Url);
			}
			catch (CoreError e)
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
			var user = userRepository.GetByEmail(Current.Email);
			var account = accountRepository.GetByUrl(Account.Url, user);
			var total = summaryRepository.GetTotal(account);
			Assert.AreEqual(accountTotal, total);
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
			catch (CoreError e)
			{
				Error = e;
			}
		}

		[Then(@"the account will not be closed")]
		public void ThenTheAccountWillNotBeClosed()
		{
			var user = userRepository.GetByEmail(Current.Email);
			var account = accountRepository.GetByUrl(AccountUrl, user);
			Assert.IsTrue(account.IsOpen());
		}

		[Then(@"the account will be closed")]
		public void ThenTheAccountWillBeClosed()
		{
			var user = userRepository.GetByEmail(Current.Email);
			var account = accountRepository.GetByUrl(AccountUrl, user);
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
			var user = userRepository.GetByEmail(Current.Email);
			var account = accountRepository.GetByUrl(AccountUrl, user);
			var moveList = moveRepository.ByAccount(account);

			foreach (var move in moveList)
			{
				Service.Money.DeleteMove(move.ID);
			}
		}

		[When(@"I try to delete the account")]
		public void WhenITryToDeleteTheAccount()
		{
			try
			{
				Service.Admin.DeleteAccount(AccountUrl);
			}
			catch (CoreError e)
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
			catch (CoreError e)
			{
				Error = e;
			}

			Assert.IsNull(Account);
			Assert.IsNotNull(Error);
			Assert.AreEqual(error.InvalidAccount, Error.Type);
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
			catch (CoreError e)
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



		#region SaveCategory
		[Given(@"I have this category to create")]
		public void GivenIHaveThisCategoryToCreate(Table table)
		{
			var categoryData = table.Rows[0];

			Category = new CategoryInfo
			{
				Name = categoryData["Name"],
			};
		}

		[Given(@"I already have this category")]
		public void GivenIAlreadyHaveCreatedThisCategory(Table table)
		{
			var categoryData = table.Rows[0];

			oldCategory = new CategoryInfo
			{
				Name = categoryData["Name"],
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
			catch (CoreError e)
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
			catch (CoreError e)
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
			catch (CoreError e)
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

			Category = new CategoryInfo
			{
				Name = categoryData["Name"],
			};

			Service.Admin.CreateCategory(Category);
		}

		[Given(@"I make this changes to the category")]
		public void WhenIMakeThisChangesToTheCategory(Table table)
		{
			var categoryData = table.Rows[0];

			Category.OriginalName = Category.Name;
			Category.Name = categoryData["Name"];
		}

		[When(@"I try to update the category")]
		public void WhenITryToUpdateTheCategory()
		{
			try
			{
				Service.Admin.UpdateCategory(Category);
			}
			catch (CoreError e)
			{
				Error = e;
			}
		}

		[Then(@"the category will (not )?be changed")]
		public void ThenTheCategoryWillBeChanged(Boolean changed)
		{
			CategoryInfo category = null;
			Error = null;

			try
			{
				category = Service.Admin.GetCategoryByName(Category.OriginalName);
			}
			catch (CoreError e)
			{
				Error = e;
			}

			if (!changed)
			{
				Assert.IsNotNull(category);
				Assert.IsNull(Error);
				return;
			}

			Assert.IsNull(category);
			Assert.IsNotNull(Error);
			Assert.AreEqual(Error.Type, error.InvalidCategory);

			Error = null;

			try
			{
				category = Service.Admin.GetCategoryByName(Category.Name);
			}
			catch (CoreError e)
			{
				Error = e;
			}

			Assert.IsNotNull(category);
			Assert.IsNull(Error);
		}
		#endregion

		#region DisableCategory
		[Given(@"I give the enabled category ([\w ]+)")]
		public void GivenIGiveAnEnabledCategory(String givenCategoryName)
		{
			Service.Admin.CreateCategory(
				new CategoryInfo
				{
					Name = givenCategoryName
				}
			);

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
			catch (CoreError e)
			{
				Error = e;
			}
		}

		[Then(@"the category will be disabled")]
		public void ThenTheCategoryWillBeDisabled()
		{
			var user = userRepository.GetByEmail(Current.Email);
			var category = categoryRepository.GetByName(CategoryName, user);
			Assert.IsFalse(category.Active);
		}
		#endregion

		#region EnableCategory
		[Given(@"I give the disabled category ([\w ]+)")]
		public void GivenIGiveADisabledCategory(String givenCategoryName)
		{
			Service.Admin.CreateCategory(
				new CategoryInfo
				{
					Name = givenCategoryName
				}
			);

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
			catch (CoreError e)
			{
				Error = e;
			}
		}

		[Then(@"the category will be enabled")]
		public void ThenTheCategoryWillBeEnabled()
		{
			var user = userRepository.GetByEmail(Current.Email);
			var category = categoryRepository.GetByName(CategoryName, user);
			Assert.IsTrue(category.Active);
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
			catch (CoreError e)
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
			catch (CoreError e)
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





		#region UpdateConfig

		[Given(@"I disable Categories use")]
		[When(@"I try to disable Categories use")]
		public void GivenIDisableCategoriesUse()
		{
			try
			{
				var mainConfig = new ConfigInfo { UseCategories = false };
				Service.Admin.UpdateConfig(mainConfig);
			}
			catch (CoreError e)
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
				var mainConfig = new ConfigInfo { UseCategories = true };
				Service.Admin.UpdateConfig(mainConfig);
			}
			catch (CoreError e)
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
				var mainConfig = new ConfigInfo { MoveCheck = false };
				Service.Admin.UpdateConfig(mainConfig);
			}
			catch (CoreError e)
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
				var mainConfig = new ConfigInfo { MoveCheck = true };
				Service.Admin.UpdateConfig(mainConfig);
			}
			catch (CoreError e)
			{
				Error = e;
			}
		}

		[Given(@"I disable wizard")]
		[When(@"I try to disable wizard")]
		public void GivenIDisableWizard()
		{
			try
			{
				var mainConfig = new ConfigInfo { Wizard = false };
				Service.Admin.UpdateConfig(mainConfig);
			}
			catch (CoreError e)
			{
				Error = e;
			}
		}

		[Given(@"I enable wizard")]
		[When(@"I try to enable wizard")]
		public void GivenIEnableWizard()
		{
			try
			{
				var mainConfig = new ConfigInfo { Wizard = true };
				Service.Admin.UpdateConfig(mainConfig);
			}
			catch (CoreError e)
			{
				Error = e;
			}
		}

		[When(@"I try to change the language to ([a-z]{2}-[A-Z]{2})")]
		public void WhenITryToChangeTheLanguageTo(String language)
		{
			try
			{
				var mainConfig = new ConfigInfo { Language = language };
				Service.Admin.UpdateConfig(mainConfig);
			}
			catch (CoreError e)
			{
				Error = e;
			}
		}

		[Then(@"the translation will be")]
		public void ThenTheTranslationWillBe(Table table)
		{
			foreach (var tableRow in table.Rows)
			{
				var translated = PlainText.Site["general", Service.Current.Language, tableRow["Key"]];
				Assert.AreEqual(tableRow["Translated"], translated);
			}
		}

		[When(@"I try to change the timezone to ([\w\s\.]+)")]
		public void WhenITryToChangeTheTimezoneTo(String timezone)
		{
			try
			{
				var mainConfig = new ConfigInfo { TimeZone = timezone };
				Service.Admin.UpdateConfig(mainConfig);
			}
			catch (CoreError e)
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
				var mainConfig = new ConfigInfo { SendMoveEmail = false };
				Service.Admin.UpdateConfig(mainConfig);
			}
			catch (CoreError e)
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
				var mainConfig = new ConfigInfo { SendMoveEmail = true };
				Service.Admin.UpdateConfig(mainConfig);
			}
			catch (CoreError e)
			{
				Error = e;
			}
		}

		#endregion

		#region ChangeTheme
		[Given(@"a theme (\w+)")]
		public void GivenAThemeSlate(BootstrapTheme chosenTheme)
		{
			theme = chosenTheme;
		}

		[When(@"I try to change the Theme")]
		public void WhenITryToChangeTheTheme()
		{
			try
			{
				Service.Admin.ChangeTheme(theme);
            }
			catch (CoreError e)
			{
				Error = e;
			}
		}

		[Then(@"the Theme will be (\w+)")]
		public void ThenTheThemeWillBeSlate(BootstrapTheme chosenTheme)
		{
			Assert.AreEqual(chosenTheme, Current.Theme);
		}
		#endregion ChangeTheme



		#region MoreThanOne
		[Given(@"I pass a url of account that doesn't exist")]
		public void GivenIPassAnNameOfAccountThatDoesNotExist()
		{
			AccountUrl = "account_that_does_not_exist";
		}

		[Given(@"I pass a name of category that doesn't exist")]
		public void GivenIPassAnNameOfCategoryThatDoesNotExist()
		{
			CategoryName = "Invalid category";
		}

		[Given(@"I give a url of the account ([\w ]+) without moves")]
		public void GivenIGiveAnNameOfAnAccountWithoutMoves(String givenAccountUrl)
		{
			Account = new AccountInfo
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
			Account = new AccountInfo
			{
				Name = givenAccountUrl,
				Url = givenAccountUrl,
			};

			Service.Admin.CreateAccount(Account);

			var move = new MoveInfo
			{
				Date = Current.Now,
				Description = "Move for account test",
				Nature = MoveNature.Out,
				Value = 10,
				OutUrl = Account.Url,
				CategoryName = Category?.Name,
			};

			Service.Money.SaveMove(move);

			AccountUrl = Account.Url;
		}

		[Given(@"the account has a schedule( with details)?")]
		public void GivenTheAccountHasSchedules(String withDetails)
		{
			scheduleInfo = new ScheduleInfo
			{
				Date = Current.Now.AddDays(1),
				Description = "Schedule for account test",
				Nature = MoveNature.Out,
				Frequency = ScheduleFrequency.Daily,
				Boundless = false,
				Times = 1,
				OutUrl = Account.Url,
				CategoryName = Category?.Name,
			};

			if (String.IsNullOrEmpty(withDetails))
			{
				scheduleInfo.Value = 10;
			}
			else
			{
				var detail = new DetailInfo
				{
					Description = "Detail",
					Amount = 42,
					Value = 27
				};

				scheduleInfo.DetailList.Add(detail);
			}

			var schedule = Service.Robot.SaveSchedule(scheduleInfo);
			scheduleInfo.ID = schedule.ID;
		}

		[Given(@"the account has a disabled schedule( with details)?")]
		public void GivenTheAccountHasADisabledSchedule(String withDetails)
		{
			scheduleInfo = new ScheduleInfo
			{
				Date = Current.Now.AddDays(1),
				Description = "Schedule for account test",
				Nature = MoveNature.Out,
				Frequency = ScheduleFrequency.Daily,
				Boundless = false,
				Times = 1,
				OutUrl = Account.Url,
			};

			if (String.IsNullOrEmpty(withDetails))
			{
				scheduleInfo.Value = 10;
			}
			else
			{
				var detail = new DetailInfo
				{
					Description = "Detail",
					Amount = 42,
					Value = 27
				};

				scheduleInfo.DetailList.Add(detail);
			}

			var schedule = Service.Robot.SaveSchedule(scheduleInfo);
			scheduleInfo.ID = schedule.ID;

			Service.Robot.DisableSchedule(scheduleInfo.ID);
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
		#endregion

	}
}
