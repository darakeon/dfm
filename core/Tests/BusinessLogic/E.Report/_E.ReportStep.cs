﻿using System;
using System.Linq;
using DFM.BusinessLogic.Exceptions;
using DFM.BusinessLogic.Response;
using DFM.Entities.Bases;
using DFM.Entities.Enums;
using DFM.Generic;
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

		private static Int16? optionalMonth
		{
			get => get<Int16?>("OptionalMonth");
			set => set("OptionalMonth", value);
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

		private static CategoryReport categoryReport
		{
			get => get<CategoryReport>("CategoryReport");
			set => set("CategoryReport", value);
		}

		private static String tip
		{
			get => get<String>("Tip");
			set => set("Tip", value);
		}

		private static String otherTip
		{
			get => get<String>("OtherTip");
			set => set("OtherTip", value);
		}
		#endregion

		#region GetMonthReport
		[When(@"I try to get the month report")]
		public void WhenITryToGetTheMonthReport()
		{
			try
			{
				monthReport = service.Report
					.GetMonthReport(accountUrl, year, month);
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
			var user = repos.User.GetByEmail(current.Email);
			var account = repos.Account.GetByUrl(accountInfo.Url, user);

			var time = year * 100 + month;
			var expected = repos.Summary
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

		[Then(@"there will be these futures moves")]
		public void ThenThereWillBeTheseFuturesMoves(Table table)
		{
			var foreseenMoves = monthReport.ForeseenList;

			var count = table.RowCount;
			Assert.AreEqual(count, foreseenMoves.Count);

			for (var m = 0; m < count; m++)
			{
				var row = table.Rows[m];
				var description = row["Description"];
				var date = DateTime.Parse(row["Date"]);
				var nature = EnumX.Parse<MoveNature>(row["Nature"]);
				var value = Int32.Parse(row["Value"]);

				var line = foreseenMoves[m];
				Assert.AreEqual(description, line.Description);
				Assert.AreEqual(date, line.GetDate());
				Assert.AreEqual(nature, line.Nature);
				Assert.AreEqual(value, line.Value);
			}
		}

		[Then(@"the foreseen future value part will be ((?:\+|\-)\d+)")]
		public void ThenTheForeseenValueWillBe(Int32 value)
		{
			var foreseen = monthReport.ForeseenTotal - monthReport.AccountTotal;
			Assert.AreEqual(value, foreseen);
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
				var foreseenIn = Int32.Parse(row["Foreseen In"]);
				var foreseenOut = Int32.Parse(row["Foreseen Out"]);
				var foreseenTotal = Int32.Parse(row["Foreseen Total"]);

				var line = months[m];
				Assert.AreEqual(number, line.Number);
				Assert.AreEqual(currentIn, line.CurrentIn);
				Assert.AreEqual(currentOut, line.CurrentOut);
				Assert.AreEqual(currentTotal, line.CurrentTotal);
				Assert.AreEqual(foreseenIn, line.ForeseenIn);
				Assert.AreEqual(foreseenOut, line.ForeseenOut);
				Assert.AreEqual(foreseenTotal, line.ForeseenTotal);
			}
		}

		[Then(@"its sum value will be equal to its months sum value")]
		public void ThenItsSumValueWillBeEqualToItsMonthsSumValue()
		{
			var expected = yearReport.MonthList.Sum(m => m.CurrentTotal);

			var user = repos.User.GetByEmail(current.Email);
			var account = repos.Account.GetByUrl(accountInfo.Url, user);

			var actual = repos.Move
				.ByAccount(account)
				.Sum(
					m => m.In?.ID == account.ID
						? m.Value
						: -m.Value
				);

			Assert.AreEqual(expected, actual);
		}

		[Then(@"the year report sums will be")]
		public void ThenTheYearReportSumsWillBe(Table table)
		{
			var row = table.Rows[0];

			var currentIn = Int32.Parse(row["Current In"]);
			Assert.AreEqual(currentIn, yearReport.CurrentIn);

			var currentOut = Int32.Parse(row["Current Out"]);
			Assert.AreEqual(currentOut, yearReport.CurrentOut);

			var currentTotal = Int32.Parse(row["Current Total"]);
			Assert.AreEqual(currentTotal, yearReport.CurrentTotal);

			var foreseenIn = Int32.Parse(row["Foreseen In"]);
			Assert.AreEqual(foreseenIn, yearReport.ForeseenIn);

			var foreseenOut = Int32.Parse(row["Foreseen Out"]);
			Assert.AreEqual(foreseenOut, yearReport.ForeseenOut);

			var foreseenTotal = Int32.Parse(row["Foreseen Total"]);
			Assert.AreEqual(foreseenTotal, yearReport.ForeseenTotal);
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

		#region GetCategoryReport
		[When(@"I try to get the category report")]
		public void WhenITryToGetTheCategoryReport()
		{
			try
			{
				categoryReport = service.Report
					.GetCategoryReport(
						accountUrl, year, optionalMonth
					);
			}
			catch (CoreError e)
			{
				error = e;
			}
		}

		[Then(@"I will receive no category report")]
		public void ThenIWillReceiveNoCategoryReport()
		{
			Assert.IsNull(categoryReport);
		}

		[Then(@"I will receive empty category report")]
		public void ThenIWillReceiveTheCategoryReport()
		{
			Assert.IsNotNull(categoryReport);
			Assert.IsEmpty(categoryReport.List);
		}

		[Then(@"I will receive this category report")]
		public void ThenIWillReceiveThisCategoryReport(Table table)
		{
			Assert.IsNotNull(categoryReport);
			Assert.IsNotEmpty(categoryReport.List);

			Assert.AreEqual(table.Rows.Count, categoryReport.List.Count);

			for (var r = 0; r < table.Rows.Count; r++)
			{
				var row = table.Rows[r];
				var item = categoryReport.List[r];
				var category = row["Category"];

				Assert.AreEqual(category, item.Category);
				Assert.AreEqual(row["Out"], (item.OutCents / 100).ToString(), $"For {category}");
				Assert.AreEqual(row["In"], (item.InCents / 100).ToString(), $"For {category}");
			}
		}
		#endregion

		#region ShowTip
		[Given(@"asked for tip (\d+) times")]
		public void GivenAskedForTipTimes(Int32 times)
		{
			for(var i = 0; i < times; i++)
			{
				service.Report.ShowTip();
			}
		}

		[Given(@"dismissed tip")]
		public void GivenDismissedTip()
		{
			service.Report.DismissTip();
		}

		[Given(@"disabled tip (.+)")]
		public void GivenDisabledTip(TipTests tip)
		{
			service.Report.DisableTip(tip);
		}

		[When(@"show a tip")]
		public void WhenShowATip()
		{
			try
			{
				tip = service.Report.ShowTip();
			}
			catch (CoreError e)
			{
				error = e;
			}
		}

		[When(@"show a tip again")]
		public void WhenShowATipAgain()
		{
			try
			{
				otherTip = service.Report.ShowTip();
			}
			catch (CoreError e)
			{
				error = e;
			}
		}

		[When(@"disable tip (.+)")]
		public void WhenDisableTip(TipTests tip)
		{
			try
			{
				service.Report.DisableTip(tip);
			}
			catch (CoreError e)
			{
				error = e;
			}
		}

		[When(@"dismiss tip")]
		public void WhenDismissTip()
		{
			try
			{
				service.Report.DismissTip();
			}
			catch (CoreError e)
			{
				error = e;
			}
		}

		[Then(@"tip will (not )?be shown")]
		public void ThenTipWillBeShown(Boolean shown)
		{
			if (shown)
				Assert.NotNull(tip);
			else
				Assert.Null(tip);
		}

		[Then(@"tip shown will (not )?be equal to last one")]
		public void ThenTipShownWillBeEqualToLastOne(Boolean equal)
		{
			if (equal)
				Assert.AreEqual(tip, otherTip);
			else
				Assert.AreNotEqual(tip, otherTip);
		}

		[Then(@"the tips will not be the first ones")]
		public void ThenTheTipsWillNotBeTheFirstOnes()
		{
			Assert.False(
				TipTests.TestTip1.ToString() == tip
				&& TipTests.TestTip2.ToString() == otherTip
			);
		}

		[Then(@"the (.+) will be disabled")]
		public void ThenTheTipWillBeDisabled(TipTests tip)
		{
			var user = repos.User.GetByEmail(userEmail);
			var tips = repos.Tips.By(user, TipType.Tests);
			Assert.NotNull(tips);

			var isPermanent = tips.IsPermanent(tip);
			Assert.True(isPermanent);
		}

		[Then(@"the next tip will shown after (\d+) requests")]
		public void ThenTheNextTipWillShownAfterXRequests(Int32 counter)
		{
			var user = repos.User.GetByEmail(userEmail);
			var tips = repos.Tips.By(user, TipType.Tests);
			Assert.NotNull(tips);

			Assert.AreEqual(counter - 1, tips.Countdown);
		}

		[Then(@"tip will be one of")]
		public void ThenTipWillBeOneOf(Table table)
		{
			var validTips = table.Rows
				.Select(r => r["Tip"])
				.ToList();

			Assert.Contains(tip, validTips);
		}
		#endregion ShowTip

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

				var category =
					row.ContainsKey("Category")
						? row["Category"]
						: categoryInfo.Name;

				var nature =
					row.ContainsKey("Nature") && row["Nature"] == "In"
						? MoveNature.In
						: MoveNature.Out;

				var accountOut =
					nature == MoveNature.Out
						? accountInfo.Url
						: null;

				var accountIn =
					nature == MoveNature.In
						? accountInfo.Url
						: null;

				var move = new MoveInfo
				{
					Description = description,
					Nature = nature,
					OutUrl = accountOut,
					InUrl = accountIn,
					CategoryName = category,
				};

				var value = row.ContainsKey("Value")
						&& row["Value"] != String.Empty
					? Int32.Parse(row["Value"])
					: 10;

				if (row.ContainsKey("Detail") && row["Detail"] != "")
				{
					var detail = new DetailInfo
					{
						Description = row["Detail"],
						Amount = 1,
						Value = value,
					};
					move.DetailList.Add(detail);
				}
				else
				{
					move.Value = value;
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

				optionalMonth = month;
			}

			var yearString = dateData["Year"];
			year = Int16.Parse(yearString);

			if (isRelative(yearString))
			{
				year = (Int16) current.Now.AddYears(year).Year;
			}
		}

		private Boolean isRelative(String dateText)
		{
			return dateText.StartsWith("+") || dateText.StartsWith("-");
		}
		#endregion
	}
}
