using System;
using System.Linq;
using DFM.BusinessLogic.Exceptions;
using DFM.BusinessLogic.Response;
using DFM.Entities.Enums;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace DFM.Tests.BusinessLogic.E.Report
{
	[Binding]
	public class ReportStep : BaseStep
	{
		#region Variables

		private static Int16 month
		{
			get => Get<Int16>("Month");
			set => Set("Month", value);
		}

		private static Int16 year
		{
			get => Get<Int16>("Year");
			set => Set("Year", value);
		}

		private static MonthReport monthReport
		{
			get => Get<MonthReport>("MonthReport");
			set => Set("MonthReport", value);
		}

		private static YearReport yearReport
		{
			get => Get<YearReport>("YearReport");
			set => Set("YearReport", value);
		}
		#endregion

		#region GetMonthReport
		[When(@"I try to get the month report")]
		public void WhenITryToGetTheMonthReport()
		{
			try
			{
				monthReport = Service.Report.GetMonthReport(AccountUrl, month, year);
			}
			catch (CoreError e)
			{
				Error = e;
			}
		}

		[Then(@"I will receive no month report")]
		public void ThenIWillReceiveNoMonthReport()
		{
			Assert.IsNull(monthReport);
		}

		[Then(@"I will receive the month report")]
		public void ThenIWillReceiveTheMonthReport()
		{
			Assert.IsNotNull(monthReport);
		}

		[Then(@"its sum value will be equal to its moves sum value")]
		public void ThenItsSumValueWillBeEqualToItsMovesSumValue()
		{
			var user = userRepository.GetByEmail(Current.Email);
			var account = accountRepository.GetByUrl(Account.Url, user);

			var time = year * 100 + month;
			var expected = summaryRepository
				.Get(account, time)
				.Sum(s => s.Value());

			var actual = monthReport.MoveList.Sum(m =>
					m.OutUrl == account.Url
						? - m.Total
						: m.Total
				);

			Assert.AreEqual(expected, actual);
		}

		[Then(@"there will be no moves")]
		public void ThenThereWillBeNoMoves()
		{
			Assert.IsEmpty(monthReport.MoveList);
		}
		#endregion

		#region GetYearReport
		[When(@"I try to get the year report")]
		public void WhenITryToGetTheYearReport()
		{
			try
			{
				yearReport = Service.Report.GetYearReport(AccountUrl, year);
			}
			catch (CoreError e)
			{
				Error = e;
			}
		}

		[Then(@"I will receive no year report")]
		public void ThenIWillReceiveNoYearReport()
		{
			Assert.IsNull(yearReport);
		}

		[Then(@"I will receive the year report")]
		public void ThenIWillReceiveTheYearReport()
		{
			Assert.IsNotNull(yearReport);
		}

		[Then(@"its sum value will be equal to its months sum value")]
		public void ThenItsSumValueWillBeEqualToItsMonthsSumValue()
		{
			var user = userRepository.GetByEmail(Current.Email);
			var account = accountRepository.GetByUrl(Account.Url, user);

			var expected = summaryRepository.GetTotal(account);

			var actual = moveRepository
				.ByAccount(account)
				.Sum(
					m => m.ID == account.ID
						? m.Total()
						: -m.Total()
				);

			Assert.AreEqual(expected, actual);
		}
		#endregion



		#region MoreThanOne
		[Given(@"I have moves of")]
		public void GivenIHaveMovesOf(Table table)
		{
			Category = CategoryInfo.Convert(
				GetOrCreateCategory(MAIN_CATEGORY_NAME)
			);

			foreach (var row in table.Rows)
			{
				var dateString = row["Date"];
				var date = isRelative(dateString)
					? DateTime.Today.AddDays(Int32.Parse(dateString))
					: DateTime.Parse(dateString);

				var move = new MoveInfo
				{
					Description = "Description",
					Date = date,
					Nature = MoveNature.Out,
					Value = 10,
					OutUrl = Account.Url,
					CategoryName = Category.Name,
				};

				Service.Money.SaveMove(move);
			}
		}

		[Given(@"I pass an invalid account url")]
		public void GivenIPassAnInvalidAccountName()
		{
			AccountUrl = "invalid";
		}

		[Given(@"I pass this date")]
		public void GivenIPassThisDate(Table table)
		{
			var dateData = table.Rows[0];

			if (table.Header.Contains("Month"))
			{
				var monthString = dateData["Month"];
				month = Int16.Parse(monthString);

				if (isRelative(monthString))
				{
					month = (Int16) DateTime.Today.AddMonths(month).Month;
				}
			}

			var yearString = dateData["Year"];
			year = Int16.Parse(yearString);

			if (isRelative(yearString))
			{
				year = (Int16) DateTime.Today.AddYears(year).Year;
			}
		}

		#endregion

		private Boolean isRelative(String date)
		{
			return date.StartsWith("+") || date.StartsWith("-");
		}

	}
}
