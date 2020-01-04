using System;
using System.Linq;
using DFM.BusinessLogic.Exceptions;
using DFM.BusinessLogic.Response;
using DFM.Entities.Bases;
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
			get => get<Int16>("Month");
			set => set("Month", value);
		}

		private static Int16 year
		{
			get => get<Int16>("Year");
			set => set("Year", value);
		}

		private static MonthReport monthReport
		{
			get => get<MonthReport>("MonthReport");
			set => set("MonthReport", value);
		}

		private static YearReport yearReport
		{
			get => get<YearReport>("YearReport");
			set => set("YearReport", value);
		}
		#endregion

		#region GetMonthReport
		[When(@"I try to get the month report")]
		public void WhenITryToGetTheMonthReport()
		{
			try
			{
				monthReport = service.Report.GetMonthReport(accountUrl, month, year);
			}
			catch (CoreError e)
			{
				error = e;
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
			var user = userRepository.GetByEmail(current.Email);
			var account = accountRepository.GetByUrl(accountInfo.Url, user);

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

		[Then(@"there will have a move with description (.*)")]
		public void ThenThereWillHaveAMoveWithDescriptionScheduleEa(String description)
		{
			Assert.IsTrue(
				monthReport.MoveList.Any(m => m.Description == description)
			);
		}
		#endregion

		#region GetYearReport
		[When(@"I try to get the year report")]
		public void WhenITryToGetTheYearReport()
		{
			try
			{
				yearReport = service.Report.GetYearReport(accountUrl, year);
			}
			catch (CoreError e)
			{
				error = e;
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
			var user = userRepository.GetByEmail(current.Email);
			var account = accountRepository.GetByUrl(accountInfo.Url, user);

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
			categoryInfo = CategoryInfo.Convert(
				getOrCreateCategory(mainCategoryName)
			);

			foreach (var row in table.Rows)
			{
				var move = new MoveInfo
				{
					Description = "Description",
					Nature = MoveNature.Out,
					Value = 10,
					OutUrl = accountInfo.Url,
					CategoryName = categoryInfo.Name,
				};

				var dateString = row["Date"];
				var moveDate = isRelative(dateString)
					? DateTime.Today.AddDays(Int32.Parse(dateString))
					: DateTime.Parse(dateString);

				move.SetDate(moveDate);

				service.Money.SaveMove(move);
			}
		}

		[Given(@"I pass an invalid account url")]
		public void GivenIPassAnInvalidAccountName()
		{
			accountUrl = "invalid";
		}

		[Given(@"I pass Account Out url")]
		public void GivenIPassAccountOutUrl()
		{
			accountUrl = accountOutUrl;
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

		private Boolean isRelative(String dateText)
		{
			return dateText.StartsWith("+") || dateText.StartsWith("-");
		}
	}
}
