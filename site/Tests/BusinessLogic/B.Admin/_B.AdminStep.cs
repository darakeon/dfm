using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using DFM.BusinessLogic.Exceptions;
using DFM.BusinessLogic.Response;
using DFM.Entities;
using DFM.Entities.Bases;
using DFM.Entities.Enums;
using DFM.Generic;
using DFM.Language;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace DFM.BusinessLogic.Tests.B.Admin
{
	[Binding]
	public class AdminStep : BaseStep
	{
		#region Variables
		private static AccountInfo oldAccount
		{
			get => get<AccountInfo>("oldAccount");
			set => set("oldAccount", value);
		}

		private static Decimal accountTotal
		{
			get => get<Decimal>("accountTotal");
			set => set("accountTotal", value);
		}

		private static IList<AccountListItem> accountList
		{
			get => get<IList<AccountListItem>>("accountList");
			set => set("accountList", value);
		}

		private static IList<CategoryListItem> categoryList
		{
			get => get<IList<CategoryListItem>>("categoryList");
			set => set("categoryList", value);
		}



		private static CategoryInfo oldCategory
		{
			get => get<CategoryInfo>("oldCategory");
			set => set("oldCategory", value);
		}


		private static Theme theme
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
				Name = accountData["Name"],
				Url = accountData["Url"],
				RedLimit = getInt(accountData["Red"]),
				YellowLimit = getInt(accountData["Yellow"]),
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
				oldAccount.RedLimit = getInt(accountData["Red"]);

			if (accountData.ContainsKey("Yellow"))
				oldAccount.YellowLimit = getInt(accountData["Yellow"]);

			service.Admin.CreateAccount(oldAccount);
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
			var account = service.Admin.GetAccount(oldAccount.Url);

			Assert.AreEqual(oldAccount.Name, account.Name);
			Assert.AreEqual(oldAccount.Url.ToLower(), account.Url);
			Assert.AreEqual(oldAccount.RedLimit, account.RedLimit);
			Assert.AreEqual(oldAccount.YellowLimit, account.YellowLimit);
		}

		[Then(@"the account will not be saved")]
		public void ThenTheAccountWillNotBeSaved()
		{
			error = null;
			var url = accountInfo.Url;
			accountInfo = null;

			try
			{
				service.Admin.GetAccount(url);
			}
			catch (CoreError e)
			{
				error = e;
			}

			Assert.IsNull(accountInfo);
			Assert.IsNotNull(error);
			Assert.AreEqual(Error.InvalidAccount, error.Type);
		}

		[Then(@"the account will be saved")]
		public void ThenTheAccountWillBeSaved()
		{
			accountInfo = service.Admin.GetAccount(accountInfo.Url);

			Assert.IsNotNull(accountInfo);
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
				Name = accountData["Name"],
				Url = accountData["Url"],
			};

			service.Admin.CreateAccount(accountInfo);
		}

		[Given(@"this account has moves")]
		public void ThisAccountHasMoves()
		{
			moveInfo = new MoveInfo
			{
				Description = "Description",
				Nature = MoveNature.Out,
				OutUrl = accountInfo.Url,
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
			var account = repos.Account.GetByUrl(accountInfo.Url, user);
			accountTotal = repos.Summary.GetTotal(account);
		}

		[When(@"I make this changes to the account")]
		public void WhenIMakeThisChangesToTheAccount(Table table)
		{
			var accountData = table.Rows[0];

			accountInfo = new AccountInfo
			{
				OriginalUrl = (accountInfo ?? oldAccount).Url,
				Url = accountData["Url"],
				Name = accountData["Name"],
			};
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

			if (accountInfo.Url != accountInfo.OriginalUrl)
			{
				try
				{
					account = service.Admin.GetAccount(accountInfo.OriginalUrl);
				}
				catch (CoreError e)
				{
					error = e;
				}

				Assert.IsNull(account);
				Assert.IsNotNull(error);
				Assert.AreEqual(error.Type, Error.InvalidAccount);

				error = null;
			}

			try
			{
				account = service.Admin.GetAccount(accountInfo.Url);
			}
			catch (CoreError e)
			{
				error = e;
			}

			Assert.IsNotNull(account);
			Assert.IsNull(error);

			Assert.AreEqual(accountInfo.Url, account.Url);
		}

		[Then(@"the account value will not change")]
		public void TheAccountValueWillNotChange()
		{
			var user = repos.User.GetByEmail(current.Email);
			var account = repos.Account.GetByUrl(accountInfo.Url, user);
			var total = repos.Summary.GetTotal(account);
			Assert.AreEqual(accountTotal, total);
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
			Assert.IsTrue(account.Open);
		}

		[Then(@"the account will be closed")]
		public void ThenTheAccountWillBeClosed()
		{
			var user = repos.User.GetByEmail(current.Email);
			var account = repos.Account.GetByUrl(accountUrl, user);
			Assert.IsFalse(account.Open);
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
			var account = repos.Account.GetByUrl(givenAccountUrl, user);
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

			Assert.IsNotNull(accountInfo);
		}

		[Then(@"the account will be deleted")]
		public void ThenTheAccountWillBeDeleted()
		{
			error = null;
			var url = accountInfo.Url;
			accountInfo = null;

			try
			{
				accountInfo = service.Admin.GetAccount(url);
			}
			catch (CoreError e)
			{
				error = e;
			}

			Assert.IsNull(accountInfo);
			Assert.IsNotNull(error);
			Assert.AreEqual(Error.InvalidAccount, error.Type);
		}
		#endregion

		#region GetAccountList
		[Given(@"I close the account (.+)")]
		public void GivenICloseTheAccount(String url)
		{
			service.Admin.CloseAccount(url);
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
			var url = accountUrl ?? accountInfo?.Url;
			var account = service.Admin.GetAccount(url);
			Assert.IsTrue(account.IsOpen);
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

			Assert.IsNotNull(error);
			Assert.IsNull(categoryInfo);
		}

		[Then(@"the category will be saved")]
		public void ThenTheCategoryWillBeSaved()
		{
			categoryInfo = service.Admin.GetCategory(categoryInfo.Name);

			Assert.IsNotNull(categoryInfo);
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
		public void GivenIHaveThisCategory(Table table)
		{
			var categoryData = table.Rows[0];

			categoryInfo = new CategoryInfo
			{
				Name = categoryData["Name"],
			};

			service.Admin.CreateCategory(categoryInfo);
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
				Assert.IsNotNull(category);
				Assert.IsNull(error);
				return;
			}

			Assert.IsNull(category);
			Assert.IsNotNull(error);
			Assert.AreEqual(error.Type, Error.InvalidCategory);

			error = null;

			try
			{
				category = service.Admin.GetCategory(categoryInfo.Name);
			}
			catch (CoreError e)
			{
				error = e;
			}

			Assert.IsNotNull(category);
			Assert.IsNull(error);
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
			Assert.IsFalse(category.Active);
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
			Assert.IsTrue(category.Active);
		}
		#endregion

		#region GetCategoryList
		[Given(@"I disable the category (.+)")]
		public void GivenICloseTheCategory(String name)
		{
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
					Assert.IsNotNull(category);
				}
				else
				{
					Assert.IsNull(category);
				}
			}
		}
		#endregion
		#endregion Category

		#region UpdateConfig

		[Given(@"I disable Categories use")]
		[When(@"I try to disable Categories use")]
		public void GivenIDisableCategoriesUse()
		{
			try
			{
				var mainConfig = new ConfigInfo { UseCategories = false };
				service.Admin.UpdateConfig(mainConfig);
			}
			catch (CoreError e)
			{
				error = e;
			}
		}

		[Given(@"I enable Categories use")]
		[When(@"I try to enable Categories use")]
		public void GivenIEnableCategoriesUse()
		{
			try
			{
				var mainConfig = new ConfigInfo { UseCategories = true };
				service.Admin.UpdateConfig(mainConfig);
			}
			catch (CoreError e)
			{
				error = e;
			}
		}

		[Given(@"I disable move check")]
		[When(@"I try to disable move check")]
		public void GivenIDisableMoveCheck()
		{
			try
			{
				var mainConfig = new ConfigInfo { MoveCheck = false };
				service.Admin.UpdateConfig(mainConfig);
			}
			catch (CoreError e)
			{
				error = e;
			}
		}

		[Given(@"I enable move check")]
		[When(@"I try to enable move check")]
		public void GivenIEnableMoveCheck()
		{
			try
			{
				var mainConfig = new ConfigInfo { MoveCheck = true };
				service.Admin.UpdateConfig(mainConfig);
			}
			catch (CoreError e)
			{
				error = e;
			}
		}

		[Given(@"I disable wizard")]
		[When(@"I try to disable wizard")]
		public void GivenIDisableWizard()
		{
			try
			{
				var mainConfig = new ConfigInfo { Wizard = false };
				service.Admin.UpdateConfig(mainConfig);
			}
			catch (CoreError e)
			{
				error = e;
			}
		}

		[Given(@"I enable wizard")]
		[When(@"I try to enable wizard")]
		public void GivenIEnableWizard()
		{
			try
			{
				var mainConfig = new ConfigInfo { Wizard = true };
				service.Admin.UpdateConfig(mainConfig);
			}
			catch (CoreError e)
			{
				error = e;
			}
		}

		[When(@"I try to change the language to ([a-z]{2}-[A-Z]{2})")]
		public void WhenITryToChangeTheLanguageTo(String language)
		{
			try
			{
				var mainConfig = new ConfigInfo { Language = language };
				service.Admin.UpdateConfig(mainConfig);
			}
			catch (CoreError e)
			{
				error = e;
			}
		}

		[Then(@"the translation will be")]
		public void ThenTheTranslationWillBe(Table table)
		{
			foreach (var tableRow in table.Rows)
			{
				var translated = PlainText.Site["general", service.Current.Language, tableRow["Key"]];
				Assert.AreEqual(tableRow["Translated"], translated);
			}
		}

		[When(@"I try to change the timezone to (.+)")]
		public void WhenITryToChangeTheTimeZoneTo(String timeZone)
		{
			try
			{
				var mainConfig = new ConfigInfo { TimeZone = timeZone };
				service.Admin.UpdateConfig(mainConfig);
			}
			catch (CoreError e)
			{
				error = e;
			}
		}

		[Given(@"I disable move send e-mail")]
		[When(@"I try to disable move send e-mail")]
		public void GivenIDisableMoveSendEmail()
		{
			try
			{
				var mainConfig = new ConfigInfo { SendMoveEmail = false };
				service.Admin.UpdateConfig(mainConfig);
			}
			catch (CoreError e)
			{
				error = e;
			}
		}

		[Given(@"I enable move send e-mail")]
		[When(@"I try to enable move send e-mail")]
		public void GivenIEnableMoveSendEmail()
		{
			try
			{
				var mainConfig = new ConfigInfo { SendMoveEmail = true };
				service.Admin.UpdateConfig(mainConfig);
			}
			catch (CoreError e)
			{
				error = e;
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
				service.Admin.ChangeTheme(theme);
            }
			catch (CoreError e)
			{
				error = e;
			}
		}

		[Then(@"the Theme will be (\w+)")]
		public void ThenTheThemeWillBeSlate(Theme chosenTheme)
		{
			Assert.AreEqual(chosenTheme, current.Theme);
		}
		#endregion ChangeTheme

		#region EndWizard
		[When(@"I end wizard")]
		public void WhenIEndWizard()
		{
			try
			{
				service.Admin.EndWizard();
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
				service.Admin.UnsubscribeMoveMail(token);
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
				service.Admin.UnsubscribeMoveMail("invalid");
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
			Assert.AreEqual(enabled, user.Config.SendMoveEmail);
		}

		[Then(@"the last two e-mails will have same unsubscribe token")]
		public void ThenTheLastTwoE_MailsWillHaveSameUnsubscribeToken()
		{
			var regex = new Regex("UnsubscribeMoveMail>(\\w+)");

			var email1 = Email.GetByPosition(-1);
			var match1 = regex.Match(email1.Body);
			var token1 = match1.Groups[1].Value;

			var email2 = Email.GetByPosition(-2);
			var match2 = regex.Match(email2.Body);
			var token2 = match2.Groups[1].Value;

			Assert.AreEqual(token1, token2);
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
				Url = givenAccountUrl,
			};

			service.Admin.CreateAccount(accountInfo);

			accountUrl = accountInfo.Url;
		}

		[Given(@"I give a url of the account ([\w]+) with moves")]
		public void GivenIGiveAnIdOfAnAccountWithMoves(String givenAccountUrl)
		{
			accountInfo = new AccountInfo
			{
				Name = givenAccountUrl,
				Url = givenAccountUrl,
			};

			service.Admin.CreateAccount(accountInfo);

			var move = new MoveInfo
			{
				Description = "Move for account test",
				Nature = MoveNature.Out,
				Value = 10,
				OutUrl = accountInfo.Url,
				CategoryName = categoryInfo?.Name,
			};

			move.SetDate(current.Now);

			service.Money.SaveMove(move);

			accountUrl = accountInfo.Url;
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
				OutUrl = accountInfo.Url,
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
				OutUrl = accountInfo.Url,
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
			Assert.IsNull(accountInfo);
		}

		[Then(@"I will receive the account")]
		public void ThenIWillReceiveTheAccount()
		{
			Assert.IsNotNull(accountInfo);

			if (accountUrl != null)
			{
				Assert.AreEqual(accountUrl, accountInfo.Url);
			}
			else
			{
				throw new NotImplementedException();
			}
		}

		[Then(@"the account will (not )?have an end date")]
		public void ThenTheAccountWillHaveAnEndDate(Boolean hasEndDate)
		{
			var url = accountUrl ?? accountInfo?.Url;
			var account = service.Admin.GetAccount(url);

			if (hasEndDate)
				Assert.IsNotNull(account.End);
			else
				Assert.IsNull(account.End);
		}

		[Then(@"I will receive no category")]
		public void ThenIWillReceiveNoCategory()
		{
			Assert.IsNull(categoryInfo);
		}

		[Then(@"I will receive the category")]
		public void ThenIWillReceiveTheCategory()
		{
			Assert.IsNotNull(categoryInfo);

			if (categoryName != null)
				Assert.AreEqual(categoryName, categoryInfo.Name);
		}
		#endregion

	}
}
