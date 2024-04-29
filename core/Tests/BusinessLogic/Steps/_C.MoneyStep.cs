using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using DFM.BusinessLogic.Exceptions;
using DFM.BusinessLogic.Response;
using DFM.BusinessLogic.Tests.Helpers;
using DFM.Email;
using DFM.Entities;
using DFM.Entities.Bases;
using DFM.Entities.Enums;
using DFM.Generic;
using DFM.Generic.Datetime;
using DFM.Language;
using DFM.Tests.Util;
using Keon.Eml;
using Microsoft.Extensions.Primitives;
using NUnit.Framework;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace DFM.BusinessLogic.Tests.Steps
{
	[Binding]
	public class MoneyStep : BaseStep
	{
		public MoneyStep(ScenarioContext context)
			: base(context) { }

		#region Variables
		private Guid guid
		{
			get => get<Guid>("guid");
			set => set("guid", value);
		}

		private List<Guid> guids
		{
			get => get<List<Guid>>("guids");
			set => set("guids", value);
		}

		private DateTime oldDate
		{
			get => get<DateTime>("OldDate");
			set => set("OldDate", value);
		}

		private static String newAccountOutUrl => "new_" + accountOutUrl;

		private static String newAccountInUrl => "new_" + accountInUrl;

		private static String newCategoryName => "new " + mainCategoryName;

		private Decimal newAccountOutTotal
		{
			get => get<Decimal>("NewAccountOutTotal");
			set => set("NewAccountOutTotal", value);
		}

		private Decimal newYearCategoryAccountOutTotal
		{
			get => get<Decimal>("NewYearCategoryAccountOutTotal");
			set => set("NewYearCategoryAccountOutTotal", value);
		}

		private Decimal newMonthCategoryAccountOutTotal
		{
			get => get<Decimal>("NewMonthCategoryAccountOutTotal");
			set => set("NewMonthCategoryAccountOutTotal", value);
		}

		private Decimal newAccountInTotal
		{
			get => get<Decimal>("NewAccountInTotal");
			set => set("NewAccountInTotal", value);
		}

		private Decimal newYearCategoryAccountInTotal
		{
			get => get<Decimal>("NewYearCategoryAccountInTotal");
			set => set("NewYearCategoryAccountInTotal", value);
		}

		private Decimal newMonthCategoryAccountInTotal
		{
			get => get<Decimal>("NewMonthCategoryAccountInTotal");
			set => set("NewMonthCategoryAccountInTotal", value);
		}

		protected String csv
		{
			get => get<String>("csv");
			set => set("csv", value);
		}
		#endregion

		#region SaveMove
		[Given(@"I have this move to create")]
		public void GivenIHaveThisMoveToCreate(Table table)
		{
			var moveData = table.Rows[0];

			moveInfo = moveData.CreateInstance<MoveInfo>();

			if (!String.IsNullOrEmpty(moveData["Date"]))
				moveInfo.SetDate(DateTime.Parse(moveData["Date"]));
		}

		[Given(@"the move has this details")]
		public void GivenTheMoveHasThisDetails(Table table)
		{
			foreach (var detailData in table.Rows)
			{
				var detail = getDetailFromTable(detailData);
				moveInfo.DetailList.Add(detail);
			}
		}

		[When(@"I try to save the move")]
		public void WhenITryToSaveTheMove()
		{
			try
			{
				moveInfo.OutUrl = accountOut?.Url;
				moveInfo.InUrl = accountIn?.Url;
				moveInfo.CategoryName = categoryName;

				moveResult = service.Money.SaveMove(moveInfo);
			}
			catch (CoreError e)
			{
				error = e;
			}
		}

		[When(@"I try to save the move with e-mail system out")]
		public void WhenITryToSaveTheMoveWithEMailSystemOut()
		{
			TestSettings.ActivateMoveEmailForUser(service);
			TestSettings.BreakTheEmailSystem();

			try
			{
				moveInfo.OutUrl = accountOut?.Url;
				moveInfo.InUrl = accountIn?.Url;
				moveInfo.CategoryName = categoryName;

				moveResult = service.Money.SaveMove(moveInfo);
				currentEmailStatus = moveResult.Email;
			}
			catch (CoreError e)
			{
				error = e;
			}

			TestSettings.FixTheEmailSystem();
			TestSettings.DeactivateMoveEmailForUser(service);
		}

		[When(@"I try to save the move with e-mail system ok")]
		public void WhenITryToSaveTheMoveWithEMailSystemOk()
		{
			TestSettings.ActivateMoveEmailForUser(service);

			try
			{
				moveInfo.OutUrl = accountOut?.Url;
				moveInfo.InUrl = accountIn?.Url;
				moveInfo.CategoryName = categoryName;

				moveResult = service.Money.SaveMove(moveInfo);
				currentEmailStatus = moveResult.Email;
			}
			catch (CoreError e)
			{
				error = e;
			}

			TestSettings.DeactivateMoveEmailForUser(service);
		}

		[Then(@"the move value will be (\d+\.?\d*)")]
		public void ThenTheMoveValueWillBe(Decimal value)
		{
			var move = repos.Move.Get(moveResult.Guid);
			Assert.That(move.Value, Is.EqualTo(value));
		}

		[Then(@"the move will not be saved")]
		public void ThenTheMoveWillNotBeSaved()
		{
			Assert.That(moveResult?.Guid ?? Guid.Empty, Is.EqualTo(Guid.Empty));
		}

		[Then(@"the move will be saved")]
		public void ThenTheMoveWillBeSaved()
		{
			Assert.That(moveResult?.Guid ?? Guid.Empty, Is.Not.EqualTo(Guid.Empty));

			var newMove = service.Money.GetMove(moveResult?.Guid ?? Guid.Empty);

			Assert.That(newMove, Is.Not.Null);
		}

		[Then(@"I will receive the notification")]
		public void ThenIWillReceiveTheNotification()
		{
			Assert.That(currentEmailStatus.HasValue, Is.True);
			Assert.That(currentEmailStatus.Value, Is.EqualTo(EmailStatus.EmailNotSent));
		}

		[Then(@"I will receive no notification")]
		public void ThenIWillReceiveNoNotification()
		{
			Assert.That(currentEmailStatus.HasValue, Is.True);
			Assert.That(currentEmailStatus.Value, Is.EqualTo(EmailStatus.EmailSent));
		}

		[Then(@"the accountIn begin date will be (\d{4}\-\d{2}\-\d{2})")]
		public void ThenTheAccountInBeginDateWillBe(DateTime beginDate)
		{
			thenTheAccountBeginDateWillBe(accountInUrl, beginDate);
		}

		[Then(@"the accountOut begin date will be (\d{4}\-\d{2}\-\d{2})")]
		public void ThenTheAccountOutBeginDateWillBe(DateTime beginDate)
		{
			thenTheAccountBeginDateWillBe(accountOutUrl, beginDate);
		}

		private void thenTheAccountBeginDateWillBe(String url, DateTime beginDate)
		{
			var user = repos.User.GetByEmail(current.Email);
			var account = repos.Account.GetByUrl(url, user);

			Assert.That(account.BeginDate, Is.EqualTo(beginDate));
		}
		#endregion

		#region UpdateMove
		[Given(@"I change the move date in (\-?\d+\.?\d*) (day|month|year)s?")]
		public void GivenIChangeTheMoveDateIn(Int32 count, String frequency)
		{
			moveInfo.AddByFrequency(frequency, count);

			var user = repos.User.GetByEmail(current.Email);
			var category = repos.Category.GetByName(mainCategoryName, user);

			if (accountOut != null)
			{
				setAccountOutNewTotals(accountOut, category, moveInfo);
			}

			if (accountIn != null)
			{
				setAccountInNewTotals(accountIn, category, moveInfo);
			}
		}

		private void setAccountOutNewTotals(Account account, Category category, MoveInfo move)
		{
			newAccountOutTotal = repos.Summary.GetTotal(account);

			newYearCategoryAccountOutTotal =
				repos.Summary.Get(
					account, category, move.Year
				)?.Out ?? 0;

			newMonthCategoryAccountOutTotal =
				repos.Summary.Get(
					account, category, move.ToMonthYear()
				)?.Out ?? 0;
		}

		private void setAccountInNewTotals(Account account, Category category, MoveInfo move)
		{
			newAccountInTotal = repos.Summary.GetTotal(account);

			newYearCategoryAccountInTotal =
				repos.Summary.Get(
					account, category, move.Year
				)?.In ?? 0;

			newMonthCategoryAccountInTotal =
				repos.Summary.Get(
					account, category, move.ToMonthYear()
				)?.In ?? 0;
		}

		[Given("I get the move")]
		public void GivenIGetTheMove()
		{
			moveInfo = service.Money.GetMove(guid);
		}

		[When(@"I change the category of the move")]
		public void GivenIChangeTheCategoryOfTheMove()
		{
			categoryInfo = CategoryInfo.Convert(
				getOrCreateCategory(newCategoryName)
			);
			categoryName = newCategoryName;
		}

		[When(@"I change the account out of the move(?: to (\w+)(?: \(([A-Z]{3})\))?)?")]
		public void GivenIChangeTheAccountOutOfTheMove(String url, Currency? currency)
		{
			if (String.IsNullOrEmpty(url))
				url = newAccountOutUrl;

			accountOut = getOrCreateAccount(url, currency);

			var user = repos.User.GetByEmail(current.Email);
			var category = repos.Category.GetByName(mainCategoryName, user);
			setAccountOutNewTotals(accountOut, category, moveInfo);
		}

		[When(@"I change the account in of the move(?: to (\w+)(?: \(([A-Z]{3})\))?)?")]
		public void GivenIChangeTheAccountInOfTheMove(String url, Currency? currency)
		{
			if (String.IsNullOrEmpty(url))
				url = newAccountInUrl;

			accountIn = getOrCreateAccount(url, currency);

			var user = repos.User.GetByEmail(current.Email);
			var category = repos.Category.GetByName(mainCategoryName, user);
			setAccountInNewTotals(accountIn, category, moveInfo);
		}

		[When(@"I change the move out to in")]
		public void GivenIChangeTheMoveOutToIn()
		{
			moveInfo.Nature = MoveNature.In;

			accountOut = null;

			accountIn = getOrCreateAccount(newAccountInUrl);

			var user = repos.User.GetByEmail(current.Email);
			var category = repos.Category.GetByName(mainCategoryName, user);
			setAccountInNewTotals(accountIn, category, moveInfo);
		}

		[When(@"I change the move in to out")]
		public void GivenIChangeTheMoveInToOut()
		{
			moveInfo.Nature = MoveNature.Out;

			accountIn = null;

			accountOut = getOrCreateAccount(newAccountOutUrl);

			var user = repos.User.GetByEmail(current.Email);
			var category = repos.Category.GetByName(mainCategoryName, user);
			setAccountOutNewTotals(accountOut, category, moveInfo);
		}

		[When(@"I change the move value to (\d+)")]
		public void WhenIChangeTheMoveValueTo(Int32 value)
		{
			moveInfo.Value = value;
		}

		[When(@"I add these details to the move")]
		public void WhenIAddTheseDetailsToTheMove(Table details)
		{
			foreach (var detailData in details.Rows)
			{
				var newDetail = getDetailFromTable(detailData);

				moveInfo.DetailList.Add(newDetail);
			}
		}

		[When(@"I change the details of the move to")]
		public void WhenIChangeTheDetailsOfTheMoveTo(Table details)
		{
			moveInfo.DetailList = new List<DetailInfo>();

			foreach (var detailData in details.Rows)
			{
				var newDetail = getDetailFromTable(detailData);

				moveInfo.DetailList.Add(newDetail);
			}
		}

		[When(@"I update the move")]
		public void WhenIUpdateTheMove()
		{
			try
			{
				moveInfo.OutUrl = accountOut?.Url;
				moveInfo.InUrl = accountIn?.Url;
				moveInfo.CategoryName = categoryName;

				moveResult = service.Money.SaveMove(moveInfo);
			}
			catch (CoreError e)
			{
				error = e;
			}
		}


		[When(@"I update the move with e-mail system out")]
		public void WhenIUpdateTheMoveWithEMailSystemOut()
		{
			TestSettings.ActivateMoveEmailForUser(service);
			TestSettings.BreakTheEmailSystem();

			try
			{
				moveInfo.OutUrl = accountOut?.Url;
				moveInfo.InUrl = accountIn?.Url;
				moveInfo.CategoryName = categoryName;

				moveResult = service.Money.SaveMove(moveInfo);
				currentEmailStatus = moveResult.Email;
			}
			catch (CoreError e)
			{
				error = e;
			}

			TestSettings.FixTheEmailSystem();
			TestSettings.DeactivateMoveEmailForUser(service);
		}

		[When(@"I update the move with e-mail system ok")]
		public void WhenIUpdateTheMoveWithEMailSystemOk()
		{
			TestSettings.ActivateMoveEmailForUser(service);

			try
			{
				moveInfo.OutUrl = accountOut?.Url;
				moveInfo.InUrl = accountIn?.Url;
				moveInfo.CategoryName = categoryName;

				moveResult = service.Money.SaveMove(moveInfo);
				currentEmailStatus = moveResult.Email;
			}
			catch (CoreError e)
			{
				error = e;
			}

			TestSettings.DeactivateMoveEmailForUser(service);
		}

		[Then(@"the old-accountOut value will change in (\-?\d+\.?\d*)")]
		public void ThenTheOldAccountOutValueWillChangeIn(Decimal value)
		{
			var account = getOrCreateAccount(accountOutUrl);

			var currentTotal = repos.Summary.GetTotal(account);

			Assert.That(currentTotal, Is.EqualTo(accountOutTotal + value));
		}

		[Then(@"the new-accountOut value will change in (\-?\d+\.?\d*)")]
		public void ThenTheNewAccountOutValueWillChangeIn(Decimal value)
		{
			var account = getOrCreateAccount(accountOut.Name);

			var currentTotal = repos.Summary.GetTotal(account);

			Assert.That(currentTotal, Is.EqualTo(newAccountOutTotal + value));
		}

		[Then(@"the old-year-category-accountOut value will change in (\-?\d+\.?\d*)")]
		public void ThenTheOldYearCategoryAccountOutValueWillChangeIn(Decimal value)
		{
			var account = getOrCreateAccount(accountOutUrl);
			var user = repos.User.GetByEmail(current.Email);
			var category = repos.Category.GetByName(mainCategoryName, user);
			var summary = repos.Summary.Get(account, category, oldDate.Year);

			Assert.That(summary.Out, Is.EqualTo(yearCategoryAccountOutTotal + value));
		}

		[Then(@"the new-year-category-accountOut value will change in (\-?\d+\.?\d*)")]
		public void ThenTheNewYearCategoryAccountOutValueWillChangeIn(Decimal value)
		{
			var account = getOrCreateAccount(accountOut.Name);
			var user = repos.User.GetByEmail(current.Email);
			var category = repos.Category.GetByName(categoryInfo?.Name, user);
			var summary = repos.Summary.Get(account, category, moveInfo.Year);

			Assert.That(summary.Out, Is.EqualTo(newYearCategoryAccountOutTotal + value));
		}

		[Then(@"the old-month-category-accountOut value will change in (\-?\d+\.?\d*)")]
		public void ThenTheOldMonthCategoryAccountOutValueWillChangeIn(Decimal value)
		{
			var account = getOrCreateAccount(accountOutUrl);
			var user = repos.User.GetByEmail(current.Email);
			var category = repos.Category.GetByName(mainCategoryName, user);
			var summary = repos.Summary.Get(account, category, oldDate.ToMonthYear());

			Assert.That(summary.Out, Is.EqualTo(monthCategoryAccountOutTotal + value));
		}

		[Then(@"the new-month-category-accountOut value will change in (\-?\d+\.?\d*)")]
		public void ThenTheNewMonthCategoryAccountOutValueWillChangeIn(Decimal value)
		{
			var account = getOrCreateAccount(accountOut.Name);
			var user = repos.User.GetByEmail(current.Email);
			var category = repos.Category.GetByName(categoryInfo?.Name, user);
			var summary = repos.Summary.Get(account, category, moveInfo.ToMonthYear());

			Assert.That(summary.Out, Is.EqualTo(newMonthCategoryAccountOutTotal + value));
		}

		[Then(@"the old-accountIn value will change in (\-?\d+\.?\d*)")]
		public void ThenTheOldAccountInValueWillChangeIn(Decimal value)
		{
			var account = getOrCreateAccount(accountInUrl);

			var currentTotal = repos.Summary.GetTotal(account);

			Assert.That(currentTotal, Is.EqualTo(accountInTotal + value));
		}

		[Then(@"the new-accountIn value will change in (\-?\d+\.?\d*)")]
		public void ThenTheNewAccountInValueWillChangeIn(Decimal value)
		{
			var account = getOrCreateAccount(accountIn.Name);

			var currentTotal = repos.Summary.GetTotal(account);

			Assert.That(currentTotal, Is.EqualTo(newAccountInTotal + value));
		}

		[Then(@"the old-year-category-accountIn value will change in (\-?\d+\.?\d*)")]
		public void ThenTheOldYearCategoryAccountInValueWillChangeIn(Decimal value)
		{
			var account = getOrCreateAccount(accountInUrl);
			var user = repos.User.GetByEmail(current.Email);
			var category = repos.Category.GetByName(mainCategoryName, user);
			var summary = repos.Summary.Get(account, category, oldDate.Year);

			Assert.That(summary.In, Is.EqualTo(yearCategoryAccountInTotal + value));
		}

		[Then(@"the new-year-category-accountIn value will change in (\-?\d+\.?\d*)")]
		public void ThenTheNewYearCategoryAccountInValueWillChangeIn(Decimal value)
		{
			var account = getOrCreateAccount(accountIn.Name);
			var user = repos.User.GetByEmail(current.Email);
			var category = repos.Category.GetByName(categoryInfo?.Name, user);
			var summary = repos.Summary.Get(account, category, moveInfo.Year);

			Assert.That(summary.In, Is.EqualTo(newYearCategoryAccountInTotal + value));
		}

		[Then(@"the old-month-category-accountIn value will change in (\-?\d+\.?\d*)")]
		public void ThenTheOldMonthCategoryAccountInValueWillChangeIn(Decimal value)
		{
			var account = getOrCreateAccount(accountInUrl);
			var user = repos.User.GetByEmail(current.Email);
			var category = repos.Category.GetByName(mainCategoryName, user);
			var summary = repos.Summary.Get(account, category, oldDate.ToMonthYear());

			Assert.That(summary.In, Is.EqualTo(monthCategoryAccountInTotal + value));
		}

		[Then(@"the new-month-category-accountIn value will change in (\-?\d+\.?\d*)")]
		public void ThenTheNewMonthCategoryAccountInValueWillChangeIn(Decimal value)
		{
			var account = getOrCreateAccount(accountIn.Name);
			var user = repos.User.GetByEmail(current.Email);
			var category = repos.Category.GetByName(categoryInfo?.Name, user);
			var summary = repos.Summary.Get(account, category, moveInfo.ToMonthYear());

			Assert.That(summary.In, Is.EqualTo(newMonthCategoryAccountInTotal + value));
		}

		[Then(@"the month-accountOut value will not change")]
		public void ThenTheMonthAccountOutValueWillNotChange()
		{
			var account = getOrCreateAccount(accountOutUrl);
			var summary = repos.Summary.Get(account, oldDate.ToMonthYear());

			Assert.That(summary.Sum(s => s.Out), Is.EqualTo(monthAccountOutTotal));
		}

		[Then(@"the year-accountOut value will not change")]
		public void ThenTheYearAccountOutValueWillNotChange()
		{
			var account = getOrCreateAccount(accountOutUrl);
			var summary = repos.Summary.Get(account, oldDate.Year);

			Assert.That(summary.Sum(s => s.Out), Is.EqualTo(yearAccountOutTotal));
		}

		[Then(@"the Move will still be at the Schedule")]
		public void ThenTheMoveWillStillBeAtTheSchedule()
		{
			var move = repos.Move.Get(moveInfo.Guid);
			Assert.That(move.Schedule, Is.Not.Null);
			Assert.That(move.Schedule.Guid, Is.EqualTo(scheduleInfo.Guid));
		}

		[Then(@"the move is (not )?checked for account (Out|In)")]
		public void ThenTheMoveIsNotCheckedForAccountOutAnymore(Boolean @checked, PrimalMoveNature nature)
		{
			var move = repos.Move.Get(moveInfo.Guid);
			Assert.That(move.IsChecked(nature), Is.EqualTo(@checked));
		}

		[Then(@"the description will still be (.+)")]
		public void ThenTheDescriptionWillStillBeScheduleCb(String description)
		{
			var report = service.Report.GetMonthReport(
				moveInfo.OutUrl,
				moveInfo.Year,
				moveInfo.Month
			);

			var move = report.MoveList
				.Single(m => m.Guid == moveInfo.Guid);

			Assert.That(move.Description, Is.EqualTo(description));
		}
		#endregion

		#region GetMoveById
		[When(@"I try to get the move")]
		public void WhenITryToGetTheMove()
		{
			moveInfo = null;
			error = null;

			try
			{
				moveInfo = service.Money.GetMove(guid);
			}
			catch (CoreError e)
			{
				error = e;
			}
		}

		[Then(@"I will receive no move")]
		public void ThenIWillReceiveNoMove()
		{
			Assert.That(moveInfo, Is.Null);
		}

		[Then(@"I will receive the move")]
		public void ThenIWillReceiveTheMove()
		{
			Assert.That(moveInfo, Is.Not.Null);
		}

		[Then(@"the Move description will be (.+)")]
		public void ThenTheMoveDescriptionWillBe(String description)
		{
			Assert.That(moveInfo.Description, Is.EqualTo(description));
		}
		#endregion

		#region DeleteMove
		[Given(@"robot run the scheduler and get the move")]
		public void GivenIRunTheSchedulerAndGetTheMove()
		{
			robotRunSchedule();

			var schedule = repos.Schedule.Get(scheduleInfo.Guid);
			guid = schedule.MoveList.Last().Guid;
		}

		[Given(@"robot run the scheduler and get all the moves")]
		public void GivenIRunTheSchedulerAndGetAllTheMoves()
		{
			robotRunSchedule();

			var schedule = repos.Schedule.Get(scheduleInfo.Guid);
			guids = schedule.MoveList.Select(m => m.Guid).ToList();
		}

		[When(@"I try to delete the move")]
		public void WhenITryToDeleteTheMove()
		{
			try
			{
				service.Money.DeleteMove(guid);
			}
			catch (CoreError e)
			{
				error = e;
			}
		}

		[When(@"I try to delete all the moves")]
		public void WhenITryToDeleteAllTheMoves()
		{
			try
			{
				foreach (var moveId in guids)
				{
					service.Money.DeleteMove(moveId);
				}
			}
			catch (CoreError e)
			{
				error = e;
			}
		}

		[When(@"I try to delete the move with e-mail system ok")]
		public void WhenITryToDeleteTheMoveWithEMailSystemOk()
		{
			TestSettings.ActivateMoveEmailForUser(service);

			try
			{
				var result = service.Money.DeleteMove(guid);
				currentEmailStatus = result.Email;
			}
			catch (CoreError e)
			{
				error = e;
			}

			TestSettings.DeactivateMoveEmailForUser(service);
		}

		[When(@"I try to delete the move with e-mail system out")]
		public void WhenITryToDeleteTheMoveWithEMailSystemOut()
		{
			TestSettings.ActivateMoveEmailForUser(service);
			TestSettings.BreakTheEmailSystem();

			try
			{
				var result = service.Money.DeleteMove(guid);
				currentEmailStatus = result.Email;
			}
			catch (CoreError e)
			{
				error = e;
			}

			TestSettings.FixTheEmailSystem();
			TestSettings.DeactivateMoveEmailForUser(service);
		}

		[Then(@"the move will (not )?be deleted")]
		public void ThenTheMoveWillOrNotBeDeleted(Boolean deleted)
		{
			var move = repos.Move.Get(guid);
			Assert.That(move == null, Is.EqualTo(deleted));
		}
		#endregion

		#region ToggleMoveChecked
		[Given(@"the move is (not )?checked for account (In|Out)")]
		public void GivenTheMoveIsChecked(Boolean @checked, PrimalMoveNature nature)
		{
			var move = repos.Move.Get(moveInfo.Guid);
			var moveChecked = move.IsChecked(nature);

			if (moveChecked == @checked)
				return;

			if (@checked)
				service.Money.CheckMove(moveInfo.Guid, nature);
			else
				service.Money.UncheckMove(moveInfo.Guid, nature);
		}

		[When(@"I try to mark it as (not )?checked for account (In|Out)")]
		public void WhenIMarkItAsChecked(Boolean @checked, PrimalMoveNature nature)
		{
			try
			{
				if (@checked)
					service.Money.CheckMove(moveInfo.Guid, nature);
				else
					service.Money.UncheckMove(moveInfo.Guid, nature);
			}
			catch (CoreError e)
			{
				error = e;
			}
		}

		[Then(@"the move will (not )?be checked for account (In|Out)")]
		public void ThenTheMoveWillBeChecked(Boolean @checked, PrimalMoveNature nature)
		{
			var move = repos.Move.Get(moveInfo.Guid);
			Assert.That(move, Is.Not.Null);

			switch (nature)
			{
				case PrimalMoveNature.In:
					Assert.That(move.CheckedIn, Is.EqualTo(@checked));
					break;
				case PrimalMoveNature.Out:
					Assert.That(move.CheckedOut, Is.EqualTo(@checked));
					break;
				default:
					throw new NotImplementedException();
			}
		}

		#endregion

		#region Import Moves File
		[Given(@"a moves file with this content")]
		public void GivenAMovesFileWithThisContent(Table table)
		{
			var lines = new List<List<String>>
			{
				table.Header.ToList()
			};

			lines.AddRange(
				table.Rows.Select(
					r => r.Values.ToList()
				).ToList()
			);

			csv = String.Join(
				"\n",
				lines.Select(
					l => String.Join(",", l)
				)
			).ForScenario(scenarioCode);
		}

		[When(@"import moves file")]
		public void WhenImportMovesFile()
		{
			try
			{
				service.Money.ImportMovesFile(csv);
			}
			catch (CoreError e)
			{
				error = e;
			}
		}

		[Then(@"the pre-import data will be recorded")]
		public void ThenThePre_ImportDataWillBeRecorded()
		{
			var archive = repos.Archive.NewQuery()
				.OrderBy(a => a.ID, false)
				.FirstOrDefault;

			Assert.That(archive, Is.Not.Null);

			var csvLines = csv.Split("\n");

			Assert.That(archive.LineList.Count, Is.EqualTo(csvLines.Length - 1));

			for (var l = 0; l < archive.LineList.Count; l++)
			{
				var line = archive.LineList[l];
				var csvLine = CSVHelper.ToCsv(line);
				Assert.That(csvLine, Is.EqualTo(csvLines[l+1]));
			}
		}

		[Then("no email will be sent")]
		public void ThenNoEmailWillBeSent()
		{
			var inboxPath = Path.Combine(
				"..", "..", "..", "..", "..", "..", "outputs", "inbox"
			);
			var inbox = new DirectoryInfo(inboxPath);

			var emails = inbox.GetFiles("*.eml")
				.Where(f => f.CreationTimeUtc >= testStart)
				.Select(f => EmlReader.FromFile(f.FullName))
				.Where(e => e.Headers["To"] == userEmail)
				.ToList();

			Assert.That(emails.Count, Is.EqualTo(0));
		}
		#endregion

		#region MoreThanOne
		[Given(@"I have a move")]
		public void GivenIHaveAMoveWithValue()
		{
			makeMove(10, MoveNature.Out, null, null);
		}

		[Given(@"I have a move with these details \((\w+)\)")]
		public void GivenIHaveAMoveWithTheseDetails(MoveNature nature, Table details)
		{
			makeMove(details, nature);
		}

		private void makeMove(Table details, MoveNature nature)
		{
			makeJustMove(nature);

			foreach (var detailData in details.Rows)
			{
				var newDetail = getDetailFromTable(detailData);

				moveInfo.DetailList.Add(newDetail);
			}

			setMoveExternals(nature, null, null);
		}

		[Given(@"I have a move with value (\-?\d+\.?\d*) \((\w+)\)(?: at account (\w+))?")]
		public void GivenIHaveAMoveWithValue(Decimal value, MoveNature nature, String moveAccountUrl)
		{
			if (moveAccountUrl == "")
				moveAccountUrl = null;

			if (moveAccountUrl != null)
				getOrCreateAccount(moveAccountUrl);

			if (nature == MoveNature.Transfer && moveAccountUrl != null)
				throw new NotImplementedException();

			makeMove(
				value,
				nature,
				nature == MoveNature.Out ? moveAccountUrl?.IntoUrl() : null,
				nature == MoveNature.In ? moveAccountUrl?.IntoUrl() : null
			);
		}

		private void makeMove(Decimal value, MoveNature nature, String moveAccountOutUrl, String moveAccountInUrl)
		{
			makeJustMove(nature);

			moveInfo.Value = value;

			setMoveExternals(
				nature,
				moveAccountOutUrl,
				moveAccountInUrl
			);
		}

		[Given(@"I have a move with value in (\-?\d+\.?\d*) at account (\w+) \(([A-Z]{3})\) and out (\-?\d+\.?\d*) at account (\w+) \(([A-Z]{3})\)")]
		public void GivenIHaveAMoveWithValues(Decimal valueIn, String moveAccountIn, Currency moveAccountInCurrency, Decimal valueOut, String moveAccountOut, Currency moveAccountOutCurrency)
		{
			accountInUrl = moveAccountIn?.IntoUrl();
			accountOutUrl = moveAccountOut?.IntoUrl();

			getOrCreateAccount(accountInUrl, moveAccountInCurrency);
			getOrCreateAccount(accountOutUrl, moveAccountOutCurrency);

			makeMove(
				valueOut,
				valueIn,
				accountOutUrl,
				accountInUrl
			);
		}

		private void makeMove(Decimal value, Decimal? conversion, String moveAccountOutUrl, String moveAccountInUrl)
		{
			makeJustMove(MoveNature.Transfer);

			moveInfo.Value = value;
			moveInfo.Conversion = conversion;

			setMoveExternals(MoveNature.Transfer, moveAccountOutUrl, moveAccountInUrl);
		}

		private void makeJustMove(MoveNature nature)
		{
			oldDate = current.Now.Date;

			moveInfo = new MoveInfo
			{
				Description = "Description",
				Nature = nature,
			};

			moveInfo.SetDate(oldDate);
		}

		private void setMoveExternals(MoveNature nature, String moveAccountOutUrl, String moveAccountInUrl)
		{
			moveInfo.OutUrl =
				nature == MoveNature.In
					? null : moveAccountOutUrl ?? accountOutUrl;

			moveInfo.InUrl =
				nature == MoveNature.Out
					? null : moveAccountInUrl ?? accountInUrl;

			moveInfo.CategoryName =
				categoryName = mainCategoryName;

			var result = service.Money.SaveMove(moveInfo);

			moveInfo.Guid = result.Guid;

			var user = repos.User.GetByEmail(current.Email);
			var category = repos.Category.GetByName(mainCategoryName, user);

			if (moveInfo.OutUrl != null)
			{
				accountOut = repos.Account.GetByUrl(moveInfo.OutUrl, user);

				accountOutTotal = repos.Summary.GetTotal(accountOut);
				yearAccountOutTotal =
					repos.Summary.Get(
						accountOut, moveInfo.Year
					).Sum(s => s.Out);
				monthAccountOutTotal =
					repos.Summary.Get(
						accountOut, moveInfo.ToMonthYear()
					).Sum(s => s.Out);
				yearCategoryAccountOutTotal =
					repos.Summary.Get(
						accountOut, category, moveInfo.Year
					).Out;
				monthCategoryAccountOutTotal =
					repos.Summary.Get(
						accountOut, category, moveInfo.ToMonthYear()
					).Out;
			}

			if (moveInfo.InUrl != null)
			{
				accountIn = repos.Account.GetByUrl(moveInfo.InUrl, user);

				accountInTotal = repos.Summary.GetTotal(accountIn);
				yearAccountInTotal =
					repos.Summary.Get(
						accountIn, moveInfo.Year
					).Sum(s => s.In);
				monthAccountInTotal =
					repos.Summary.Get(
						accountIn, moveInfo.ToMonthYear()
					).Sum(s => s.In);
				yearCategoryAccountInTotal =
					repos.Summary.Get(
						accountIn, category, moveInfo.Year
					).In;
				monthCategoryAccountInTotal =
					repos.Summary.Get(
						accountIn, category, moveInfo.ToMonthYear()
					).In;
			}
		}

		[Given(@"I pass an id of Move that doesn't exist")]
		public void GivenIPassAnIdOfMoveThatDoesNotExist()
		{
			guid = Guid.Empty;
		}

		[Given(@"I pass valid Move ID")]
		public void GivenIPassValidMoveID()
		{
			guid = moveInfo.Guid;
		}

		[Given(@"I pass the first schedule move guid")]
		public void GivenIPassTheFirstScheduleMoveGuid()
		{
			var schedule = repos.Schedule.Get(scheduleInfo.Guid);
			var move = schedule.MoveList.First();

			guid = move.Guid;
		}

		[Then(@"the move e-mail will have an unsubscribe link")]
		public void ThenTheMoveE_MailWillHaveAnUnsubscribeLink()
		{
			var email = EmlHelper.ByPosition(-1);

			var user = repos.User.GetByEmail(current.Email);

			var lang = user.Settings.Language;
			var subject = PlainText.Email[
				"MoveNotification", lang, "Subject"
			].Text;
			Assert.That(email.Subject, Is.EqualTo(subject));

			var token = repos.Security
				.Where(
					s => s.Active
						&& s.User.ID == user.ID
						&& s.Action == SecurityAction.UnsubscribeMoveMail
				).MaxBy(s => s.ID)?.Token;

			Assert.That(token, Is.Not.Null);

			var link = $"https://dontflymoney.com/>UnsubscribeMoveMail>{token}";

			Assert.That(
				email.Body.Contains(link),
				() => $"{link}\nnot found at\n{email.Body}"
			);

			Assert.That(
				email.Headers["List-Unsubscribe-Post"],
				Is.EqualTo("List-Unsubscribe=One-Click")
			);

			Assert.That(
				email.Headers["List-Unsubscribe"],
				Is.EqualTo(link)
			);
		}
		#endregion
	}
}
