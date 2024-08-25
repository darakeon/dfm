using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using DFM.BusinessLogic.Exceptions;
using DFM.BusinessLogic.Response;
using DFM.BusinessLogic.Tests.Helpers;
using DFM.Entities;
using DFM.Entities.Bases;
using DFM.Entities.Enums;
using DFM.Generic;
using DFM.Language;
using DFM.Tests.Util;
using NUnit.Framework;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
using Error = DFM.BusinessLogic.Exceptions.Error;

namespace DFM.BusinessLogic.Tests.Steps
{
	[Binding]
	public class AdminStep : BaseStep
	{
		public AdminStep(ScenarioContext context)
			: base(context) { }

		#region Variables
		private AccountInfo oldAccount
		{
			get => get<AccountInfo>("oldAccount");
			set => set("oldAccount", value);
		}

		private Decimal accountTotal
		{
			get => get<Decimal>("accountTotal");
			set => set("accountTotal", value);
		}

		private IList<AccountListItem> accountList
		{
			get => get<IList<AccountListItem>>("accountList");
			set => set("accountList", value);
		}

		private IList<CategoryListItem> categoryList
		{
			get => get<IList<CategoryListItem>>("categoryList");
			set => set("categoryList", value);
		}



		private CategoryInfo oldCategory
		{
			get => get<CategoryInfo>("oldCategory");
			set => set("oldCategory", value);
		}


		private Theme theme
		{
			get => get<Theme>("theme");
			set => set("theme", value);
		}

		#endregion

		#region Account
		#region SaveAccount
		[Given(@"I have this account to create")]
		public void GivenIHaveThisAccountToCreate(Table table)
		{
			var accountData = table.Rows[0];

			accountInfo = new AccountInfo
			{
				Name = accountData["Name"]
					.ForScenario(scenarioCode),
				RedLimit = accountData.ContainsKey("Red")
					? getInt(accountData["Red"])
					: null,
				YellowLimit = accountData.ContainsKey("Yellow")
					? getInt(accountData["Yellow"])
					: null,
				Currency = accountData.ContainsKey("Currency")
					? EnumX.Parse<Currency>(accountData["Currency"])
					: null,
			};
		}

		[Given(@"I already have this account")]
		public void GivenIAlreadyHaveThisAccount(Table table)
		{
			var accountData = table.Rows[0];

			oldAccount = new AccountInfo
			{
				Name = accountData["Name"]
					.ForScenario(scenarioCode),
			};

			if (accountData.ContainsKey("Red"))
				oldAccount.RedLimit = getInt(accountData["Red"]);

			if (accountData.ContainsKey("Yellow"))
				oldAccount.YellowLimit = getInt(accountData["Yellow"]);

			if (accountData.ContainsKey("Currency"))
				oldAccount.Currency = EnumX.Parse<Currency>(accountData["Currency"]);

			service.Admin.CreateAccount(oldAccount);
		}

		[Given(@"I have these accounts")]
		public void GivenIHaveTheseAccounts(Table table)
		{
			foreach (var accountData in table.Rows)
			{
				var account = new AccountInfo
				{
					Name = accountData["Name"]
						.ForScenario(scenarioCode),
				};

				if (accountData.ContainsKey("Red"))
					account.RedLimit = getInt(accountData["Red"]);

				if (accountData.ContainsKey("Yellow"))
					account.YellowLimit = getInt(accountData["Yellow"]);

				if (accountData.ContainsKey("Currency") && accountData["Currency"] != "")
					account.Currency = EnumX.Parse<Currency>(accountData["Currency"]);

				service.Admin.CreateAccount(account);
			}
		}

		[When(@"I try to save the account")]
		public void WhenITryToSaveTheAccount()
		{
			try
			{
				service.Admin.CreateAccount(accountInfo);
			}
			catch (CoreError e)
			{
				error = e;
			}
		}

		[Then(@"the account will not be changed")]
		public void ThenTheAccountWillNotBeChanged()
		{
			var account = service.Admin.GetAccount(oldAccount.Name.IntoUrl());

			Assert.That(account.Name, Is.EqualTo(oldAccount.Name));
			Assert.That(account.RedLimit, Is.EqualTo(oldAccount.RedLimit));
			Assert.That(account.YellowLimit, Is.EqualTo(oldAccount.YellowLimit));
			Assert.That(account.Currency, Is.EqualTo(oldAccount.Currency));
		}

		[Then(@"the account will not be saved")]
		public void ThenTheAccountWillNotBeSaved()
		{
			error = null;
			var url = accountInfo.Name.IntoUrl();
			accountInfo = null;

			try
			{
				service.Admin.GetAccount(url);
			}
			catch (CoreError e)
			{
				error = e;
			}

			Assert.That(accountInfo, Is.Null);
			Assert.That(error, Is.Not.Null);
			Assert.That(error.Type, Is.EqualTo(Error.InvalidAccount));
		}

		[Then(@"the account will be saved")]
		public void ThenTheAccountWillBeSaved()
		{
			var newAccount = service.Admin.GetAccount(accountInfo.Name.IntoUrl());

			Assert.That(newAccount, Is.Not.Null);
			Assert.That(newAccount.RedLimit, Is.EqualTo(accountInfo.RedLimit));
			Assert.That(newAccount.YellowLimit, Is.EqualTo(accountInfo.YellowLimit));
			Assert.That(newAccount.Currency, Is.EqualTo(accountInfo.Currency));
		}

		[Then(@"the account url will be (.+)")]
		public void ThenTheAccountUrlWillBe(String url)
		{
			url = url.ForScenario(scenarioCode.ToLower());
			var newAccount = service.Admin.GetAccount(url);

			Assert.That(accountInfo.Name, Is.Not.Null, newAccount.Name);
		}
		#endregion

		#region GetAccountByUrl
		[Given(@"I pass an url of account that doesn't exist")]
		public void GivenIPassAUrlOfAccountThatDoesNotExist()
		{
			accountUrl = "Invalid_account_url";
		}

		[When(@"I try to get the account by its url")]
		public void WhenITryToGetTheAccountByItsUrl()
		{
			accountInfo = null;

			try
			{
				accountInfo = service.Admin.GetAccount(accountUrl);
			}
			catch (CoreError e)
			{
				error = e;
			}
		}
		#endregion

		#region UpdateAccount
		[Given(@"I have this account")]
		public void GivenIHaveThisAccount(Table table)
		{
			var accountData = table.Rows[0];

			accountInfo = new AccountInfo
			{
				Name = accountData["Name"].ForScenario(scenarioCode),
			};

			if (accountData.ContainsKey("Yellow") && accountData["Yellow"] != "")
				accountInfo.YellowLimit = Decimal.Parse(accountData["Yellow"]);

			if (accountData.ContainsKey("Red") && accountData["Red"] != "")
				accountInfo.RedLimit = Decimal.Parse(accountData["Red"]);

			if (accountData.ContainsKey("Currency") && accountData["Currency"] != "")
				accountInfo.Currency = EnumX.Parse<Currency>(accountData["Currency"]);

			service.Admin.CreateAccount(accountInfo);
		}

		[Given(@"this account has moves")]
		public void ThisAccountHasMoves()
		{
			moveInfo = new MoveInfo
			{
				Description = "Description",
				Nature = MoveNature.Out,
				OutUrl = accountInfo.Name.IntoUrl(),
			};

			moveInfo.SetDate(current.Now);

			var newDetail = new DetailInfo
			{
				Description = moveInfo.Description,
				Amount = 1,
				Value = 10,
			};

			moveInfo.DetailList.Add(newDetail);

			service.Money.SaveMove(moveInfo);

			var user = repos.User.GetByEmail(current.Email);
			var account = repos.Account.GetByUrl(accountInfo.Name.IntoUrl(), user);
			accountTotal = repos.Summary.GetTotal(account);
		}

		[When(@"I make this changes to the account")]
		public void WhenIMakeThisChangesToTheAccount(Table table)
		{
			var accountData = table.Rows[0];

			accountInfo = new AccountInfo
			{
				OriginalUrl = (accountInfo ?? oldAccount).Name.IntoUrl(),
				Name = accountData["Name"]
					.ForScenario(scenarioCode),
			};

			if (accountData.ContainsKey("Red"))
				accountInfo.RedLimit = getInt(accountData["Red"]);

			if (accountData.ContainsKey("Yellow"))
				accountInfo.YellowLimit = getInt(accountData["Yellow"]);

			if (accountData.ContainsKey("Currency"))
				accountInfo.Currency = EnumX.Parse<Currency>(accountData["Currency"]);
		}

		[When(@"I try to update the account")]
		public void WhenITryToUpdateTheAccount()
		{
			try
			{
				service.Admin.UpdateAccount(accountInfo);
			}
			catch (CoreError e)
			{
				error = e;
			}
		}

		[Then(@"the account will be changed")]
		public void ThenTheAccountWillBeChanged()
		{
			AccountInfo account = null;
			error = null;

			try
			{
				var url = accountInfo.Name.IntoUrl();
				account = service.Admin.GetAccount(url);
			}
			catch (CoreError e)
			{
				error = e;
			}

			Assert.That(account, Is.Not.Null);
			Assert.That(error, Is.Null);

			Assert.That(account.Name, Is.EqualTo(accountInfo.Name));
			Assert.That(account.HasLimit, Is.EqualTo(accountInfo.HasLimit));
			Assert.That(account.RedLimit, Is.EqualTo(accountInfo.RedLimit));
			Assert.That(account.YellowLimit, Is.EqualTo(accountInfo.YellowLimit));
			Assert.That(account.Currency, Is.EqualTo(accountInfo.Currency));
		}

		[Then(@"the account value will not change")]
		public void TheAccountValueWillNotChange()
		{
			var user = repos.User.GetByEmail(current.Email);
			var account = repos.Account.GetByUrl(accountInfo.Name.IntoUrl(), user);
			var total = repos.Summary.GetTotal(account);
			Assert.That(total, Is.EqualTo(accountTotal));
		}
		#endregion

		#region CloseAccount
		[Given(@"I already have closed the account")]
		public void GivenICloseTheAccount()
		{
			service.Admin.CloseAccount(accountUrl);
		}

		[When(@"I try to close the account")]
		public void WhenITryToCloseTheAccount()
		{
			try
			{
				service.Admin.CloseAccount(accountUrl);
			}
			catch (CoreError e)
			{
				error = e;
			}
		}

		[Then(@"the account will not be closed")]
		public void ThenTheAccountWillNotBeClosed()
		{
			var user = repos.User.GetByEmail(current.Email);
			var account = repos.Account.GetByUrl(accountUrl, user);
			Assert.That(account.Open, Is.True);
		}

		[Then(@"the account will be closed")]
		public void ThenTheAccountWillBeClosed()
		{
			var user = repos.User.GetByEmail(current.Email);
			var account = repos.Account.GetByUrl(accountUrl, user);
			Assert.That(account.Open, Is.False);
		}
		#endregion

		#region DeleteAccount
		[Given(@"I already have deleted the account")]
		public void GivenIDeleteAnAccount()
		{
			service.Admin.DeleteAccount(accountUrl);
		}

		[Given(@"I delete the moves of ([\w ]+)")]
		public void GivenIDeleteTheMovesOf(String givenAccountUrl)
		{
			var user = repos.User.GetByEmail(current.Email);
			var account = repos.Account.GetByUrl(givenAccountUrl.IntoUrl(), user);
			var moveList = repos.Move.ByAccount(account);

			foreach (var move in moveList)
			{
				service.Money.DeleteMove(move.Guid);
			}
		}

		[When(@"I try to delete the account")]
		public void WhenITryToDeleteTheAccount()
		{
			try
			{
				service.Admin.DeleteAccount(accountUrl);
			}
			catch (CoreError e)
			{
				error = e;
			}
		}

		[Then(@"the account will not be deleted")]
		public void ThenTheAccountWillNotBeDeleted()
		{
			accountInfo = service.Admin.GetAccount(accountUrl);

			Assert.That(accountInfo, Is.Not.Null);
		}

		[Then(@"the account will be deleted")]
		public void ThenTheAccountWillBeDeleted()
		{
			error = null;
			var url = accountInfo.Name.IntoUrl();
			accountInfo = null;

			try
			{
				accountInfo = service.Admin.GetAccount(url);
			}
			catch (CoreError e)
			{
				error = e;
			}

			Assert.That(accountInfo, Is.Null);
			Assert.That(error, Is.Not.Null);
			Assert.That(error.Type, Is.EqualTo(Error.InvalidAccount));
		}
		#endregion

		#region GetAccountList
		[Given(@"I open the account (.+)")]
		public void GivenIOpenTheAccount(String url)
		{
			url = url.IntoUrl();
			var user = repos.User.GetByEmail(current.Email);
			var account = repos.Account.GetByUrl(url, user);

			if (!account.Open)
				service.Admin.ReopenAccount(url);
		}

		[Given(@"I close the account (.+)")]
		public void GivenICloseTheAccount(String url)
		{
			url = url.IntoUrl();
			var user = repos.User.GetByEmail(current.Email);
			var account = repos.Account.GetByUrl(url, user);

			if (account.Open)
			{
				var hasMoves = repos.Move.Any(
					m => m.In.ID == account.ID || m.Out.ID == account.ID
				);

				if (!hasMoves)
				{
					var move = new MoveInfo
					{
						Description = "for closing",
						Nature = MoveNature.Out,
						OutUrl = account.Url,
						Value = 1,
					};
					move.SetDate(new DateTime(1986, 3, 27));

					if (user.Settings.UseCategories)
						move.CategoryName = mainCategoryName;

					service.Money.SaveMove(move);

					if (account.Url == accountOutUrl)
					{
						accountOutTotal -= move.Value;
					}

					if (account.Url == accountInUrl)
					{
						accountInTotal -= move.Value;
					}
				}

				service.Admin.CloseAccount(url);
			}
		}

		[When(@"ask for the (not )?active account list")]
		public void WhenAskForTheActiveAccountList(Boolean active)
		{
			try
			{
				accountList = service.Admin.GetAccountList(active);
			}
			catch (CoreError e)
			{
				error = e;
			}
		}

		[Then(@"the account list will (not )?have this")]
		public void ThenTheAccountListWillHaveThis(Boolean has, Table table)
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
					Assert.That(account, Is.Not.Null);
				}
				else
				{
					Assert.That(account, Is.Null);
				}
			}
		}
		#endregion

		#region ReopenAccount
		[When(@"I try to reopen the account")]
		public void WhenITryToReopenTheAccount()
		{
			try
			{
				service.Admin.ReopenAccount(accountUrl);
			}
			catch (CoreError e)
			{
				error = e;
			}
		}

		[Then(@"the account will be open")]
		public void ThenTheAccountWillBeOpen()
		{
			var url = accountUrl ?? accountInfo?.Name.IntoUrl();
			var account = service.Admin.GetAccount(url);
			Assert.That(account.IsOpen, Is.True);
		}
		#endregion
		#endregion Account

		#region Category
		#region SaveCategory
		[Given(@"I have this category to create")]
		public void GivenIHaveThisCategoryToCreate(Table table)
		{
			var categoryData = table.Rows[0];

			categoryInfo = new CategoryInfo
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

			service.Admin.CreateCategory(oldCategory);
		}

		[When(@"I try to save the category")]
		public void WhenITryToSaveTheCategory()
		{
			try
			{
				service.Admin.CreateCategory(categoryInfo);
			}
			catch (CoreError e)
			{
				error = e;
			}
		}

		[Then(@"the category will not be saved")]
		public void ThenTheCategoryWillNotBeSaved()
		{
			error = null;
			var name = categoryInfo.Name;
			categoryInfo = null;

			try
			{
				categoryInfo = service.Admin.GetCategory(name);
			}
			catch (CoreError e)
			{
				error = e;
			}

			Assert.That(error, Is.Not.Null);
			Assert.That(categoryInfo, Is.Null);
		}

		[Then(@"the category will be saved")]
		public void ThenTheCategoryWillBeSaved()
		{
			categoryInfo = service.Admin.GetCategory(categoryInfo.Name);

			Assert.That(categoryInfo, Is.Not.Null);
		}
		#endregion

		#region GetCategoryByName
		[Given(@"I pass a valid category name")]
		public void GivenIPassValidCategoryName()
		{
			categoryName = categoryInfo.Name;
		}

		[When(@"I try to get the category by its name")]
		public void WhenITryToGetTheCategoryByItsName()
		{
			categoryInfo = null;

			try
			{
				categoryInfo = service.Admin.GetCategory(categoryName);
			}
			catch (CoreError e)
			{
				error = e;
			}
		}
		#endregion

		#region UpdateCategory
		[Given(@"I have this category")]
		[Given(@"I have these categories")]
		public void GivenIHaveTheseCategories(Table table)
		{
			foreach (var row in table.Rows)
			{
				var name = row["Name"];
				categoryInfo = new CategoryInfo { Name = name };
				service.Admin.CreateCategory(categoryInfo);

				if (row.ContainsKey("Enabled") && !Boolean.Parse(row["Enabled"]))
				{
					service.Admin.DisableCategory(name);
				}
			}
		}

		[Given(@"I make this changes to the category")]
		public void WhenIMakeThisChangesToTheCategory(Table table)
		{
			var categoryData = table.Rows[0];

			categoryInfo.OriginalName = categoryInfo.Name;
			categoryInfo.Name = categoryData["Name"];
		}

		[When(@"I try to update the category")]
		public void WhenITryToUpdateTheCategory()
		{
			try
			{
				service.Admin.UpdateCategory(categoryInfo);
			}
			catch (CoreError e)
			{
				error = e;
			}
		}

		[Then(@"the category will (not )?be changed")]
		public void ThenTheCategoryWillBeChanged(Boolean changed)
		{
			CategoryInfo category = null;
			error = null;

			try
			{
				category = service.Admin.GetCategory(categoryInfo.OriginalName);
			}
			catch (CoreError e)
			{
				error = e;
			}

			if (!changed)
			{
				Assert.That(category, Is.Not.Null);
				Assert.That(error, Is.Null);
				return;
			}

			Assert.That(category, Is.Null);
			Assert.That(error, Is.Not.Null);
			Assert.That(Error.InvalidCategory, Is.EqualTo(error.Type));

			error = null;

			try
			{
				category = service.Admin.GetCategory(categoryInfo.Name);
			}
			catch (CoreError e)
			{
				error = e;
			}

			Assert.That(category, Is.Not.Null);
			Assert.That(error, Is.Null);
		}
		#endregion

		#region DisableCategory
		[Given(@"I give the enabled category ([\w ]+)")]
		public void GivenIGiveAnEnabledCategory(String givenCategoryName)
		{
			service.Admin.CreateCategory(
				new CategoryInfo
				{
					Name = givenCategoryName
				}
			);

			categoryInfo = service.Admin.GetCategory(givenCategoryName);

			categoryName = categoryInfo.Name;
		}

		[Given(@"I already have disabled the category")]
		public void GivenIDisableACategory()
		{
			service.Admin.DisableCategory(categoryName);
		}

		[When(@"I try to disable the category")]
		public void WhenITryToDisableTheCategory()
		{
			try
			{
				service.Admin.DisableCategory(categoryName);
			}
			catch (CoreError e)
			{
				error = e;
			}
		}

		[Then(@"the category will be disabled")]
		public void ThenTheCategoryWillBeDisabled()
		{
			var user = repos.User.GetByEmail(current.Email);
			var category = repos.Category.GetByName(categoryName, user);
			Assert.That(category.Active, Is.False);
		}
		#endregion

		#region EnableCategory
		[Given(@"I give the disabled category ([\w ]+)")]
		public void GivenIGiveADisabledCategory(String givenCategoryName)
		{
			service.Admin.CreateCategory(
				new CategoryInfo
				{
					Name = givenCategoryName
				}
			);

			categoryInfo = service.Admin.GetCategory(givenCategoryName);

			categoryName = categoryInfo.Name;

			service.Admin.DisableCategory(categoryName);
		}

		[Given(@"I already have enabled the category")]
		public void GivenIEnableACategory()
		{
			service.Admin.EnableCategory(categoryName);
		}

		[When(@"I try to enable the category")]
		public void WhenITryToEnableTheCategory()
		{
			try
			{
				service.Admin.EnableCategory(categoryName);
			}
			catch (CoreError e)
			{
				error = e;
			}
		}

		[Then(@"the category will be enabled")]
		public void ThenTheCategoryWillBeEnabled()
		{
			var user = repos.User.GetByEmail(current.Email);
			var category = repos.Category.GetByName(categoryName, user);
			Assert.That(category.Active, Is.True);
		}
		#endregion

		#region GetCategoryList
		[Given(@"I enable the category (.+)")]
		public void GivenIEnableTheCategory(String name)
		{
			var user = repos.User.GetByEmail(current.Email);
			var category = repos.Category.GetByName(name, user);

			if (!category.Active)
				service.Admin.EnableCategory(name);
		}

		[Given(@"I disable the category (.+)")]
		public void GivenIDisableTheCategory(String name)
		{
			var user = repos.User.GetByEmail(current.Email);
			var category = repos.Category.GetByName(name, user);

			if (category.Active)
				service.Admin.DisableCategory(name);
		}

		[When(@"ask for all the category list")]
		public void WhenAskForAllTheCategoryList()
		{
			try
			{
				categoryList = service.Admin.GetCategoryList();
			}
			catch (CoreError e)
			{
				error = e;
			}
		}

		[When(@"ask for the (not )?active category list")]
		public void WhenAskForTheActiveCategoryList(Boolean active)
		{
			try
			{
				categoryList = service.Admin.GetCategoryList(active);
			}
			catch (CoreError e)
			{
				error = e;
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
					Assert.That(category, Is.Not.Null);
				}
				else
				{
					Assert.That(category, Is.Null);
				}
			}
		}
		#endregion

		#region UnifyCategories
		[Given(@"category (.+) has a defective summary")]
		public void GivenCatBHasADefectiveSummary(String categoryName)
		{
			var user = repos.User.GetByEmail(userEmail);
			var category = repos.Category.GetByName(categoryName, user);
			var account = repos.Account.GetByUrl(accountInfo.Name.IntoUrl(), user);

			var summary = new Summary
			{
				Account = account,
				Category = category,
				In = 27,
				Nature = SummaryNature.Month,
				Time = 198603,
			};

			repos.Summary.SaveOrUpdate(summary);
		}

		[When(@"unify categories (.+) to (.+)")]
		public void WhenUnifyCategoriesCatAAndCatB(String categoryToDelete, String categoryToKeep)
		{
			try
			{
				service.Admin.UnifyCategory(categoryToKeep, categoryToDelete);
			}
			catch (CoreError e)
			{
				error = e;
			}
		}

		[Then(@"category (.+) will( not)? exist")]
		public void ThenCategoryCatBWillNotExist(String categoryName, Boolean exist)
		{
			var user = repos.User.GetByEmail(userEmail);
			var category = repos.Category.GetByName(categoryName, user);

			if (exist)
				Assert.That(category, Is.Not.Null);
			else
				Assert.That(category, Is.Null);
		}

		[Then(@"category (.+) will have (\d+) moves")]
		public void ThenCatAWillHaveMoves(String categoryName, Int32 count)
		{
			var user = repos.User.GetByEmail(userEmail);
			var category = repos.Category.GetByName(categoryName, user);
			var moves = repos.Move.ByCategory(category);

			Assert.That(moves.Count, Is.EqualTo(count));
		}

		[Then(@"category (.+) will have (\d+) schedules")]
		public void ThenCatAWillHaveSchedules(String categoryName, Int32 count)
		{
			var user = repos.User.GetByEmail(userEmail);
			var category = repos.Category.GetByName(categoryName, user);
			var schedules = repos.Schedule.ByCategory(category);

			Assert.That(schedules.Count, Is.EqualTo(count));
		}
		#endregion Unify Categories

		#endregion Category

		#region UpdateSettings

		[Given("these settings")]
		public void GivenTheseSettings(Table table)
		{
			var options = new InstanceCreationOptions
			{
				VerifyAllColumnsBound = true
			};

			var mainSettings = table.CreateInstance<SettingsInfo>(options);

			service.Clip.UpdateSettings(mainSettings);
		}

		[When("try update the settings")]
		public void WhenITryUpdateTheSettings(Table table)
		{
			try
			{
				var mainSettings = table.CreateInstance<SettingsInfo>();
				service.Clip.UpdateSettings(mainSettings);
			}
			catch (CoreError e)
			{
				error = e;
			}
		}

		[Then("the settings will be")]
		public void ThenTheSettingsWillBe(Table table)
		{
			var settings = table.CreateInstance<SettingsInfo>();

			var testUser = repos.User.GetByEmail(userEmail);

			if (settings.UseCategories.HasValue)
			{
				Assert.That(
					testUser.Settings.UseCategories,
					Is.EqualTo(settings.UseCategories.Value)
				);
			}

			if (settings.UseAccountsSigns.HasValue)
			{
				Assert.That(
					testUser.Settings.UseAccountsSigns,
					Is.EqualTo(settings.UseAccountsSigns.Value)
				);
			}

			if (settings.SendMoveEmail.HasValue)
			{
				Assert.That(
					testUser.Settings.SendMoveEmail,
					Is.EqualTo(settings.SendMoveEmail.Value)
				);
			}

			if (settings.MoveCheck.HasValue)
			{
				Assert.That(
					testUser.Settings.MoveCheck,
					Is.EqualTo(settings.MoveCheck.Value)
				);
			}

			if (settings.Wizard.HasValue)
			{
				Assert.That(
					testUser.Settings.Wizard,
					Is.EqualTo(settings.Wizard.Value)
				);
			}

			if (settings.UseCurrency.HasValue)
			{
				Assert.That(
					testUser.Settings.UseCurrency,
					Is.EqualTo(settings.UseCurrency.Value)
				);
			}

			if (settings.Language != null)
			{
				Assert.That(
					testUser.Settings.Language,
					Is.EqualTo(settings.Language)
				);
			}

			if (settings.TimeZone != null)
			{
				Assert.That(
					testUser.Settings.TimeZone,
					Is.EqualTo(settings.TimeZone)
				);
			}

		}

		[Then(@"the translation will be")]
		public void ThenTheTranslationWillBe(Table table)
		{
			foreach (var tableRow in table.Rows)
			{
				var translated = PlainText.Site[
					"general", service.Current.Language, tableRow["Key"]
				].Text;
				Assert.That(translated, Is.EqualTo(tableRow["Translated"]));
			}
		}

		[Then(@"the account list will( not)? have sign")]
		public void ThenTheAccountListWillHaveSign(Boolean hasSign)
		{
			var accountList = service.Admin.GetAccountList(true);
			var url = accountInfo.Name.IntoUrl();
			var account = accountList.Single(a => a.Url == url);

			if (hasSign)
				Assert.That(account.Sign, Is.Not.EqualTo(AccountSign.None));
			else
				Assert.That(account.Sign, Is.EqualTo(AccountSign.None));
		}

		[Then(@"the year report will( not)? have sign")]
		public void ThenTheYearReportWillNotHaveSign(Boolean hasSign)
		{
			var url = accountInfo.Name.IntoUrl();
			var year = (Int16)current.Now.Year;
			var yearReport = service.Report.GetYearReport(url, year);

			if (hasSign)
			{
				Assert.That(yearReport.AccountSign, Is.Not.EqualTo(AccountSign.None));
				Assert.That(yearReport.AccountForeseenSign, Is.Not.EqualTo(AccountSign.None));
			}
			else
			{
				Assert.That(yearReport.AccountSign, Is.EqualTo(AccountSign.None));
				Assert.That(yearReport.AccountForeseenSign, Is.EqualTo(AccountSign.None));
			}
		}

		[Then(@"the month report will( not)? have sign")]
		public void ThenTheMonthReportWillNotHaveSign(Boolean hasSign)
		{
			var url = accountInfo.Name.IntoUrl();
			var year = (Int16)current.Now.Year;
			var month = (Int16)current.Now.Month;
			var monthReport = service.Report.GetMonthReport(url, year, month);

			if (hasSign)
			{
				Assert.That(monthReport.AccountTotalSign, Is.Not.EqualTo(AccountSign.None));
				Assert.That(monthReport.ForeseenTotalSign, Is.Not.EqualTo(AccountSign.None));
			}
			else
			{
				Assert.That(monthReport.AccountTotalSign, Is.EqualTo(AccountSign.None));
				Assert.That(monthReport.ForeseenTotalSign, Is.EqualTo(AccountSign.None));
			}
		}
		#endregion

		#region ChangeTheme
		[Given(@"a theme (\w+)")]
		public void GivenAThemeSlate(Theme chosenTheme)
		{
			theme = chosenTheme;
		}

		[When(@"I try to change the Theme")]
		public void WhenITryToChangeTheTheme()
		{
			try
			{
				service.Clip.ChangeTheme(theme);
			}
			catch (CoreError e)
			{
				error = e;
			}
		}

		[Then(@"the Theme will be (\w+)")]
		public void ThenTheThemeWillBeSlate(Theme chosenTheme)
		{
			Assert.That(current.Theme, Is.EqualTo(chosenTheme));
		}
		#endregion ChangeTheme

		#region EndWizard
		[When(@"I end wizard")]
		public void WhenIEndWizard()
		{
			try
			{
				service.Clip.EndWizard();
			}
			catch (CoreError e)
			{
				error = e;
			}
		}
		#endregion EndWizard

		#region UnsubscribeMoveMail
		[When(@"I unsubscribe move mail")]
		public void WhenIUnsubscribeMoveMail()
		{
			try
			{
				service.Outside.UnsubscribeMoveMail(token);
			}
			catch (CoreError e)
			{
				error = e;
			}
		}

		[When(@"I unsubscribe move mail \(invalid token\)")]
		public void WhenIUnsubscribeMoveMailInvalidToken()
		{
			try
			{
				service.Outside.UnsubscribeMoveMail("invalid");
			}
			catch (CoreError e)
			{
				error = e;
			}
		}

		[Then(@"the move mail will( not)? be enabled")]
		public void ThenTheMoveMailWill_BeEnabled(Boolean enabled)
		{
			var user = repos.User.GetByEmail(userEmail);
			Assert.That(user.Settings.SendMoveEmail, Is.EqualTo(enabled));
		}

		[Then(@"the last two e-mails will have same unsubscribe token")]
		public void ThenTheLastTwoE_MailsWillHaveSameUnsubscribeToken()
		{
			var regex = new Regex("UnsubscribeMoveMail>(\\w+)");

			var email1 = EmlHelper.ByPosition(-1);
			var match1 = regex.Match(email1.Body);
			var token1 = match1.Groups[1].Value;

			var email2 = EmlHelper.ByPosition(-2);
			var match2 = regex.Match(email2.Body);
			var token2 = match2.Groups[1].Value;

			Assert.That(token2, Is.EqualTo(token1));
		}
		#endregion UnsubscribeMoveMail

		#region MoreThanOne
		[Given(@"I pass a url of account that doesn't exist")]
		public void GivenIPassAnNameOfAccountThatDoesNotExist()
		{
			accountUrl = "account_that_does_not_exist";
		}

		[Given(@"I pass a name of category that doesn't exist")]
		public void GivenIPassAnNameOfCategoryThatDoesNotExist()
		{
			categoryName = "Invalid category";
		}

		[Given(@"I give a url of the account ([\w]+) without moves")]
		public void GivenIGiveAnNameOfAnAccountWithoutMoves(String givenAccountUrl)
		{
			accountInfo = new AccountInfo
			{
				Name = givenAccountUrl,
			};

			service.Admin.CreateAccount(accountInfo);
			accountUrl = givenAccountUrl.IntoUrl();
		}

		[Given(@"I give a url of the account ([\w]+) with moves")]
		public void GivenIGiveAnIdOfAnAccountWithMoves(String givenAccountUrl)
		{
			accountInfo = new AccountInfo
			{
				Name = givenAccountUrl,
			};

			service.Admin.CreateAccount(accountInfo);

			accountUrl = givenAccountUrl.IntoUrl();

			var move = new MoveInfo
			{
				Description = "Move for account test",
				Nature = MoveNature.Out,
				Value = 10,
				OutUrl = accountUrl,
				CategoryName = categoryInfo?.Name,
			};

			move.SetDate(current.Now);

			service.Money.SaveMove(move);
		}

		[Given(@"the account has a schedule( with details)?")]
		public void GivenTheAccountHasSchedules(String withDetails)
		{
			scheduleInfo = new ScheduleInfo
			{
				Description = "Schedule for account test",
				Nature = MoveNature.Out,
				Frequency = ScheduleFrequency.Daily,
				Boundless = false,
				Times = 1,
				OutUrl = accountInfo.Name.IntoUrl(),
				CategoryName = categoryInfo?.Name,
			};

			scheduleInfo.SetDate(current.Now.AddDays(1));

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

			var schedule = service.Robot.SaveSchedule(scheduleInfo);
			scheduleInfo.Guid = schedule.Guid;
		}

		[Given(@"the account has a disabled schedule( with details)?")]
		public void GivenTheAccountHasADisabledSchedule(String withDetails)
		{
			scheduleInfo = new ScheduleInfo
			{
				Description = "Schedule for account test",
				Nature = MoveNature.Out,
				Frequency = ScheduleFrequency.Daily,
				Boundless = false,
				Times = 1,
				OutUrl = accountInfo.Name.IntoUrl(),
			};

			scheduleInfo.SetDate(current.Now.AddDays(1));

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

			var schedule = service.Robot.SaveSchedule(scheduleInfo);
			scheduleInfo.Guid = schedule.Guid;

			service.Robot.DisableSchedule(scheduleInfo.Guid);
		}

		[Then(@"I will receive no account")]
		public void ThenIWillReceiveNoAccount()
		{
			Assert.That(accountInfo, Is.Null);
		}

		[Then(@"I will receive the account")]
		public void ThenIWillReceiveTheAccount()
		{
			Assert.That(accountInfo, Is.Not.Null);

			if (accountUrl != null)
			{
				Assert.That(accountInfo.Name.IntoUrl(), Is.EqualTo(accountUrl));
			}
			else
			{
				throw new NotImplementedException();
			}
		}

		[Then(@"the account will (not )?have an end date")]
		public void ThenTheAccountWillHaveAnEndDate(Boolean hasEndDate)
		{
			var url = accountUrl ?? accountInfo?.Name.IntoUrl();
			var account = service.Admin.GetAccount(url);

			if (hasEndDate)
				Assert.That(account.End, Is.Not.Null);
			else
				Assert.That(account.End, Is.Null);
		}

		[Then(@"I will receive no category")]
		public void ThenIWillReceiveNoCategory()
		{
			Assert.That(categoryInfo, Is.Null);
		}

		[Then(@"I will receive the category")]
		public void ThenIWillReceiveTheCategory()
		{
			Assert.That(categoryInfo, Is.Not.Null);

			if (categoryName != null)
				Assert.That(categoryInfo.Name, Is.EqualTo(categoryName));
		}
		#endregion

	}
}
