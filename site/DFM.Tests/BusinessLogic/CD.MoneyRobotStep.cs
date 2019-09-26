using System;
using System.Collections.Generic;
using DFM.BusinessLogic.Response;
using DFM.Entities;
using DFM.Entities.Bases;
using DFM.Entities.Enums;
using DFM.Generic;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace DFM.Tests.BusinessLogic
{
	[Binding]
	public class MoneyRobotStep : BaseStep
	{
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

			accountOutTotal = summaryRepository.GetTotal(accountOut);

			var user = userRepository.GetByEmail(current.Email);
			var category = categoryRepository.GetByName(categoryName, user);

			yearCategoryAccountOutTotal =
				summaryRepository.Get(
					accountOut, category, date.Year
				)?.Out ?? 0;
			monthCategoryAccountOutTotal =
				summaryRepository.Get(
					accountOut, category, date.ToMonthYear()
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
			var user = userRepository.GetByEmail(current.Email);

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
			var url = makeUrlFromName("closed out");

			var account = new AccountInfo
			{
				Name = "closed out",
				Url = url,
			};

			service.Admin.CreateAccount(account);
			var user = userRepository.GetByEmail(current.Email);
			accountOut = accountRepository.GetByUrl(url, user);

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

			accountInTotal = summaryRepository.GetTotal(accountIn);

			var user = userRepository.GetByEmail(current.Email);
			var category = categoryRepository.GetByName(categoryName, user);

			yearCategoryAccountInTotal =
				summaryRepository.Get(
					accountIn, category, date.Year
				)?.In ?? 0;
			monthCategoryAccountInTotal =
				summaryRepository.Get(
					accountIn, category, date.ToMonthYear()
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
			var user = userRepository.GetByEmail(current.Email);

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
			var url = makeUrlFromName("closed in");

			var account = new AccountInfo
			{
				Name = "closed in",
				Url = url,
			};
			 
			service.Admin.CreateAccount(account);

			var user = userRepository.GetByEmail(current.Email);
			accountIn = accountRepository.GetByUrl(url, user);

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

			accountInTotal = summaryRepository.GetTotal(accountIn);

			var user = userRepository.GetByEmail(current.Email);
			var category = categoryRepository.GetByName(categoryName, user);

			yearCategoryAccountInTotal =
				summaryRepository.Get(
					accountIn, category, date.Year
				)?.In ?? 0;
			monthCategoryAccountInTotal =
				summaryRepository.Get(
					accountIn, category, date.ToMonthYear()
				)?.In ?? 0;

			yearCategoryAccountOutTotal =
				summaryRepository.Get(
					accountOut, category, date.Year
				)?.Out ?? 0;
			monthCategoryAccountOutTotal =
				summaryRepository.Get(
					accountOut, category, date.ToMonthYear()
				)?.Out ?? 0;
		}



		[Then(@"the accountOut value will not change")]
		public void ThenTheAccountOutValueWillNotChange()
		{
			accountOut = getOrCreateAccount(accountOut.Url);

			Assert.AreEqual(accountOutTotal, summaryRepository.GetTotal(accountOut));
		}

		[Then(@"the month-category-accountOut value will not change")]
		public void ThenTheMonthCategoryAccountOutValueWillNotChange()
		{
			accountOut = getOrCreateAccount(accountOut.Name);

			var user = userRepository.GetByEmail(current.Email);
			var category = categoryRepository.GetByName(categoryName, user);

			var currentTotal = summaryRepository.Get(
				accountOut, category, date.ToMonthYear()
			)?.Out ?? 0;

			Assert.AreEqual(monthCategoryAccountOutTotal, currentTotal);
		}

		[Then(@"the year-category-accountOut value will not change")]
		public void ThenTheYearCategoryAccountOutValueWillNotChange()
		{
			accountOut = getOrCreateAccount(accountOut.Name);

			var user = userRepository.GetByEmail(current.Email);
			var category = categoryRepository.GetByName(categoryName, user);

			var currentTotal = summaryRepository.Get(
				accountOut, category, date.Year
			)?.Out ?? 0;

			Assert.AreEqual(yearCategoryAccountOutTotal, currentTotal);
		}

		[Then(@"the accountIn value will not change")]
		public void ThenTheAccountInValueWillNotChange()
		{
			accountIn = getOrCreateAccount(accountIn.Name);

			Assert.AreEqual(accountInTotal, summaryRepository.GetTotal(accountIn));
		}

		[Then(@"the month-category-accountIn value will not change")]
		public void ThenTheMonthCategoryAccountInValueWillNotChange()
		{
			accountIn = getOrCreateAccount(accountIn.Name);

			var user = userRepository.GetByEmail(current.Email);
			var category = categoryRepository.GetByName(categoryName, user);

			var currentTotal = summaryRepository.Get(
				accountIn, category, date.ToMonthYear()
			)?.In ?? 0;

			Assert.AreEqual(monthCategoryAccountInTotal, currentTotal);
		}

		[Then(@"the year-category-accountIn value will not change")]
		public void ThenTheYearCategoryAccountInValueWillNotChange()
		{
			accountIn = getOrCreateAccount(accountIn.Name);

			var user = userRepository.GetByEmail(current.Email);
			var category = categoryRepository.GetByName(categoryName, user);

			var currentTotal = summaryRepository.Get(
				accountIn, category, date.Year
			)?.In ?? 0;

			Assert.AreEqual(yearCategoryAccountInTotal, currentTotal);
		}


		[Then(@"the accountOut value will change in (\-?\d+\.?\d*)")]
		public void ThenTheAccountOutValueWillDecreaseIn(Decimal change)
		{
			accountOut = getOrCreateAccount(accountOut.Name);

			var currentTotal = summaryRepository.GetTotal(accountOut);

			Assert.AreEqual(accountOutTotal + change, currentTotal);
		}

		[Then(@"the month-category-accountOut value will change in (\-?\d+\.?\d*)")]
		public void ThenTheMonthCategoryAccountOutValueWillChangeIn(Decimal change)
		{
			accountOut = getOrCreateAccount(accountOut.Name);

			var user = userRepository.GetByEmail(current.Email);
			var category = categoryRepository.GetByName(categoryName, user);

			var currentTotal = summaryRepository.Get(
				accountOut, category, date.ToMonthYear()
			)?.Out ?? 0;

			Assert.AreEqual(monthCategoryAccountOutTotal + change, currentTotal);
		}

		[Then(@"the year-category-accountOut value will change in (\-?\d+\.?\d*)")]
		public void ThenTheYearCategoryAccountOutValueWillChangeIn(Decimal change)
		{
			accountOut = getOrCreateAccount(accountOut.Name);

			var user = userRepository.GetByEmail(current.Email);
			var category = categoryRepository.GetByName(categoryName, user);

			var currentTotal = summaryRepository.Get(
				accountOut, category, date.Year
			)?.Out ?? 0;

			Assert.AreEqual(yearCategoryAccountOutTotal + change, currentTotal);
		}


		[Then(@"the accountIn value will change in (\-?\d+\.?\d*)")]
		public void ThenTheAccountInValueWillIncreaseIn(Decimal change)
		{
			accountIn = getOrCreateAccount(accountIn.Name);

			var currentTotal = summaryRepository.GetTotal(accountIn);

			Assert.AreEqual(accountInTotal + change, currentTotal);
		}

		[Then(@"the month-category-accountIn value will change in (\-?\d+\.?\d*)")]
		public void ThenTheMonthCategoryAccountInValueWillChangeIn(Decimal change)
		{
			accountIn = getOrCreateAccount(accountIn.Name);

			var user = userRepository.GetByEmail(current.Email);
			var category = categoryRepository.GetByName(categoryName, user);

			var currentTotal = summaryRepository.Get(
				accountIn, category, date.ToMonthYear()
			)?.In ?? 0;

			Assert.AreEqual(monthCategoryAccountInTotal + change, currentTotal);
		}

		[Then(@"the year-category-accountIn value will change in (\-?\d+\.?\d*)")]
		public void ThenTheYearCategoryAccountInValueWillChangeIn(Decimal change)
		{
			accountIn = getOrCreateAccount(accountIn.Name);

			var user = userRepository.GetByEmail(current.Email);
			var category = categoryRepository.GetByName(categoryName, user);

			var currentTotal = summaryRepository.Get(
				accountIn, category, date.Year
			)?.In ?? 0;

			Assert.AreEqual(yearCategoryAccountInTotal + change, currentTotal);
		}
	}
}
