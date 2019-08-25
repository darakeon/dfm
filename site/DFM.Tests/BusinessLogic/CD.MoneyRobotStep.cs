using System;
using System.Collections.Generic;
using DFM.Entities;
using DFM.Entities.Enums;
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
			if (Move != null)
				Move.DetailList = new List<Detail>();

			if (Schedule != null)
				Schedule.DetailList = new List<Detail>();
		}

		[Given(@"the move has this details")]
		public void GivenTheMoveHasThisDetails(Table table)
		{
			foreach (var detailData in table.Rows)
			{
				var detail = GetDetailFromTable(detailData);

				DetailList.Add(detail);
			}
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

			var moveCategory = new Category { Name = "disabled", User = User };

			Service.Admin.CreateCategory(moveCategory);
			Service.Admin.DisableCategory(moveCategory.Name);
		}


		[Given(@"it has an Account Out")]
		public void GivenItHasAnAccountOut()
		{
			AccountOut = GetOrCreateAccount(AccountOutUrl);

			var year = AccountOut[Date.Year, true];
			var month = year[Date.Month, true];

			AccountOutTotal = AccountOut.Total();
			YearCategoryAccountOutTotal = (year[CategoryName] ?? new Summary()).Out;
			MonthCategoryAccountOutTotal = (month[CategoryName] ?? new Summary()).Out;
		}

		[Given(@"it has no Account Out")]
		public void GivenItHasNoAccountOut()
		{
			AccountOut = null;
		}

		[Given(@"it has an unknown Account Out")]
		public void GivenItHasAnUnknownAccountOut()
		{
			AccountOut = new Account
			{
				Name = "unknown",
				Url = "unknown",
				User = User
			};
		}

		[Given(@"it has a closed Account Out")]
		public void GivenItHasAClosedAccountOut()
		{
			AccountOut = new Account
			{
				Name = "closed out",
				Url = MakeUrlFromName("closed out"),
			};

			Service.Admin.CreateAccount(AccountOut);

			var move = new Move
			{
				Date = Current.User.Now(),
				Description = "Description",
				Nature = MoveNature.Out,
				Value = 10
			};

			Service.Money.SaveOrUpdateMove(move, AccountOut.Url, null, Category.Name);

			Service.Admin.CloseAccount(AccountOut.Url);
		}


		[Given(@"it has an Account In")]
		public void GivenItHasAnAccountIn()
		{
			AccountIn = GetOrCreateAccount(AccountInUrl);

			var year = AccountIn[Date.Year, true];
			var month = year[Date.Month, true];

			AccountInTotal = AccountIn.Total();
			YearCategoryAccountInTotal = (year[CategoryName] ?? new Summary()).In;
			MonthCategoryAccountInTotal = (month[CategoryName] ?? new Summary()).In;
		}

		[Given(@"it has no Account In")]
		public void GivenItHasNoAccountIn()
		{
			AccountIn = null;
		}

		[Given(@"it has an unknown Account In")]
		public void GivenItHasAnUnknownAccountIn()
		{
			AccountIn = new Account
			{
				Name = "unknown",
				Url = "unknown",
				User = User
			};
		}

		[Given(@"it has a closed Account In")]
		public void GivenItHasAClosedAccountIn()
		{
			AccountIn = new Account
			{
				Name = "closed in",
				Url = MakeUrlFromName("closed in"),
			};

			Service.Admin.CreateAccount(AccountIn);

			var move = new Move
			{
				Date = Current.User.Now(),
				Description = "Description",
				Nature = MoveNature.In,
				Value = 10
			};

			Service.Money.SaveOrUpdateMove(move, null, AccountIn.Url, Category.Name);

			Service.Admin.CloseAccount(AccountIn.Url);
		}


		[Given(@"it has an Account In equal to Out")]
		public void GivenItHasAnAccountInEqualToOut()
		{
			AccountOut = GetOrCreateAccount(AccountOutUrl);
			AccountIn = AccountOut;

			var year = AccountIn[Date.Year, true];
			var month = year[Date.Month, true];

			AccountInTotal = AccountIn.Total();
			YearCategoryAccountInTotal = (year[CategoryName] ?? new Summary()).In;
			MonthCategoryAccountInTotal = (month[CategoryName] ?? new Summary()).In;
		}



		[Then(@"the accountOut value will not change")]
		public void ThenTheAccountOutValueWillNotChange()
		{
			AccountOut = GetOrCreateAccount(AccountOut.Url);

			Assert.AreEqual(AccountOutTotal, AccountOut.Total());
		}

		[Then(@"the month-category-accountOut value will not change")]
		public void ThenTheMonthCategoryAccountOutValueWillNotChange()
		{
			AccountOut = GetOrCreateAccount(AccountOut.Name);

			var year = AccountOut[Date.Year, true];
			var month = year[Date.Month, true];

			var currentTotal = (month[CategoryName] ?? new Summary()).Out;

			Assert.AreEqual(MonthCategoryAccountOutTotal, currentTotal);
		}

		[Then(@"the year-category-accountOut value will not change")]
		public void ThenTheYearCategoryAccountOutValueWillNotChange()
		{
			AccountOut = GetOrCreateAccount(AccountOut.Name);

			var year = AccountOut[Date.Year, true];

			var currentTotal = (year[CategoryName] ?? new Summary()).Out;

			Assert.AreEqual(YearCategoryAccountOutTotal, currentTotal);
		}


		[Then(@"the accountIn value will not change")]
		public void ThenTheAccountInValueWillNotChange()
		{
			AccountIn = GetOrCreateAccount(AccountIn.Name);

			Assert.AreEqual(AccountInTotal, AccountIn.Total());
		}

		[Then(@"the month-category-accountIn value will not change")]
		public void ThenTheMonthCategoryAccountInValueWillNotChange()
		{
			AccountIn = GetOrCreateAccount(AccountIn.Name);

			var year = AccountIn[Date.Year, true];
			var month = year[Date.Month, true];

			var currentTotal = (month[CategoryName] ?? new Summary()).In;

			Assert.AreEqual(MonthCategoryAccountInTotal, currentTotal);
		}

		[Then(@"the year-category-accountIn value will not change")]
		public void ThenTheYearCategoryAccountInValueWillNotChange()
		{
			AccountIn = GetOrCreateAccount(AccountIn.Name);

			var year = AccountIn[Date.Year, true];

			var currentTotal = (year[CategoryName] ?? new Summary()).In;

			Assert.AreEqual(YearCategoryAccountInTotal, currentTotal);
		}


		[Then(@"the accountOut value will change in (\-?\d+\.?\d*)")]
		public void ThenTheAccountOutValueWillDecreaseIn(Decimal change)
		{
			AccountOut = GetOrCreateAccount(AccountOut.Name);

			var currentTotal = AccountOut.Total();

			Assert.AreEqual(AccountOutTotal + change, currentTotal);
		}

		[Then(@"the month-category-accountOut value will change in (\-?\d+\.?\d*)")]
		public void ThenTheMonthCategoryAccountOutValueWillChangeIn(Decimal change)
		{
			AccountOut = GetOrCreateAccount(AccountOut.Name);

			var year = AccountOut[Date.Year, true];
			var month = year[Date.Month, true];

			var currentTotal = month[CategoryName].Out;

			Assert.AreEqual(MonthCategoryAccountOutTotal + change, currentTotal);
		}

		[Then(@"the year-category-accountOut value will change in (\-?\d+\.?\d*)")]
		public void ThenTheYearCategoryAccountOutValueWillChangeIn(Decimal change)
		{
			AccountOut = GetOrCreateAccount(AccountOut.Name);

			var year = AccountOut[Date.Year, true];

			var currentTotal = year[CategoryName].Out;

			Assert.AreEqual(YearCategoryAccountOutTotal + change, currentTotal);
		}


		[Then(@"the accountIn value will change in (\-?\d+\.?\d*)")]
		public void ThenTheAccountInValueWillIncreaseIn(Decimal change)
		{
			AccountIn = GetOrCreateAccount(AccountIn.Name);

			var currentTotal = AccountIn.Total();

			Assert.AreEqual(AccountInTotal + change, currentTotal);
		}

		[Then(@"the month-category-accountIn value will change in (\-?\d+\.?\d*)")]
		public void ThenTheMonthCategoryAccountInValueWillChangeIn(Decimal change)
		{
			AccountIn = GetOrCreateAccount(AccountIn.Name);

			var year = AccountIn[Date.Year, true];
			var month = year[Date.Month, true];

			var currentTotal = month[CategoryName].In;

			Assert.AreEqual(MonthCategoryAccountInTotal + change, currentTotal);
		}

		[Then(@"the year-category-accountIn value will change in (\-?\d+\.?\d*)")]
		public void ThenTheYearCategoryAccountInValueWillChangeIn(Decimal change)
		{
			AccountIn = GetOrCreateAccount(AccountIn.Name);

			var year = AccountIn[Date.Year, true];

			var currentTotal = year[CategoryName].In;

			Assert.AreEqual(YearCategoryAccountInTotal + change, currentTotal);
		}



	}
}
