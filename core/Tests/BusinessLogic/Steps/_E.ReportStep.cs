using System;
using System.Linq;
using DFM.BusinessLogic.Exceptions;
using DFM.BusinessLogic.Response;
using DFM.BusinessLogic.Tests.Helpers;
using DFM.Entities.Bases;
using DFM.Entities.Enums;
using DFM.Generic;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace DFM.BusinessLogic.Tests.Steps
{
	[Binding]
	public class ReportStep : BaseStep
	{
		public ReportStep(ScenarioContext context)
			: base(context) { }

		#region Variables
		private Int16 month
		{
			get => get<Int16>("Month");
			set => set("Month", value);
		}

		private Int16? optionalMonth
		{
			get => get<Int16?>("OptionalMonth");
			set => set("OptionalMonth", value);
		}

		private Int16 year
		{
			get => get<Int16>("Year");
			set => set("Year", value);
		}

		private MonthReport monthReport
		{
			get => get<MonthReport>("MonthReport");
			set => set("MonthReport", value);
		}

		private YearReport yearReport
		{
			get => get<YearReport>("YearReport");
			set => set("YearReport", value);
		}

		private SearchResult searchResult
		{
			get => get<SearchResult>("SearchResult");
			set => set("SearchResult", value);
		}

		private CategoryReport categoryReport
		{
			get => get<CategoryReport>("CategoryReport");
			set => set("CategoryReport", value);
		}

		private String tip
		{
			get => get<String>("Tip");
			set => set("Tip", value);
		}

		private String otherTip
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
			Assert.That(monthReport, Is.Null);
		}

		[Then(@"I will receive the month report")]
		public void ThenIWillReceiveTheMonthReport()
		{
			Assert.That(monthReport, Is.Not.Null);
		}

		[Then(@"its sum value will be equal to its moves sum value")]
		public void ThenItsSumValueWillBeEqualToItsMovesSumValue()
		{
			var user = repos.User.GetByEmail(current.Email);
			var account = repos.Account.GetByUrl(accountInfo.Name.IntoUrl(), user);

			var time = year * 100 + month;
			var expected = repos.Summary
				.Get(account, time)
				.Sum(s => s.Value());

			var actual = monthReport.MoveList.Sum(m =>
					m.OutUrl == account.Url
						? -m.Value
						: m.Value
				);

			Assert.That(actual, Is.EqualTo(expected));
		}

		[Then(@"there will be no moves")]
		public void ThenThereWillBeNoMoves()
		{
			Assert.That(monthReport.MoveList, Is.Empty);
		}

		[Then(@"there will have a move with description (.*)")]
		public void ThenThereWillHaveAMoveWithDescriptionScheduleEa(String description)
		{
			Assert.That(
				monthReport.MoveList.Any(m => m.Description == description),
				Is.True
			);
		}

		[Then(@"there will be these futures moves")]
		public void ThenThereWillBeTheseFuturesMoves(Table table)
		{
			var foreseenMoves = monthReport.ForeseenList;

			var count = table.RowCount;
			Assert.That(foreseenMoves.Count, Is.EqualTo(count));

			for (var m = 0; m < count; m++)
			{
				var row = table.Rows[m];
				var description = row["Description"].ForScenario(scenarioCode);
				var date = DateTime.Parse(row["Date"]);
				var nature = EnumX.Parse<MoveNature>(row["Nature"]);
				var value = Int32.Parse(row["Value"]);

				var line = foreseenMoves[m];
				Assert.That(line.Description, Is.EqualTo(description));
				Assert.That(line.GetDate(), Is.EqualTo(date));
				Assert.That(line.Nature, Is.EqualTo(nature));
				Assert.That(line.Value, Is.EqualTo(value));
			}
		}

		[Then(@"the foreseen future value part will be ((?:\+|\-)\d+)")]
		public void ThenTheForeseenValueWillBe(Int32 value)
		{
			var foreseen = monthReport.ForeseenTotal - monthReport.AccountTotal;
			Assert.That(foreseen, Is.EqualTo(value));
		}

		[Then(@"there will be these moves")]
		public void ThenThereWillBeTheseMoves(Table table)
		{
			var moveList = monthReport.MoveList;

			var count = table.RowCount;
			Assert.That(moveList.Count, Is.EqualTo(count));

			for (var m = 0; m < count; m++)
			{
				var row = table.Rows[m];
				var line = moveList[m];

				if (row.ContainsKey("Description"))
				{
					var description = row["Description"];
					Assert.That(line.Description, Is.EqualTo(description));
				}

				if (row.ContainsKey("Date"))
				{
					var dateString = row["Date"];
					var date = isRelative(dateString)
						? current.Now.Date.AddDays(Int32.Parse(dateString))
						: DateTime.Parse(dateString);

					Assert.That(line.GetDate(), Is.EqualTo(date));
				}

				if (row.ContainsKey("Nature"))
				{
					var nature = EnumX.Parse<MoveNature>(row["Nature"]);
					Assert.That(line.Nature, Is.EqualTo(nature));
				}

				if (row.ContainsKey("Value"))
				{
					var value = Int32.Parse(row["Value"]);
					Assert.That(line.Value, Is.EqualTo(value));
				}
			}
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
			Assert.That(yearReport, Is.Null);
		}

		[Then(@"I will receive the year report")]
		public void ThenIWillReceiveTheYearReport(Table table)
		{
			Assert.That(yearReport, Is.Not.Null);

			var months = yearReport.MonthList;

			var count = table.RowCount;
			Assert.That(months.Count, Is.EqualTo(count));

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
				Assert.That(line.Number, Is.EqualTo(number));
				Assert.That(line.CurrentIn, Is.EqualTo(currentIn));
				Assert.That(line.CurrentOut, Is.EqualTo(currentOut));
				Assert.That(line.CurrentTotal, Is.EqualTo(currentTotal));
				Assert.That(line.ForeseenIn, Is.EqualTo(foreseenIn));
				Assert.That(line.ForeseenOut, Is.EqualTo(foreseenOut));
				Assert.That(line.ForeseenTotal, Is.EqualTo(foreseenTotal));
			}
		}

		[Then(@"its sum value will be equal to its months sum value")]
		public void ThenItsSumValueWillBeEqualToItsMonthsSumValue()
		{
			var expected = yearReport.MonthList.Sum(m => m.CurrentTotal);

			var user = repos.User.GetByEmail(current.Email);
			var account = repos.Account.GetByUrl(accountInfo.Name.IntoUrl(), user);

			var actual = repos.Move
				.ByAccount(account)
				.Sum(
					m => m.In?.ID == account.ID
						? m.Conversion != null && m.Conversion != 0
							? m.Conversion.Value
							: m.Value
						: -m.Value
				);

			Assert.That(actual, Is.EqualTo(expected));
		}

		[Then(@"the year report sums will be")]
		public void ThenTheYearReportSumsWillBe(Table table)
		{
			var row = table.Rows[0];

			var currentIn = Int32.Parse(row["Current In"]);
			Assert.That(yearReport.CurrentIn, Is.EqualTo(currentIn));

			var currentOut = Int32.Parse(row["Current Out"]);
			Assert.That(yearReport.CurrentOut, Is.EqualTo(currentOut));

			var currentTotal = Int32.Parse(row["Current Total"]);
			Assert.That(yearReport.CurrentTotal, Is.EqualTo(currentTotal));

			var foreseenIn = Int32.Parse(row["Foreseen In"]);
			Assert.That(yearReport.ForeseenIn, Is.EqualTo(foreseenIn));

			var foreseenOut = Int32.Parse(row["Foreseen Out"]);
			Assert.That(yearReport.ForeseenOut, Is.EqualTo(foreseenOut));

			var foreseenTotal = Int32.Parse(row["Foreseen Total"]);
			Assert.That(yearReport.ForeseenTotal, Is.EqualTo(foreseenTotal));
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
			Assert.That(searchResult.MoveList.Count, Is.EqualTo(0));
		}

		[Then(@"I will receive these moves")]
		public void ThenIWillReceiveTheseMoves(Table table)
		{
			Assert.That(searchResult.MoveList.Count, Is.EqualTo(table.RowCount));

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

				Assert.That(
					move,
					Is.Not.Null,
					$"NOT FOUND: [{moveDate}] {description} ({detail})"
				);

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
			Assert.That(categoryReport, Is.Null);
		}

		[Then(@"I will receive empty category report")]
		public void ThenIWillReceiveTheCategoryReport()
		{
			Assert.That(categoryReport, Is.Not.Null);
			Assert.That(categoryReport.List, Is.Empty);
		}

		[Then(@"I will receive this category report")]
		public void ThenIWillReceiveThisCategoryReport(Table table)
		{
			Assert.That(categoryReport, Is.Not.Null);
			Assert.That(categoryReport.List, Is.Not.Empty);

			Assert.That(categoryReport.List.Count, Is.EqualTo(table.Rows.Count));

			for (var r = 0; r < table.Rows.Count; r++)
			{
				var row = table.Rows[r];
				var item = categoryReport.List[r];
				var category = row["Category"];

				Assert.That(item.Category, Is.EqualTo(category));
				Assert.That((item.OutCents / 100).ToString(), Is.EqualTo(row["Out"]), $"For {category}");
				Assert.That((item.InCents / 100).ToString(), Is.EqualTo(row["In"]), $"For {category}");
			}
		}
		#endregion

		#region ShowTip
		[Given(@"asked for tip (\d+) times")]
		public void GivenAskedForTipTimes(Int32 times)
		{
			for (var i = 0; i < times; i++)
			{
				service.Clip.ShowTip();
			}
		}

		[Given(@"dismissed tip")]
		public void GivenDismissedTip()
		{
			service.Clip.DismissTip();
		}

		[Given(@"disabled tip (.+)")]
		public void GivenDisabledTip(TipTests tip)
		{
			service.Clip.DisableTip(tip);
		}

		[When(@"show a tip")]
		public void WhenShowATip()
		{
			try
			{
				tip = service.Clip.ShowTip();
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
				otherTip = service.Clip.ShowTip();
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
				service.Clip.DisableTip(tip);
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
				service.Clip.DismissTip();
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
				Assert.That(tip, Is.Not.Null);
			else
				Assert.That(tip, Is.Null);
		}

		[Then(@"tip shown will (not )?be equal to last one")]
		public void ThenTipShownWillBeEqualToLastOne(Boolean equal)
		{
			if (equal)
				Assert.That(otherTip, Is.EqualTo(tip));
			else
				Assert.That(otherTip, Is.Not.EqualTo(tip));
		}

		[Then(@"the tips will not be the first ones")]
		public void ThenTheTipsWillNotBeTheFirstOnes()
		{
			Assert.That(
				TipTests.TestTip1.ToString() == tip
				&& TipTests.TestTip2.ToString() == otherTip,
				Is.False
			);
		}

		[Then(@"the (.+) will be disabled")]
		public void ThenTheTipWillBeDisabled(TipTests tip)
		{
			var user = repos.User.GetByEmail(userEmail);
			var tips = repos.Tips.By(user, TipType.Tests);
			Assert.That(tips, Is.Not.Null);

			var isPermanent = tips.IsPermanent(tip);
			Assert.That(isPermanent, Is.True);
		}

		[Then(@"the next tip will shown after (\d+) requests")]
		public void ThenTheNextTipWillShownAfterXRequests(Int32 counter)
		{
			var user = repos.User.GetByEmail(userEmail);
			var tips = repos.Tips.By(user, TipType.Tests);
			Assert.That(tips, Is.Not.Null);

			Assert.That(tips.Countdown, Is.EqualTo(counter - 1));
		}

		[Then(@"tip will be one of")]
		public void ThenTipWillBeOneOf(Table table)
		{
			var validTips = table.Rows
				.Select(r => r["Tip"])
				.ToList();

			Assert.That(validTips.Contains(tip), Is.True);
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
					row.ContainsKey("Nature") && row["Nature"] != ""
						? EnumX.Parse<MoveNature>(row["Nature"])
						: MoveNature.Out;

				var moveAccountOut =
					nature switch
					{
						MoveNature.Out => accountInfo.Name.IntoUrl(),
						MoveNature.Transfer => accountOutUrl,
						_ => null
					};

				var moveAccountIn =
					nature switch
					{
						MoveNature.In => accountInfo.Name.IntoUrl(),
						MoveNature.Transfer => accountInUrl,
						_ => null
					};

				var move = new MoveInfo
				{
					Description = description,
					Nature = nature,
					OutUrl = moveAccountOut,
					InUrl = moveAccountIn,
					CategoryName = category,
				};

				var value = row.ContainsKey("Value")
				        && row["Value"] != String.Empty
					? Int32.Parse(row["Value"])
					: 10;

				var conversion = row.ContainsKey("Conversion")
				        && row["Conversion"] != String.Empty
					? Int32.Parse(row["Conversion"])
					: default(Int32?);

				if (row.ContainsKey("Detail") && row["Detail"] != "")
				{
					var detail = new DetailInfo
					{
						Description = row["Detail"],
						Amount = 1,
						Value = value,
						Conversion = conversion,
					};
					move.DetailList.Add(detail);
				}
				else
				{
					move.Value = value;
					move.Conversion = conversion;
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

		[Given(@"I pass Account In url")]
		public void GivenIPassAccountInUrl()
		{
			accountUrl = accountInUrl;
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
					month = (Int16)current.Now.AddMonths(month).Month;
				}

				optionalMonth = month;
			}

			var yearString = dateData["Year"];
			year = Int16.Parse(yearString);

			if (isRelative(yearString))
			{
				year = (Int16)current.Now.AddYears(year).Year;
			}
		}

		private Boolean isRelative(String dateText)
		{
			return dateText.StartsWith("+") || dateText.StartsWith("-");
		}
		#endregion
	}
}
