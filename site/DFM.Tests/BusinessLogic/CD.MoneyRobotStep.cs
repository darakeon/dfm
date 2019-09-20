using System;
using System.Collections.Generic;
using DFM.BusinessLogic.Response;
using DFM.Entities;
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
			GetOrCreateAccount(AccountOutUrl);
			GetOrCreateAccount(AccountInUrl);
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
			CategoryName = Category.Name;
		}

		[Given(@"it has no Category")]
		public void GivenItHasNoCategory()
		{
			CategoryName = null;
			Category = null;
		}

		[Given(@"it has an unknown Category")]
		public void GivenItHasAnUnknownCategory()
		{
			CategoryName = "unknown";
		}

		[Given(@"it has a disabled Category")]
		public void GivenItHasADisabledCategory()
		{
			CategoryName = "disabled";

			var moveCategory = new CategoryInfo
			{
				Name = "disabled"
			};

			Service.Admin.CreateCategory(moveCategory);
			Service.Admin.DisableCategory(moveCategory.Name);
		}


		[Given(@"it has an Account Out")]
		public void GivenItHasAnAccountOut()
		{
			AccountOut = GetOrCreateAccount(AccountOutUrl);

			AccountOutTotal = summaryRepository.GetTotal(AccountOut);

			var user = userRepository.GetByEmail(Current.Email);
			var category = categoryRepository.GetByName(CategoryName, user);

			YearCategoryAccountOutTotal =
				summaryRepository.Get(
					AccountOut, category, Date.Year
				)?.Out ?? 0;
			MonthCategoryAccountOutTotal =
				summaryRepository.Get(
					AccountOut, category, Date.ToMonthYear()
				)?.Out ?? 0;
		}

		[Given(@"it has no Account Out")]
		public void GivenItHasNoAccountOut()
		{
			AccountOut = null;
		}

		[Given(@"it has an unknown Account Out")]
		public void GivenItHasAnUnknownAccountOut()
		{
			var user = userRepository.GetByEmail(Current.Email);

			AccountOut = new Account
			{
				Name = "unknown",
				Url = "unknown",
				User = user
			};
		}

		[Given(@"it has a closed Account Out")]
		public void GivenItHasAClosedAccountOut()
		{
			var url = MakeUrlFromName("closed out");

			var account = new AccountInfo
			{
				Name = "closed out",
				Url = url,
			};

			Service.Admin.CreateAccount(account);
			var user = userRepository.GetByEmail(Current.Email);
			AccountOut = accountRepository.GetByUrl(url, user);

			var move = new MoveInfo
			{
				Date = Current.Now,
				Description = "Description",
				Nature = MoveNature.Out,
				Value = 10,
				OutUrl = AccountOut.Url,
				CategoryName = Category.Name,
			};

			Service.Money.SaveMove(move);

			Service.Admin.CloseAccount(AccountOut.Url);
		}


		[Given(@"it has an Account In")]
		public void GivenItHasAnAccountIn()
		{
			AccountIn = GetOrCreateAccount(AccountInUrl);

			AccountInTotal = summaryRepository.GetTotal(AccountIn);

			var user = userRepository.GetByEmail(Current.Email);
			var category = categoryRepository.GetByName(CategoryName, user);

			YearCategoryAccountInTotal =
				summaryRepository.Get(
					AccountIn, category, Date.Year
				)?.In ?? 0;
			MonthCategoryAccountInTotal =
				summaryRepository.Get(
					AccountIn, category, Date.ToMonthYear()
				)?.In ?? 0;
		}

		[Given(@"it has no Account In")]
		public void GivenItHasNoAccountIn()
		{
			AccountIn = null;
		}

		[Given(@"it has an unknown Account In")]
		public void GivenItHasAnUnknownAccountIn()
		{
			var user = userRepository.GetByEmail(Current.Email);

			AccountIn = new Account
			{
				Name = "unknown",
				Url = "unknown",
				User = user
			};
		}

		[Given(@"it has a closed Account In")]
		public void GivenItHasAClosedAccountIn()
		{
			var url = MakeUrlFromName("closed in");

			var account = new AccountInfo
			{
				Name = "closed in",
				Url = url,
			};
			 
			Service.Admin.CreateAccount(account);

			var user = userRepository.GetByEmail(Current.Email);
			AccountIn = accountRepository.GetByUrl(url, user);

			var move = new MoveInfo
			{
				Date = Current.Now,
				Description = "Description",
				Nature = MoveNature.In,
				Value = 10,
				InUrl = AccountIn.Url,
				CategoryName = Category.Name,
			};

			Service.Money.SaveMove(move);

			Service.Admin.CloseAccount(AccountIn.Url);
		}


		[Given(@"it has an Account In equal to Out")]
		public void GivenItHasAnAccountInEqualToOut()
		{
			AccountOut = GetOrCreateAccount(AccountOutUrl);
			AccountIn = AccountOut;

			AccountInTotal = summaryRepository.GetTotal(AccountIn);

			var user = userRepository.GetByEmail(Current.Email);
			var category = categoryRepository.GetByName(CategoryName, user);

			YearCategoryAccountInTotal =
				summaryRepository.Get(
					AccountIn, category, Date.Year
				)?.In ?? 0;
			MonthCategoryAccountInTotal =
				summaryRepository.Get(
					AccountIn, category, Date.ToMonthYear()
				)?.In ?? 0;

			YearCategoryAccountOutTotal =
				summaryRepository.Get(
					AccountOut, category, Date.Year
				)?.Out ?? 0;
			MonthCategoryAccountOutTotal =
				summaryRepository.Get(
					AccountOut, category, Date.ToMonthYear()
				)?.Out ?? 0;
		}



		[Then(@"the accountOut value will not change")]
		public void ThenTheAccountOutValueWillNotChange()
		{
			AccountOut = GetOrCreateAccount(AccountOut.Url);

			Assert.AreEqual(AccountOutTotal, summaryRepository.GetTotal(AccountOut));
		}

		[Then(@"the month-category-accountOut value will not change")]
		public void ThenTheMonthCategoryAccountOutValueWillNotChange()
		{
			AccountOut = GetOrCreateAccount(AccountOut.Name);

			var user = userRepository.GetByEmail(Current.Email);
			var category = categoryRepository.GetByName(CategoryName, user);

			var currentTotal = summaryRepository.Get(
				AccountOut, category, Date.ToMonthYear()
			)?.Out ?? 0;

			Assert.AreEqual(MonthCategoryAccountOutTotal, currentTotal);
		}

		[Then(@"the year-category-accountOut value will not change")]
		public void ThenTheYearCategoryAccountOutValueWillNotChange()
		{
			AccountOut = GetOrCreateAccount(AccountOut.Name);

			var user = userRepository.GetByEmail(Current.Email);
			var category = categoryRepository.GetByName(CategoryName, user);

			var currentTotal = summaryRepository.Get(
				AccountOut, category, Date.Year
			)?.Out ?? 0;

			Assert.AreEqual(YearCategoryAccountOutTotal, currentTotal);
		}

		[Then(@"the accountIn value will not change")]
		public void ThenTheAccountInValueWillNotChange()
		{
			AccountIn = GetOrCreateAccount(AccountIn.Name);

			Assert.AreEqual(AccountInTotal, summaryRepository.GetTotal(AccountIn));
		}

		[Then(@"the month-category-accountIn value will not change")]
		public void ThenTheMonthCategoryAccountInValueWillNotChange()
		{
			AccountIn = GetOrCreateAccount(AccountIn.Name);

			var user = userRepository.GetByEmail(Current.Email);
			var category = categoryRepository.GetByName(CategoryName, user);

			var currentTotal = summaryRepository.Get(
				AccountIn, category, Date.ToMonthYear()
			)?.In ?? 0;

			Assert.AreEqual(MonthCategoryAccountInTotal, currentTotal);
		}

		[Then(@"the year-category-accountIn value will not change")]
		public void ThenTheYearCategoryAccountInValueWillNotChange()
		{
			AccountIn = GetOrCreateAccount(AccountIn.Name);

			var user = userRepository.GetByEmail(Current.Email);
			var category = categoryRepository.GetByName(CategoryName, user);

			var currentTotal = summaryRepository.Get(
				AccountIn, category, Date.Year
			)?.In ?? 0;

			Assert.AreEqual(YearCategoryAccountInTotal, currentTotal);
		}


		[Then(@"the accountOut value will change in (\-?\d+\.?\d*)")]
		public void ThenTheAccountOutValueWillDecreaseIn(Decimal change)
		{
			AccountOut = GetOrCreateAccount(AccountOut.Name);

			var currentTotal = summaryRepository.GetTotal(AccountOut);

			Assert.AreEqual(AccountOutTotal + change, currentTotal);
		}

		[Then(@"the month-category-accountOut value will change in (\-?\d+\.?\d*)")]
		public void ThenTheMonthCategoryAccountOutValueWillChangeIn(Decimal change)
		{
			AccountOut = GetOrCreateAccount(AccountOut.Name);

			var user = userRepository.GetByEmail(Current.Email);
			var category = categoryRepository.GetByName(CategoryName, user);

			var currentTotal = summaryRepository.Get(
				AccountOut, category, Date.ToMonthYear()
			)?.Out ?? 0;

			Assert.AreEqual(MonthCategoryAccountOutTotal + change, currentTotal);
		}

		[Then(@"the year-category-accountOut value will change in (\-?\d+\.?\d*)")]
		public void ThenTheYearCategoryAccountOutValueWillChangeIn(Decimal change)
		{
			AccountOut = GetOrCreateAccount(AccountOut.Name);

			var user = userRepository.GetByEmail(Current.Email);
			var category = categoryRepository.GetByName(CategoryName, user);

			var currentTotal = summaryRepository.Get(
				AccountOut, category, Date.Year
			)?.Out ?? 0;

			Assert.AreEqual(YearCategoryAccountOutTotal + change, currentTotal);
		}


		[Then(@"the accountIn value will change in (\-?\d+\.?\d*)")]
		public void ThenTheAccountInValueWillIncreaseIn(Decimal change)
		{
			AccountIn = GetOrCreateAccount(AccountIn.Name);

			var currentTotal = summaryRepository.GetTotal(AccountIn);

			Assert.AreEqual(AccountInTotal + change, currentTotal);
		}

		[Then(@"the month-category-accountIn value will change in (\-?\d+\.?\d*)")]
		public void ThenTheMonthCategoryAccountInValueWillChangeIn(Decimal change)
		{
			AccountIn = GetOrCreateAccount(AccountIn.Name);

			var user = userRepository.GetByEmail(Current.Email);
			var category = categoryRepository.GetByName(CategoryName, user);

			var currentTotal = summaryRepository.Get(
				AccountIn, category, Date.ToMonthYear()
			)?.In ?? 0;

			Assert.AreEqual(MonthCategoryAccountInTotal + change, currentTotal);
		}

		[Then(@"the year-category-accountIn value will change in (\-?\d+\.?\d*)")]
		public void ThenTheYearCategoryAccountInValueWillChangeIn(Decimal change)
		{
			AccountIn = GetOrCreateAccount(AccountIn.Name);

			var user = userRepository.GetByEmail(Current.Email);
			var category = categoryRepository.GetByName(CategoryName, user);

			var currentTotal = summaryRepository.Get(
				AccountIn, category, Date.Year
			)?.In ?? 0;

			Assert.AreEqual(YearCategoryAccountInTotal + change, currentTotal);
		}
	}
}
