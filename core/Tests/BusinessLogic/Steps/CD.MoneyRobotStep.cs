using System;
using System.Collections.Generic;
using DFM.BusinessLogic.Response;
using DFM.Entities;
using DFM.Entities.Bases;
using DFM.Entities.Enums;
using DFM.Generic;
using DFM.Generic.Datetime;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace DFM.BusinessLogic.Tests.Steps
{
	[Binding]
	public class MoneyRobotStep : BaseStep
	{
		public MoneyRobotStep(ScenarioContext context)
			: base(context) { }

		[Given(@"I have two accounts")]
		public void GivenIHaveTwoAccounts()
		{
			getOrCreateAccount(accountOutUrl);
			getOrCreateAccount(accountInUrl);
		}

		[Given(@"it has no Details")]
		public void GivenItHasNoDetails()
		{
			if (moveInfo != null)
				moveInfo.DetailList = new List<DetailInfo>();

			if (scheduleInfo != null)
				scheduleInfo.DetailList = new List<DetailInfo>();
		}

		[Given(@"it has a Category")]
		public void GivenItHasACategory()
		{
			categoryName = categoryInfo.Name;
		}

		[Given(@"it has no Category")]
		public void GivenItHasNoCategory()
		{
			categoryName = null;
			categoryInfo = null;
		}

		[Given(@"it has an unknown Category")]
		public void GivenItHasAnUnknownCategory()
		{
			categoryName = "unknown";
		}

		[Given(@"it has a disabled Category")]
		public void GivenItHasADisabledCategory()
		{
			categoryName = "disabled";

			var moveCategory = new CategoryInfo
			{
				Name = "disabled"
			};

			service.Admin.CreateCategory(moveCategory);
			service.Admin.DisableCategory(moveCategory.Name);
		}


		[Given(@"it has an Account Out")]
		public void GivenItHasAnAccountOut()
		{
			accountOut = getOrCreateAccount(accountOutUrl);

			accountOutTotal = repos.Summary.GetTotal(accountOut);

			var user = repos.User.GetByEmail(current.Email);
			var category = repos.Category.GetByName(categoryName, user);

			yearCategoryAccountOutTotal =
				repos.Summary.Get(
					accountOut, category, entityDate.Year
				)?.Out ?? 0;
			monthCategoryAccountOutTotal =
				repos.Summary.Get(
					accountOut, category, entityDate.ToMonthYear()
				)?.Out ?? 0;
		}

		[Given(@"it has no Account Out")]
		public void GivenItHasNoAccountOut()
		{
			accountOut = null;
		}

		[Given(@"it has an unknown Account Out")]
		public void GivenItHasAnUnknownAccountOut()
		{
			var user = repos.User.GetByEmail(current.Email);

			accountOut = new Account
			{
				Name = "unknown",
				Url = "unknown",
				User = user
			};
		}

		[Given(@"it has a closed Account Out")]
		public void GivenItHasAClosedAccountOut()
		{
			var url = "closed out".IntoUrl();

			var account = new AccountInfo
			{
				Name = "closed out",
			};

			service.Admin.CreateAccount(account);
			var user = repos.User.GetByEmail(current.Email);
			accountOut = repos.Account.GetByUrl(url, user);

			var move = new MoveInfo
			{
				Description = "Description",
				Nature = MoveNature.Out,
				Value = 10,
				OutUrl = accountOut.Url,
				CategoryName = categoryInfo.Name,
			};

			move.SetDate(current.Now);

			service.Money.SaveMove(move);

			service.Admin.CloseAccount(accountOut.Url);
		}


		[Given(@"it has an Account In")]
		public void GivenItHasAnAccountIn()
		{
			accountIn = getOrCreateAccount(accountInUrl);

			accountInTotal = repos.Summary.GetTotal(accountIn);

			var user = repos.User.GetByEmail(current.Email);
			var category = repos.Category.GetByName(categoryName, user);

			yearCategoryAccountInTotal =
				repos.Summary.Get(
					accountIn, category, entityDate.Year
				)?.In ?? 0;
			monthCategoryAccountInTotal =
				repos.Summary.Get(
					accountIn, category, entityDate.ToMonthYear()
				)?.In ?? 0;
		}

		[Given(@"it has no Account In")]
		public void GivenItHasNoAccountIn()
		{
			accountIn = null;
		}

		[Given(@"it has an unknown Account In")]
		public void GivenItHasAnUnknownAccountIn()
		{
			var user = repos.User.GetByEmail(current.Email);

			accountIn = new Account
			{
				Name = "unknown",
				Url = "unknown",
				User = user
			};
		}

		[Given(@"it has a closed Account In")]
		public void GivenItHasAClosedAccountIn()
		{
			var url = "closed in".IntoUrl();

			var account = new AccountInfo
			{
				Name = "closed in",
			};

			service.Admin.CreateAccount(account);

			var user = repos.User.GetByEmail(current.Email);
			accountIn = repos.Account.GetByUrl(url, user);

			var move = new MoveInfo
			{
				Description = "Description",
				Nature = MoveNature.In,
				Value = 10,
				InUrl = accountIn.Url,
				CategoryName = categoryInfo.Name,
			};

			move.SetDate(current.Now);

			service.Money.SaveMove(move);

			service.Admin.CloseAccount(accountIn.Url);
		}

		[Given(@"it has an Account In equal to Out")]
		public void GivenItHasAnAccountInEqualToOut()
		{
			accountOut = getOrCreateAccount(accountOutUrl);
			accountIn = accountOut;

			accountInTotal = repos.Summary.GetTotal(accountIn);

			var user = repos.User.GetByEmail(current.Email);
			var category = repos.Category.GetByName(categoryName, user);

			yearCategoryAccountInTotal =
				repos.Summary.Get(
					accountIn, category, entityDate.Year
				)?.In ?? 0;
			monthCategoryAccountInTotal =
				repos.Summary.Get(
					accountIn, category, entityDate.ToMonthYear()
				)?.In ?? 0;

			yearCategoryAccountOutTotal =
				repos.Summary.Get(
					accountOut, category, entityDate.Year
				)?.Out ?? 0;
			monthCategoryAccountOutTotal =
				repos.Summary.Get(
					accountOut, category, entityDate.ToMonthYear()
				)?.Out ?? 0;
		}

		[Then(@"the accountOut value will not change")]
		public void ThenTheAccountOutValueWillNotChange()
		{
			var url = accountOut?.Url ?? accountOutUrl;
			accountOut = getOrCreateAccount(url);

			Assert.AreEqual(accountOutTotal, repos.Summary.GetTotal(accountOut));
		}

		[Then(@"the month-category-accountOut value will not change")]
		public void ThenTheMonthCategoryAccountOutValueWillNotChange()
		{
			var url = accountOut?.Url ?? accountOutUrl;
			accountOut = getOrCreateAccount(url);

			var user = repos.User.GetByEmail(current.Email);
			var category = repos.Category.GetByName(categoryName, user);

			var currentTotal = repos.Summary.Get(
				accountOut, category, entityDate.ToMonthYear()
			)?.Out ?? 0;

			Assert.AreEqual(monthCategoryAccountOutTotal, currentTotal);
		}

		[Then(@"the year-category-accountOut value will not change")]
		public void ThenTheYearCategoryAccountOutValueWillNotChange()
		{
			var url = accountOut?.Url ?? accountOutUrl;
			accountOut = getOrCreateAccount(url);

			var user = repos.User.GetByEmail(current.Email);
			var category = repos.Category.GetByName(categoryName, user);

			var currentTotal = repos.Summary.Get(
				accountOut, category, entityDate.Year
			)?.Out ?? 0;

			Assert.AreEqual(yearCategoryAccountOutTotal, currentTotal);
		}

		[Then(@"the accountIn value will not change")]
		public void ThenTheAccountInValueWillNotChange()
		{
			var url = accountIn?.Url ?? accountInUrl;
			accountIn = getOrCreateAccount(url);

			Assert.AreEqual(accountInTotal, repos.Summary.GetTotal(accountIn));
		}

		[Then(@"the month-category-accountIn value will not change")]
		public void ThenTheMonthCategoryAccountInValueWillNotChange()
		{
			var url = accountIn?.Url ?? accountInUrl;
			accountIn = getOrCreateAccount(url);

			var user = repos.User.GetByEmail(current.Email);
			var category = repos.Category.GetByName(categoryName, user);

			var currentTotal = repos.Summary.Get(
				accountIn, category, entityDate.ToMonthYear()
			)?.In ?? 0;

			Assert.AreEqual(monthCategoryAccountInTotal, currentTotal);
		}

		[Then(@"the year-category-accountIn value will not change")]
		public void ThenTheYearCategoryAccountInValueWillNotChange()
		{
			var url = accountIn?.Url ?? accountInUrl;
			accountIn = getOrCreateAccount(url);

			var user = repos.User.GetByEmail(current.Email);
			var category = repos.Category.GetByName(categoryName, user);

			var currentTotal = repos.Summary.Get(
				accountIn, category, entityDate.Year
			)?.In ?? 0;

			Assert.AreEqual(yearCategoryAccountInTotal, currentTotal);
		}

		[Then(@"the accountOut value will change in (\-?\d+\.?\d*)")]
		public void ThenTheAccountOutValueWillDecreaseIn(Decimal change)
		{
			var url = accountOut?.Url ?? accountOutUrl;
			accountOut = getOrCreateAccount(url);

			var currentTotal = repos.Summary.GetTotal(accountOut);

			Assert.AreEqual(accountOutTotal + change, currentTotal);
		}

		[Then(@"the month-category-accountOut value will change in (\-?\d+\.?\d*)")]
		public void ThenTheMonthCategoryAccountOutValueWillChangeIn(Decimal change)
		{
			var url = accountOut?.Url ?? accountOutUrl;
			accountOut = getOrCreateAccount(url);

			var user = repos.User.GetByEmail(current.Email);
			var category = repos.Category.GetByName(categoryName, user);

			var currentTotal = repos.Summary.Get(
				accountOut, category, entityDate.ToMonthYear()
			)?.Out ?? 0;

			Assert.AreEqual(monthCategoryAccountOutTotal + change, currentTotal);
		}

		[Then(@"the year-category-accountOut value will change in (\-?\d+\.?\d*)")]
		public void ThenTheYearCategoryAccountOutValueWillChangeIn(Decimal change)
		{
			var url = accountOut?.Url ?? accountOutUrl;
			accountOut = getOrCreateAccount(url);

			var user = repos.User.GetByEmail(current.Email);
			var category = repos.Category.GetByName(categoryName, user);

			var currentTotal = repos.Summary.Get(
				accountOut, category, entityDate.Year
			)?.Out ?? 0;

			Assert.AreEqual(yearCategoryAccountOutTotal + change, currentTotal);
		}


		[Then(@"the accountIn value will change in (\-?\d+\.?\d*)")]
		public void ThenTheAccountInValueWillIncreaseIn(Decimal change)
		{
			var url = accountIn?.Url ?? accountInUrl;
			accountIn = getOrCreateAccount(url);

			var currentTotal = repos.Summary.GetTotal(accountIn);

			Assert.AreEqual(accountInTotal + change, currentTotal);
		}

		[Then(@"the month-category-accountIn value will change in (\-?\d+\.?\d*)")]
		public void ThenTheMonthCategoryAccountInValueWillChangeIn(Decimal change)
		{
			var url = accountIn?.Url ?? accountInUrl;
			accountIn = getOrCreateAccount(url);

			var user = repos.User.GetByEmail(current.Email);
			var category = repos.Category.GetByName(categoryName, user);

			var currentTotal = repos.Summary.Get(
				accountIn, category, entityDate.ToMonthYear()
			)?.In ?? 0;

			Assert.AreEqual(monthCategoryAccountInTotal + change, currentTotal);
		}

		[Then(@"the year-category-accountIn value will change in (\-?\d+\.?\d*)")]
		public void ThenTheYearCategoryAccountInValueWillChangeIn(Decimal change)
		{
			var url = accountIn?.Url ?? accountInUrl;
			accountIn = getOrCreateAccount(url);

			var user = repos.User.GetByEmail(current.Email);
			var category = repos.Category.GetByName(categoryName, user);

			var currentTotal = repos.Summary.Get(
				accountIn, category, entityDate.Year
			)?.In ?? 0;

			Assert.AreEqual(yearCategoryAccountInTotal + change, currentTotal);
		}
	}
}
