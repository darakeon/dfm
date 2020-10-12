using System;
using System.Linq;
using DFM.BusinessLogic.Exceptions;
using DFM.BusinessLogic.Response;
using DFM.Entities.Bases;
using DFM.Entities.Enums;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace DFM.BusinessLogic.Tests.E.Report
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

		private SearchResult searchResult
		{
			get => get<SearchResult>("SearchResult");
			set => set("SearchResult", value);
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
						? - m.Value
						: m.Value
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
		public void ThenIWillReceiveTheYearReport(Table table)
		{
			Assert.IsNotNull(yearReport);

			var months = yearReport.MonthList;

			var count = table.RowCount;
			Assert.AreEqual(count, months.Count);

			for (var m = 0; m < count; m++)
			{
				var row = table.Rows[m];
				var number = Int32.Parse(row["Number"]);
				var currentIn = Int32.Parse(row["Current In"]);
				var currentOut = Int32.Parse(row["Current Out"]);
				var currentTotal = Int32.Parse(row["Current Total"]);
				var line = months[m];
				Assert.AreEqual(number, line.Number);
				Assert.AreEqual(currentIn, line.CurrentIn);
				Assert.AreEqual(currentOut, line.CurrentOut);
				Assert.AreEqual(currentTotal, line.CurrentTotal);
			}
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
					m => m.In?.ID == account.ID
						? m.Value
						: -m.Value
				);

			Assert.AreEqual(expected, actual);
		}
		#endregion

		#region SearchByDescription
		[When(@"I try to search by description (.+)")]
		public void WhenITryToSearchByDescription(String description)
		{
			// ReSharper disable once SwitchStatementMissingSomeCases
			switch (description)
			{
				case "{empty}": description = ""; break;
				case "{null}": description = null; break;
			}

			try
			{
				searchResult = service.Report.SearchByDescription(description);
			}
			catch (CoreError e)
			{
				error = e;
			}
		}

		[Then(@"I will receive no moves")]
		public void ThenIWillReceiveNoMoves()
		{
			Assert.AreEqual(0, searchResult.MoveList.Count);
		}

		[Then(@"I will receive these moves")]
		public void ThenIWillReceiveTheseMoves(Table table)
		{
			Assert.AreEqual(table.RowCount, searchResult.MoveList.Count);

			foreach (var row in table.Rows)
			{
				var description = row["Description"];

				var dateString = row["Date"];
				var moveDate = isRelative(dateString)
					? current.Now.AddDays(Int32.Parse(dateString))
					: DateTime.Parse(dateString);
				var detail = row["Detail"];

				var move = searchResult.MoveList.FirstOrDefault(
					m => m.Description == description
						&& m.GetDate() == moveDate
						&& (
							 detail == "" ||
							 m.DetailList.FirstOrDefault()?.Description == detail
						)
				);

				Assert.IsNotNull(move, $"NOT FOUND: [{moveDate}] {description} ({detail})");

				searchResult.MoveList.Remove(move);
			}
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
				var description =
					row.ContainsKey("Description")
						? row["Description"]
						: "Description";

				var move = new MoveInfo
				{
					Description = description,
					Nature = MoveNature.Out,
					OutUrl = accountInfo.Url,
					CategoryName = categoryInfo.Name,
				};

				if (row.ContainsKey("Detail") && row["Detail"] != "")
				{
					var detail = new DetailInfo
					{
						Description = row["Detail"],
						Amount = 1,
						Value = 10,
					};
					move.DetailList.Add(detail);
				}
				else
				{
					move.Value = 10;
				}

				var dateString = row["Date"];
				var moveDate = isRelative(dateString)
					? current.Now.AddDays(Int32.Parse(dateString))
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
					month = (Int16) current.Now.AddMonths(month).Month;
				}
			}

			var yearString = dateData["Year"];
			year = Int16.Parse(yearString);

			if (isRelative(yearString))
			{
				year = (Int16) current.Now.AddYears(year).Year;
			}
		}

		#endregion

		private Boolean isRelative(String dateText)
		{
			return dateText.StartsWith("+") || dateText.StartsWith("-");
		}
	}
}
