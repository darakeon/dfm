﻿using System;
using System.Collections.Generic;
using System.Linq;
using DFM.BusinessLogic.Exceptions;
using DFM.BusinessLogic.Response;
using DFM.BusinessLogic.Tests.Helpers;
using DFM.Email;
using DFM.Entities;
using DFM.Entities.Bases;
using DFM.Entities.Enums;
using DFM.Generic;
using DFM.Tests.Util;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace DFM.BusinessLogic.Tests.C.Money
{
	[Binding]
	public class MoneyStep : BaseStep
	{
		#region Variables
		private static Guid guid
		{
			get => get<Guid>("guid");
			set => set("guid", value);
		}

		private static List<Guid> guids
		{
			get => get<List<Guid>>("guids");
			set => set("guids", value);
		}

		private static DateTime oldDate
		{
			get => get<DateTime>("OldDate");
			set => set("OldDate", value);
		}

		private static String newAccountOutUrl => "new_" + accountOutUrl;

		private static String newAccountInUrl => "new_" + accountInUrl;

		private static String newCategoryName => "new " + mainCategoryName;

		private static Decimal newAccountOutTotal
		{
			get => get<Decimal>("NewAccountOutTotal");
			set => set("NewAccountOutTotal", value);
		}

		private static Decimal newYearCategoryAccountOutTotal
		{
			get => get<Decimal>("NewYearCategoryAccountOutTotal");
			set => set("NewYearCategoryAccountOutTotal", value);
		}

		private static Decimal newMonthCategoryAccountOutTotal
		{
			get => get<Decimal>("NewMonthCategoryAccountOutTotal");
			set => set("NewMonthCategoryAccountOutTotal", value);
		}

		private static Decimal newAccountInTotal
		{
			get => get<Decimal>("NewAccountInTotal");
			set => set("NewAccountInTotal", value);
		}

		private static Decimal newYearCategoryAccountInTotal
		{
			get => get<Decimal>("NewYearCategoryAccountInTotal");
			set => set("NewYearCategoryAccountInTotal", value);
		}

		private static Decimal newMonthCategoryAccountInTotal
		{
			get => get<Decimal>("NewMonthCategoryAccountInTotal");
			set => set("NewMonthCategoryAccountInTotal", value);
		}
		#endregion

		#region SaveMove
		[Given(@"I have this move to create")]
		public void GivenIHaveThisMoveToCreate(Table table)
		{
			var moveData = table.Rows[0];

			moveInfo = new MoveInfo { Description = moveData["Description"] };

			if (!String.IsNullOrEmpty(moveData["Date"]))
				moveInfo.SetDate(DateTime.Parse(moveData["Date"]));

			if (!String.IsNullOrEmpty(moveData["Nature"]))
				moveInfo.Nature = EnumX.Parse<MoveNature>(moveData["Nature"]);

			if (!String.IsNullOrEmpty(moveData["Value"]))
				moveInfo.Value = Decimal.Parse(moveData["Value"]);
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
			ConfigHelper.ActivateMoveEmailForUser(service);
			ConfigHelper.BreakTheEmailSystem();

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

			ConfigHelper.FixTheEmailSystem();
			ConfigHelper.DeactivateMoveEmailForUser(service);
		}

		[When(@"I try to save the move with e-mail system ok")]
		public void WhenITryToSaveTheMoveWithEMailSystemOk()
		{
			ConfigHelper.ActivateMoveEmailForUser(service);

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

			ConfigHelper.DeactivateMoveEmailForUser(service);
		}

		[Then(@"the move value will be (\d+\.?\d*)")]
		public void ThenTheMoveValueWillBe(Decimal value)
		{
			var move = moveRepository.Get(moveResult.Guid);
			Assert.AreEqual(value, move.Value);
		}

		[Then(@"the move will not be saved")]
		public void ThenTheMoveWillNotBeSaved()
		{
			Assert.AreEqual(Guid.Empty, moveResult?.Guid ?? Guid.Empty);
		}

		[Then(@"the move will be saved")]
		public void ThenTheMoveWillBeSaved()
		{
			Assert.AreNotEqual(Guid.Empty, moveResult?.Guid ?? Guid.Empty);

			var newMove = service.Money.GetMove(moveResult?.Guid ?? Guid.Empty);

			Assert.IsNotNull(newMove);
		}

		[Then(@"I will receive the notification")]
		public void ThenIWillReceiveTheNotification()
		{
			Assert.IsTrue(currentEmailStatus.HasValue);
			Assert.AreEqual(EmailStatus.EmailNotSent, currentEmailStatus.Value);
		}

		[Then(@"I will receive no notification")]
		public void ThenIWillReceiveNoNotification()
		{
			Assert.IsTrue(currentEmailStatus.HasValue);
			Assert.AreEqual(EmailStatus.EmailSent, currentEmailStatus.Value);
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
			var user = userRepository.GetByEmail(current.Email);
			var account = accountRepository.GetByUrl(url, user);

			Assert.AreEqual(beginDate, account.BeginDate);
		}
		#endregion

		#region UpdateMove
		[Given(@"I change the move date in (\-?\d+\.?\d*) (day|month|year)s?")]
		public void GivenIChangeTheMoveDateIn(Int32 count, String frequency)
		{
			moveInfo.AddByFrequency(frequency, count);

			var user = userRepository.GetByEmail(current.Email);
			var category = categoryRepository.GetByName(mainCategoryName, user);

			if (accountOut != null)
			{
				setAccountOutNewTotals(accountOut, category, moveInfo);
			}

			if (accountIn != null)
			{
				setAccountInNewTotals(accountIn, category, moveInfo);
			}
		}

		private static void setAccountOutNewTotals(Account account, Category category, MoveInfo move)
		{
			newAccountOutTotal = summaryRepository.GetTotal(account);

			newYearCategoryAccountOutTotal =
				summaryRepository.Get(
					account, category, move.Year
				)?.Out ?? 0;

			newMonthCategoryAccountOutTotal =
				summaryRepository.Get(
					account, category, move.ToMonthYear()
				)?.Out ?? 0;
		}

		private static void setAccountInNewTotals(Account account, Category category, MoveInfo move)
		{
			newAccountInTotal = summaryRepository.GetTotal(account);

			newYearCategoryAccountInTotal =
				summaryRepository.Get(
					account, category, move.Year
				)?.In ?? 0;

			newMonthCategoryAccountInTotal =
				summaryRepository.Get(
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

		[When(@"I change the account out of the move")]
		public void GivenIChangeTheAccountOutOfTheMove()
		{
			accountOut = getOrCreateAccount(newAccountOutUrl);

			var user = userRepository.GetByEmail(current.Email);
			var category = categoryRepository.GetByName(mainCategoryName, user);
			setAccountOutNewTotals(accountOut, category, moveInfo);
		}

		[When(@"I change the account in of the move")]
		public void GivenIChangeTheAccountInOfTheMove()
		{
			accountIn = getOrCreateAccount(newAccountInUrl);

			var user = userRepository.GetByEmail(current.Email);
			var category = categoryRepository.GetByName(mainCategoryName, user);
			setAccountInNewTotals(accountIn, category, moveInfo);
		}

		[When(@"I change the move out to in")]
		public void GivenIChangeTheMoveOutToIn()
		{
			moveInfo.Nature = MoveNature.In;

			accountOut = null;

			accountIn = getOrCreateAccount(newAccountInUrl);

			var user = userRepository.GetByEmail(current.Email);
			var category = categoryRepository.GetByName(mainCategoryName, user);
			setAccountInNewTotals(accountIn, category, moveInfo);
		}

		[When(@"I change the move in to out")]
		public void GivenIChangeTheMoveInToOut()
		{
			moveInfo.Nature = MoveNature.Out;

			accountIn = null;

			accountOut = getOrCreateAccount(newAccountOutUrl);

			var user = userRepository.GetByEmail(current.Email);
			var category = categoryRepository.GetByName(mainCategoryName, user);
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

		[Then(@"the old-accountOut value will change in (\-?\d+\.?\d*)")]
		public void ThenTheOldAccountOutValueWillChangeIn(Decimal value)
		{
			var account = getOrCreateAccount(accountOutUrl);

			var currentTotal = summaryRepository.GetTotal(account);

			Assert.AreEqual(accountOutTotal + value, currentTotal);
		}

		[Then(@"the new-accountOut value will change in (\-?\d+\.?\d*)")]
		public void ThenTheNewAccountOutValueWillChangeIn(Decimal value)
		{
			var account = getOrCreateAccount(accountOut.Name);

			var currentTotal = summaryRepository.GetTotal(account);

			Assert.AreEqual(newAccountOutTotal + value, currentTotal);
		}

		[Then(@"the old-year-category-accountOut value will change in (\-?\d+\.?\d*)")]
		public void ThenTheOldYearCategoryAccountOutValueWillChangeIn(Decimal value)
		{
			var account = getOrCreateAccount(accountOutUrl);
			var user = userRepository.GetByEmail(current.Email);
			var category = categoryRepository.GetByName(mainCategoryName, user);
			var summary = summaryRepository.Get(account, category, oldDate.Year);

			Assert.AreEqual(yearCategoryAccountOutTotal + value, summary.Out);
		}

		[Then(@"the new-year-category-accountOut value will change in (\-?\d+\.?\d*)")]
		public void ThenTheNewYearCategoryAccountOutValueWillChangeIn(Decimal value)
		{
			var account = getOrCreateAccount(accountOut.Name);
			var user = userRepository.GetByEmail(current.Email);
			var category = categoryRepository.GetByName(categoryInfo?.Name, user);
			var summary = summaryRepository.Get(account, category, moveInfo.Year);

			Assert.AreEqual(newYearCategoryAccountOutTotal + value, summary.Out);
		}

		[Then(@"the old-month-category-accountOut value will change in (\-?\d+\.?\d*)")]
		public void ThenTheOldMonthCategoryAccountOutValueWillChangeIn(Decimal value)
		{
			var account = getOrCreateAccount(accountOutUrl);
			var user = userRepository.GetByEmail(current.Email);
			var category = categoryRepository.GetByName(mainCategoryName, user);
			var summary = summaryRepository.Get(account, category, oldDate.ToMonthYear());

			Assert.AreEqual(monthCategoryAccountOutTotal + value, summary.Out);
		}

		[Then(@"the new-month-category-accountOut value will change in (\-?\d+\.?\d*)")]
		public void ThenTheNewMonthCategoryAccountOutValueWillChangeIn(Decimal value)
		{
			var account = getOrCreateAccount(accountOut.Name);
			var user = userRepository.GetByEmail(current.Email);
			var category = categoryRepository.GetByName(categoryInfo?.Name, user);
			var summary = summaryRepository.Get(account, category, moveInfo.ToMonthYear());

			Assert.AreEqual(newMonthCategoryAccountOutTotal + value, summary.Out);
		}

		[Then(@"the old-accountIn value will change in (\-?\d+\.?\d*)")]
		public void ThenTheOldAccountInValueWillChangeIn(Decimal value)
		{
			var account = getOrCreateAccount(accountInUrl);

			var currentTotal = summaryRepository.GetTotal(account);

			Assert.AreEqual(accountInTotal + value, currentTotal);
		}

		[Then(@"the new-accountIn value will change in (\-?\d+\.?\d*)")]
		public void ThenTheNewAccountInValueWillChangeIn(Decimal value)
		{
			var account = getOrCreateAccount(accountIn.Name);

			var currentTotal = summaryRepository.GetTotal(account);

			Assert.AreEqual(newAccountInTotal + value, currentTotal);
		}

		[Then(@"the old-year-category-accountIn value will change in (\-?\d+\.?\d*)")]
		public void ThenTheOldYearCategoryAccountInValueWillChangeIn(Decimal value)
		{
			var account = getOrCreateAccount(accountInUrl);
			var user = userRepository.GetByEmail(current.Email);
			var category = categoryRepository.GetByName(mainCategoryName, user);
			var summary = summaryRepository.Get(account, category, oldDate.Year);

			Assert.AreEqual(yearCategoryAccountInTotal + value, summary.In);
		}

		[Then(@"the new-year-category-accountIn value will change in (\-?\d+\.?\d*)")]
		public void ThenTheNewYearCategoryAccountInValueWillChangeIn(Decimal value)
		{
			var account = getOrCreateAccount(accountIn.Name);
			var user = userRepository.GetByEmail(current.Email);
			var category = categoryRepository.GetByName(categoryInfo?.Name, user);
			var summary = summaryRepository.Get(account, category, moveInfo.Year);

			Assert.AreEqual(newYearCategoryAccountInTotal + value, summary.In);
		}

		[Then(@"the old-month-category-accountIn value will change in (\-?\d+\.?\d*)")]
		public void ThenTheOldMonthCategoryAccountInValueWillChangeIn(Decimal value)
		{
			var account = getOrCreateAccount(accountInUrl);
			var user = userRepository.GetByEmail(current.Email);
			var category = categoryRepository.GetByName(mainCategoryName, user);
			var summary = summaryRepository.Get(account, category, oldDate.ToMonthYear());

			Assert.AreEqual(monthCategoryAccountInTotal + value, summary.In);
		}

		[Then(@"the new-month-category-accountIn value will change in (\-?\d+\.?\d*)")]
		public void ThenTheNewMonthCategoryAccountInValueWillChangeIn(Decimal value)
		{
			var account = getOrCreateAccount(accountIn.Name);
			var user = userRepository.GetByEmail(current.Email);
			var category = categoryRepository.GetByName(categoryInfo?.Name, user);
			var summary = summaryRepository.Get(account, category, moveInfo.ToMonthYear());

			Assert.AreEqual(newMonthCategoryAccountInTotal + value, summary.In);
		}

		[Then(@"the month-accountOut value will not change")]
		public void ThenTheMonthAccountOutValueWillNotChange()
		{
			var account = getOrCreateAccount(accountOutUrl);
			var summary = summaryRepository.Get(account, oldDate.ToMonthYear());

			Assert.AreEqual(monthAccountOutTotal, summary.Sum(s => s.Out));
		}

		[Then(@"the year-accountOut value will not change")]
		public void ThenTheYearAccountOutValueWillNotChange()
		{
			var account = getOrCreateAccount(accountOutUrl);
			var summary = summaryRepository.Get(account, oldDate.Year);

			Assert.AreEqual(yearAccountOutTotal, summary.Sum(s => s.Out));
		}

		[Then(@"the Move will still be at the Schedule")]
		public void ThenTheMoveWillStillBeAtTheSchedule()
		{
			var move = moveRepository.Get(moveInfo.Guid);
			Assert.IsNotNull(move.Schedule);
			Assert.AreEqual(scheduleInfo.Guid, move.Schedule.Guid);
		}

		[Then(@"the move is (not )?checked for account (Out|In)")]
		public void ThenTheMoveIsNotCheckedForAccountOutAnymore(Boolean @checked, PrimalMoveNature nature)
		{
			var move = moveRepository.Get(moveInfo.Guid);
			Assert.AreEqual(@checked, move.IsChecked(nature));
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
			Assert.IsNull(moveInfo);
		}

		[Then(@"I will receive the move")]
		public void ThenIWillReceiveTheMove()
		{
			Assert.IsNotNull(moveInfo);
		}

		[Then(@"the Move description will be (.+)")]
		public void ThenTheMoveDescriptionWillBe(String description)
		{
			Assert.AreEqual(description, moveInfo.Description);
		}
		#endregion

		#region DeleteMove
		[Given(@"I run the scheduler and get the move")]
		public void GivenIRunTheSchedulerAndGetTheMove()
		{
			service.Robot.RunSchedule();

			var schedule = scheduleRepository.Get(scheduleInfo.Guid);
			guid = schedule.MoveList.Last().Guid;
		}

		[Given(@"I run the scheduler and get all the moves")]
		public void GivenIRunTheSchedulerAndGetAllTheMoves()
		{
			service.Robot.RunSchedule();

			var schedule = scheduleRepository.Get(scheduleInfo.Guid);
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
			ConfigHelper.ActivateMoveEmailForUser(service);

			try
			{
				var result = service.Money.DeleteMove(guid);
				currentEmailStatus = result.Email;
			}
			catch (CoreError e)
			{
				error = e;
			}

			ConfigHelper.DeactivateMoveEmailForUser(service);
		}

		[When(@"I try to delete the move with e-mail system out")]
		public void WhenITryToDeleteTheMoveWithEMailSystemOut()
		{
			ConfigHelper.ActivateMoveEmailForUser(service);
			ConfigHelper.BreakTheEmailSystem();

			try
			{
				var result = service.Money.DeleteMove(guid);
				currentEmailStatus = result.Email;
			}
			catch (CoreError e)
			{
				error = e;
			}

			ConfigHelper.FixTheEmailSystem();
			ConfigHelper.DeactivateMoveEmailForUser(service);
		}

		[Then(@"the move will (not )?be deleted")]
		public void ThenTheMoveWillOrNotBeDeleted(Boolean deleted)
		{
			var move = moveRepository.Get(guid);
			Assert.AreEqual(deleted, move == null);
		}
		#endregion

		#region ToggleMoveChecked
		[Given(@"the move is (not )?checked for account (In|Out)")]
		public void GivenTheMoveIsChecked(Boolean @checked, PrimalMoveNature nature)
		{
			var move = moveRepository.Get(moveInfo.Guid);
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
			var move = moveRepository.Get(moveInfo.Guid);
			Assert.IsNotNull(move);

			switch (nature)
			{
				case PrimalMoveNature.In:
					Assert.AreEqual(@checked, move.CheckedIn);
					break;
				case PrimalMoveNature.Out:
					Assert.AreEqual(@checked, move.CheckedOut);
					break;
				default:
					throw new NotImplementedException();
			}
		}

		#endregion

		#region MoreThanOne
		[Given(@"I have a move")]
		public void GivenIHaveAMoveWithValue()
		{
			makeMove(10);
		}

		[Given(@"I have a move with these details \((\w+)\)")]
		public void GivenIHaveAMoveWithTheseDetails(MoveNature nature, Table details)
		{
			makeMove(details, nature);
		}

		[Given(@"I have a move with value (\-?\d+\.?\d*) \((\w+)\)(?: at account (\w+))?")]
		public void GivenIHaveAMoveWithValue(Decimal value, MoveNature nature, String moveAccountUrl)
		{
			if (moveAccountUrl == "")
				moveAccountUrl = null;

			if (moveAccountUrl != null)
				getOrCreateAccount(moveAccountUrl);

			makeMove(value, nature, moveAccountUrl);
		}

		private void makeMove(Table details, MoveNature nature)
		{
			makeJustMove(nature);

			foreach (var detailData in details.Rows)
			{
				var newDetail = getDetailFromTable(detailData);

				moveInfo.DetailList.Add(newDetail);
			}

			setMoveExternals(nature);
		}

		private void makeMove(Decimal value, MoveNature nature = MoveNature.Out, String moveAccountUrl = null)
		{
			makeJustMove(nature);

			moveInfo.Value = value;

			setMoveExternals(nature, moveAccountUrl);
		}

		private void makeJustMove(MoveNature nature)
		{
			oldDate = new DateTime(2014, 12, 31);

			moveInfo = new MoveInfo
			{
				Description = "Description",
				Nature = nature,
			};

			moveInfo.SetDate(oldDate);
		}

		private void setMoveExternals(MoveNature nature, String moveAccountUrl = null)
		{
			moveInfo.OutUrl =
				nature == MoveNature.In
					? null : moveAccountUrl ?? accountOutUrl;

			moveInfo.InUrl =
				nature == MoveNature.Out
					? null : moveAccountUrl ?? accountInUrl;

			moveInfo.CategoryName =
				categoryName = mainCategoryName;

			var result = service.Money.SaveMove(moveInfo);

			moveInfo.Guid = result.Guid;

			var user = userRepository.GetByEmail(current.Email);
			var category = categoryRepository.GetByName(mainCategoryName, user);

			if (moveInfo.OutUrl != null)
			{
				accountOut = accountRepository.GetByUrl(moveInfo.OutUrl, user);

				accountOutTotal = summaryRepository.GetTotal(accountOut);
				yearAccountOutTotal =
					summaryRepository.Get(
						accountOut, moveInfo.Year
					).Sum(s => s.Out);
				monthAccountOutTotal =
					summaryRepository.Get(
						accountOut, moveInfo.ToMonthYear()
					).Sum(s => s.Out);
				yearCategoryAccountOutTotal =
					summaryRepository.Get(
						accountOut, category, moveInfo.Year
					).Out;
				monthCategoryAccountOutTotal =
					summaryRepository.Get(
						accountOut, category, moveInfo.ToMonthYear()
					).Out;
			}

			if (moveInfo.InUrl != null)
			{
				accountIn = accountRepository.GetByUrl(moveInfo.InUrl, user);

				accountInTotal = summaryRepository.GetTotal(accountIn);
				yearAccountInTotal =
					summaryRepository.Get(
						accountIn, moveInfo.Year
					).Sum(s => s.In);
				monthAccountInTotal =
					summaryRepository.Get(
						accountIn, moveInfo.ToMonthYear()
					).Sum(s => s.In);
				yearCategoryAccountInTotal =
					summaryRepository.Get(
						accountIn, category, moveInfo.Year
					).In;
				monthCategoryAccountInTotal =
					summaryRepository.Get(
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
			var schedule = scheduleRepository.Get(scheduleInfo.Guid);
			var move = schedule.MoveList.First();

			guid = move.Guid;
		}
		#endregion
	}
}
