using System;
using System.Collections.Generic;
using System.Linq;
using DFM.BusinessLogic.Exceptions;
using DFM.BusinessLogic.Response;
using DFM.Email;
using DFM.Entities;
using DFM.Entities.Enums;
using DFM.Generic;
using DFM.Tests.BusinessLogic.Helpers;
using NUnit.Framework;
using TechTalk.SpecFlow;
using error = DFM.BusinessLogic.Exceptions.Error;

namespace DFM.Tests.BusinessLogic.C.Money
{
	[Binding]
	public class MoneyStep : BaseStep
	{
		#region Variables
		private static Int64 id
		{
			get => Get<Int64>("ID");
			set => Set("ID", value);
		}

		private static List<Int64> ids
		{
			get => Get<List<Int64>>("IDs");
			set => Set("IDs", value);
		}

		private static DateTime oldDate
		{
			get => Get<DateTime>("OldDate");
			set => Set("OldDate", value);
		}

		private static String newAccountOutUrl => "new_" + AccountOutUrl;

		private static String newAccountInUrl => "new_" + AccountInUrl;

		private static String newCategoryName => "new " + MAIN_CATEGORY_NAME;

		private static Decimal newAccountOutTotal
		{
			get => Get<Decimal>("NewAccountOutTotal");
			set => Set("NewAccountOutTotal", value);
		}

		private static Decimal newYearCategoryAccountOutTotal
		{
			get => Get<Decimal>("NewYearCategoryAccountOutTotal");
			set => Set("NewYearCategoryAccountOutTotal", value);
		}

		private static Decimal newMonthCategoryAccountOutTotal
		{
			get => Get<Decimal>("NewMonthCategoryAccountOutTotal");
			set => Set("NewMonthCategoryAccountOutTotal", value);
		}

		private static Decimal newAccountInTotal
		{
			get => Get<Decimal>("NewAccountInTotal");
			set => Set("NewAccountInTotal", value);
		}

		private static Decimal newYearCategoryAccountInTotal
		{
			get => Get<Decimal>("NewYearCategoryAccountInTotal");
			set => Set("NewYearCategoryAccountInTotal", value);
		}

		private static Decimal newMonthCategoryAccountInTotal
		{
			get => Get<Decimal>("NewMonthCategoryAccountInTotal");
			set => Set("NewMonthCategoryAccountInTotal", value);
		}
		#endregion


		#region SaveMove
		[Given(@"I have this move to create")]
		public void GivenIHaveThisMoveToCreate(Table table)
		{
			var moveData = table.Rows[0];

			moveInfo = new MoveInfo { Description = moveData["Description"] };

			if (!String.IsNullOrEmpty(moveData["Date"]))
				moveInfo.Date = DateTime.Parse(moveData["Date"]);

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
				var detail = GetDetailFromTable(detailData);
				moveInfo.DetailList.Add(detail);
			}
		}

		[When(@"I try to save the move")]
		public void WhenITryToSaveTheMove()
		{
			try
			{
				moveInfo.OutUrl = AccountOut?.Url;
				moveInfo.InUrl = AccountIn?.Url;
				moveInfo.CategoryName = CategoryName;

				moveResult = Service.Money.SaveMove(moveInfo);
			}
			catch (CoreError e)
			{
				Error = e;
			}
		}

		[When(@"I try to save the move with e-mail system out")]
		public void WhenITryToSaveTheMoveWithEMailSystemOut()
		{
			ConfigHelper.ActivateMoveEmailForUser(Service);
			ConfigHelper.BreakTheEmailSystem();

			try
			{
				moveInfo.OutUrl = AccountOut?.Url;
				moveInfo.InUrl = AccountIn?.Url;
				moveInfo.CategoryName = CategoryName;

				moveResult = Service.Money.SaveMove(moveInfo);
				CurrentEmailStatus = moveResult.Email;
			}
			catch (CoreError e)
			{
				Error = e;
			}

			ConfigHelper.FixTheEmailSystem();
			ConfigHelper.DeactivateMoveEmailForUser(Service);
		}

		[When(@"I try to save the move with e-mail system ok")]
		public void WhenITryToSaveTheMoveWithEMailSystemOk()
		{
			ConfigHelper.ActivateEmailSystem();
			ConfigHelper.ActivateMoveEmailForUser(Service);

			try
			{
				moveInfo.OutUrl = AccountOut?.Url;
				moveInfo.InUrl = AccountIn?.Url;
				moveInfo.CategoryName = CategoryName;

				moveResult = Service.Money.SaveMove(moveInfo);
				CurrentEmailStatus = moveResult.Email;
			}
			catch (CoreError e)
			{
				Error = e;
			}

			ConfigHelper.DeactivateMoveEmailForUser(Service);
			ConfigHelper.DeactivateEmailSystem();
		}



		[Then(@"the move will not be saved")]
		public void ThenTheMoveWillNotBeSaved()
		{
			Assert.AreEqual(0, moveResult?.ID ?? 0);
		}

		[Then(@"the move will be saved")]
		public void ThenTheMoveWillBeSaved()
		{
			Assert.AreNotEqual(0, moveResult?.ID ?? 0);

			var newMove = Service.Money.GetMove(moveResult?.ID ?? 0);

			Assert.IsNotNull(newMove);
		}

		[Then(@"I will receive the notification")]
		public void ThenIWillReceiveTheNotification()
		{
			Assert.IsTrue(CurrentEmailStatus.HasValue);
			Assert.AreEqual(EmailStatus.EmailNotSent, CurrentEmailStatus.Value);
		}

		[Then(@"I will receive no notification")]
		public void ThenIWillReceiveNoNotification()
		{
			Assert.IsTrue(CurrentEmailStatus.HasValue);
			Assert.AreEqual(EmailStatus.EmailSent, CurrentEmailStatus.Value);
		}
		#endregion

		#region UpdateMove
		[Given(@"I change the move date in (\-?\d+\.?\d*) (day|month|year)s?")]
		public void GivenIChangeTheMoveDateIn1Day(Int32 count, String frequency)
		{
			switch (frequency)
			{
				case "day": moveInfo.Date = moveInfo.Date.AddDays(count); break;
				case "month": moveInfo.Date = moveInfo.Date.AddMonths(count); break;
				case "year": moveInfo.Date = moveInfo.Date.AddYears(count); break;
			}

			var user = userRepository.GetByEmail(Current.Email);
			var category = categoryRepository.GetByName(MAIN_CATEGORY_NAME, user);

			if (AccountOut != null)
			{
				setAccountOutNewTotals(AccountOut, category, moveInfo);
			}

			if (AccountIn != null)
			{
				setAccountInNewTotals(AccountIn, category, moveInfo);
			}
		}

		private static void setAccountOutNewTotals(Account account, Category category, MoveInfo move)
		{
			newAccountOutTotal = summaryRepository.GetTotal(account);

			newYearCategoryAccountOutTotal =
				summaryRepository.Get(
					account, category, move.Date.Year
				)?.Out ?? 0;

			newMonthCategoryAccountOutTotal =
				summaryRepository.Get(
					account, category, move.Date.ToMonthYear()
				)?.Out ?? 0;
		}

		private static void setAccountInNewTotals(Account account, Category category, MoveInfo move)
		{
			newAccountInTotal = summaryRepository.GetTotal(account);

			newYearCategoryAccountInTotal =
				summaryRepository.Get(
					account, category, move.Date.Year
				)?.In ?? 0;

			newMonthCategoryAccountInTotal =
				summaryRepository.Get(
					account, category, move.Date.ToMonthYear()
				)?.In ?? 0;
		}

		[Given(@"I get the Move at position (\d+) of the Schedule")]
		public void GivenIGetTheMoveOfTheSchedule(Int32 position)
		{
			var schedule = scheduleRepository.Get(scheduleInfo.ID);
			var move = schedule.MoveList[--position];
			moveInfo = Service.Money.GetMove(move.ID);
		}

		[When(@"I change the category of the move")]
		public void GivenIChangeTheCategoryOfTheMove()
		{
			Category = CategoryInfo.Convert(
				GetOrCreateCategory(newCategoryName)
			);
			CategoryName = newCategoryName;
		}

		[When(@"I change the account out of the move")]
		public void GivenIChangeTheAccountOutOfTheMove()
		{
			AccountOut = GetOrCreateAccount(newAccountOutUrl);

			var user = userRepository.GetByEmail(Current.Email);
			var category = categoryRepository.GetByName(MAIN_CATEGORY_NAME, user);
			setAccountOutNewTotals(AccountOut, category, moveInfo);
		}

		[When(@"I change the account in of the move")]
		public void GivenIChangeTheAccountInOfTheMove()
		{
			AccountIn = GetOrCreateAccount(newAccountInUrl);

			var user = userRepository.GetByEmail(Current.Email);
			var category = categoryRepository.GetByName(MAIN_CATEGORY_NAME, user);
			setAccountInNewTotals(AccountIn, category, moveInfo);
		}

		[When(@"I change the move out to in")]
		public void GivenIChangeTheMoveOutToIn()
		{
			moveInfo.Nature = MoveNature.In;

			AccountOut = null;

			AccountIn = GetOrCreateAccount(newAccountInUrl);

			var user = userRepository.GetByEmail(Current.Email);
			var category = categoryRepository.GetByName(MAIN_CATEGORY_NAME, user);
			setAccountInNewTotals(AccountIn, category, moveInfo);
		}

		[When(@"I change the move in to out")]
		public void GivenIChangeTheMoveInToOut()
		{
			moveInfo.Nature = MoveNature.Out;

			AccountIn = null;

			AccountOut = GetOrCreateAccount(newAccountOutUrl);

			var user = userRepository.GetByEmail(Current.Email);
			var category = categoryRepository.GetByName(MAIN_CATEGORY_NAME, user);
			setAccountOutNewTotals(AccountOut, category, moveInfo);
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
				var newDetail = GetDetailFromTable(detailData);

				moveInfo.DetailList.Add(newDetail);
			}
		}

		[When(@"I change the details of the move to")]
		public void WhenIChangeTheDetailsOfTheMoveTo(Table details)
		{
			moveInfo.DetailList = new List<DetailInfo>();

			foreach (var detailData in details.Rows)
			{
				var newDetail = GetDetailFromTable(detailData);

				moveInfo.DetailList.Add(newDetail);
			}
		}

		[When(@"I update the move")]
		public void WhenIUpdateTheMove()
		{
			try
			{
				moveInfo.OutUrl = AccountOut?.Url;
				moveInfo.InUrl = AccountIn?.Url;
				moveInfo.CategoryName = CategoryName;

				Service.Money.SaveMove(moveInfo);
			}
			catch (CoreError e)
			{
				Error = e;
			}
		}

		[Then(@"the old-accountOut value will change in (\-?\d+\.?\d*)")]
		public void ThenTheOldAccountOutValueWillChangeIn(Decimal value)
		{
			var account = GetOrCreateAccount(AccountOutUrl);

			var currentTotal = summaryRepository.GetTotal(account);

			Assert.AreEqual(AccountOutTotal + value, currentTotal);
		}

		[Then(@"the new-accountOut value will change in (\-?\d+\.?\d*)")]
		public void ThenTheNewAccountOutValueWillChangeIn(Decimal value)
		{
			var account = GetOrCreateAccount(AccountOut.Name);

			var currentTotal = summaryRepository.GetTotal(account);

			Assert.AreEqual(newAccountOutTotal + value, currentTotal);
		}

		[Then(@"the old-year-category-accountOut value will change in (\-?\d+\.?\d*)")]
		public void ThenTheOldYearCategoryAccountOutValueWillChangeIn(Decimal value)
		{
			var account = GetOrCreateAccount(AccountOutUrl);
			var user = userRepository.GetByEmail(Current.Email);
			var category = categoryRepository.GetByName(MAIN_CATEGORY_NAME, user);
			var summary = summaryRepository.Get(account, category, oldDate.Year);

			Assert.AreEqual(YearCategoryAccountOutTotal + value, summary.Out);
		}

		[Then(@"the new-year-category-accountOut value will change in (\-?\d+\.?\d*)")]
		public void ThenTheNewYearCategoryAccountOutValueWillChangeIn(Decimal value)
		{
			var account = GetOrCreateAccount(AccountOut.Name);
			var user = userRepository.GetByEmail(Current.Email);
			var category = categoryRepository.GetByName(Category?.Name, user);
			var summary = summaryRepository.Get(account, category, moveInfo.Date.Year);

			Assert.AreEqual(newYearCategoryAccountOutTotal + value, summary.Out);
		}

		[Then(@"the old-month-category-accountOut value will change in (\-?\d+\.?\d*)")]
		public void ThenTheOldMonthCategoryAccountOutValueWillChangeIn(Decimal value)
		{
			var account = GetOrCreateAccount(AccountOutUrl);
			var user = userRepository.GetByEmail(Current.Email);
			var category = categoryRepository.GetByName(MAIN_CATEGORY_NAME, user);
			var summary = summaryRepository.Get(account, category, oldDate.ToMonthYear());

			Assert.AreEqual(MonthCategoryAccountOutTotal + value, summary.Out);
		}

		[Then(@"the new-month-category-accountOut value will change in (\-?\d+\.?\d*)")]
		public void ThenTheNewMonthCategoryAccountOutValueWillChangeIn(Decimal value)
		{
			var account = GetOrCreateAccount(AccountOut.Name);
			var user = userRepository.GetByEmail(Current.Email);
			var category = categoryRepository.GetByName(Category?.Name, user);
			var summary = summaryRepository.Get(account, category, moveInfo.Date.ToMonthYear());

			Assert.AreEqual(newMonthCategoryAccountOutTotal + value, summary.Out);
		}

		[Then(@"the old-accountIn value will change in (\-?\d+\.?\d*)")]
		public void ThenTheOldAccountInValueWillChangeIn(Decimal value)
		{
			var account = GetOrCreateAccount(AccountInUrl);

			var currentTotal = summaryRepository.GetTotal(account);

			Assert.AreEqual(AccountInTotal + value, currentTotal);
		}

		[Then(@"the new-accountIn value will change in (\-?\d+\.?\d*)")]
		public void ThenTheNewAccountInValueWillChangeIn(Decimal value)
		{
			var account = GetOrCreateAccount(AccountIn.Name);

			var currentTotal = summaryRepository.GetTotal(account);

			Assert.AreEqual(newAccountInTotal + value, currentTotal);
		}

		[Then(@"the old-year-category-accountIn value will change in (\-?\d+\.?\d*)")]
		public void ThenTheOldYearCategoryAccountInValueWillChangeIn(Decimal value)
		{
			var account = GetOrCreateAccount(AccountInUrl);
			var user = userRepository.GetByEmail(Current.Email);
			var category = categoryRepository.GetByName(MAIN_CATEGORY_NAME, user);
			var summary = summaryRepository.Get(account, category, oldDate.Year);

			Assert.AreEqual(YearCategoryAccountInTotal + value, summary.In);
		}

		[Then(@"the new-year-category-accountIn value will change in (\-?\d+\.?\d*)")]
		public void ThenTheNewYearCategoryAccountInValueWillChangeIn(Decimal value)
		{
			var account = GetOrCreateAccount(AccountIn.Name);
			var user = userRepository.GetByEmail(Current.Email);
			var category = categoryRepository.GetByName(Category?.Name, user);
			var summary = summaryRepository.Get(account, category, moveInfo.Date.Year);

			Assert.AreEqual(newYearCategoryAccountInTotal + value, summary.In);
		}

		[Then(@"the old-month-category-accountIn value will change in (\-?\d+\.?\d*)")]
		public void ThenTheOldMonthCategoryAccountInValueWillChangeIn(Decimal value)
		{
			var account = GetOrCreateAccount(AccountInUrl);
			var user = userRepository.GetByEmail(Current.Email);
			var category = categoryRepository.GetByName(MAIN_CATEGORY_NAME, user);
			var summary = summaryRepository.Get(account, category, oldDate.ToMonthYear());

			Assert.AreEqual(MonthCategoryAccountInTotal + value, summary.In);
		}

		[Then(@"the new-month-category-accountIn value will change in (\-?\d+\.?\d*)")]
		public void ThenTheNewMonthCategoryAccountInValueWillChangeIn(Decimal value)
		{
			var account = GetOrCreateAccount(AccountIn.Name);
			var user = userRepository.GetByEmail(Current.Email);
			var category = categoryRepository.GetByName(Category?.Name, user);
			var summary = summaryRepository.Get(account, category, moveInfo.Date.ToMonthYear());

			Assert.AreEqual(newMonthCategoryAccountInTotal + value, summary.In);
		}

		[Then(@"the month-accountOut value will not change")]
		public void ThenTheMonthAccountOutValueWillNotChange()
		{
			var account = GetOrCreateAccount(AccountOutUrl);
			var summary = summaryRepository.Get(account, oldDate.ToMonthYear());

			Assert.AreEqual(MonthAccountOutTotal, summary.Sum(s => s.Out));
		}

		[Then(@"the year-accountOut value will not change")]
		public void ThenTheYearAccountOutValueWillNotChange()
		{
			var account = GetOrCreateAccount(AccountOutUrl);
			var summary = summaryRepository.Get(account, oldDate.Year);

			Assert.AreEqual(YearAccountOutTotal, summary.Sum(s => s.Out));
		}

		[Then(@"the move total will be (\-?\d+\.?\d*)")]
		public void ThenTheMoveTotalWillBe(Decimal value)
		{
			var move = Service.Money.GetMove(moveInfo.ID);
			Assert.AreEqual(value, move.Total);
		}

		[Then(@"the Move will still be at the Schedule")]
		public void ThenTheMoveWillStillBeAtTheSchedule()
		{
			var move = moveRepository.Get(moveInfo.ID);
			Assert.IsNotNull(move.Schedule);
			Assert.AreEqual(scheduleInfo.ID, move.Schedule.ID);
		}
		#endregion

		#region GetMoveById
		[When(@"I try to get the move")]
		public void WhenITryToGetTheMove()
		{
			moveInfo = null;
			Error = null;

			try
			{
				moveInfo = Service.Money.GetMove(id);
			}
			catch (CoreError e)
			{
				Error = e;
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
		#endregion

		#region DeleteMove
		[Given(@"I run the scheduler and get the move")]
		public void GivenIRunTheSchedulerAndGetTheMove()
		{
			Service.Robot.RunSchedule();

			var schedule = scheduleRepository.Get(scheduleInfo.ID);
			id = schedule.MoveList.Last().ID;
		}

		[Given(@"I run the scheduler and get all the moves")]
		public void GivenIRunTheSchedulerAndGetAllTheMoves()
		{
			Service.Robot.RunSchedule();

			var schedule = scheduleRepository.Get(scheduleInfo.ID);
			ids = schedule.MoveList.Select(m => m.ID).ToList();
		}

		[When(@"I try to delete the move")]
		public void WhenITryToDeleteTheMove()
		{
			try
			{
				Service.Money.DeleteMove(id);
			}
			catch (CoreError e)
			{
				Error = e;
			}
		}

		[When(@"I try to delete all the moves")]
		public void WhenITryToDeleteAllTheMoves()
		{
			try
			{
				foreach (var moveId in ids)
				{
					Service.Money.DeleteMove(moveId);
				}
			}
			catch (CoreError e)
			{
				Error = e;
			}
		}

		[When(@"I try to delete the move with e-mail system ok")]
		public void WhenITryToDeleteTheMoveWithEMailSystemOk()
		{
			ConfigHelper.ActivateEmailSystem();
			ConfigHelper.ActivateMoveEmailForUser(Service);

			try
			{
				var result = Service.Money.DeleteMove(id);
				CurrentEmailStatus = result.Email;
			}
			catch (CoreError e)
			{
				Error = e;
			}

			ConfigHelper.DeactivateMoveEmailForUser(Service);
			ConfigHelper.DeactivateEmailSystem();
		}

		[When(@"I try to delete the move with e-mail system out")]
		public void WhenITryToDeleteTheMoveWithEMailSystemOut()
		{
			ConfigHelper.ActivateMoveEmailForUser(Service);
			ConfigHelper.BreakTheEmailSystem();

			try
			{
				var result = Service.Money.DeleteMove(id);
				CurrentEmailStatus = result.Email;
			}
			catch (CoreError e)
			{
				Error = e;
			}

			ConfigHelper.FixTheEmailSystem();
			ConfigHelper.DeactivateMoveEmailForUser(Service);
		}

		[Then(@"the move will (not )?be deleted")]
		public void ThenTheMoveWillOrNotBeDeleted(Boolean deleted)
		{
			Error = null;

			try
			{
				Service.Money.GetMove(id);
			}
			catch (CoreError e)
			{
				Error = e;
			}

			if (deleted)
			{
				Assert.IsNotNull(Error);
				Assert.AreEqual(error.InvalidMove, Error.Type);
			}
			else
			{
				Assert.IsNull(Error);
			}
		}
		#endregion

		#region ToggleMoveChecked
		[Given(@"the move is (not )?checked")]
		public void GivenTheMoveIsChecked(Boolean @checked)
		{
			if (moveInfo.Checked == @checked)
				return;

			if (@checked)
				Service.Money.CheckMove(moveInfo.ID);
			else
				Service.Money.UncheckMove(moveInfo.ID);
		}

		[When(@"I try to mark it as (not )?checked")]
		public void WhenIMarkItAsChecked(Boolean @checked)
		{
			try
			{
				if (@checked)
				{
					Service.Money.CheckMove(moveInfo.ID);
				}
				else
				{
					Service.Money.UncheckMove(moveInfo.ID);
				}
			}
			catch (CoreError e)
			{
				Error = e;
			}
		}

		[Then(@"the move will (not )?be checked")]
		public void ThenTheMoveWillBeChecked(Boolean @checked)
		{
			var move = moveRepository.Get(moveInfo.ID);

			Assert.IsNotNull(move);
			Assert.AreEqual(@checked, move.Checked);
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

		[Given(@"I have a move with value (\-?\d+\.?\d*) \((\w+)\)")]
		public void GivenIHaveAMoveWithValue(Decimal value, MoveNature nature)
		{
			makeMove(value, nature);
		}

		private void makeMove(Table details, MoveNature nature)
		{
			makeJustMove(nature);

			foreach (var detailData in details.Rows)
			{
				var newDetail = GetDetailFromTable(detailData);

				moveInfo.DetailList.Add(newDetail);
			}

			setMoveExternals(nature);
		}

		private void makeMove(Decimal value, MoveNature nature = MoveNature.Out)
		{
			makeJustMove(nature);

			moveInfo.Value = value;

			setMoveExternals(nature);
		}

		private void makeJustMove(MoveNature nature)
		{
			oldDate = new DateTime(2014, 12, 31);

			moveInfo = new MoveInfo
			{
				Description = "Description",
				Date = oldDate,
				Nature = nature,
			};
		}

		private void setMoveExternals(MoveNature nature)
		{
			var accountOutUrl =
				moveInfo.OutUrl =
				nature == MoveNature.In
					? null : AccountOutUrl;

			var accountInUrl =
				moveInfo.InUrl =
				nature == MoveNature.Out
					? null : AccountInUrl;

			moveInfo.CategoryName =
				CategoryName = MAIN_CATEGORY_NAME;

			var result = Service.Money.SaveMove(moveInfo);

			moveInfo.ID = result.ID;

			var user = userRepository.GetByEmail(Current.Email);
			var category = categoryRepository.GetByName(MAIN_CATEGORY_NAME, user);

			if (accountOutUrl != null)
			{
				AccountOut = accountRepository.GetByUrl(accountOutUrl, user);

				AccountOutTotal = summaryRepository.GetTotal(AccountOut);
				YearAccountOutTotal =
					summaryRepository.Get(
						AccountOut, moveInfo.Date.Year
					).Sum(s => s.Out);
				MonthAccountOutTotal =
					summaryRepository.Get(
						AccountOut, moveInfo.Date.ToMonthYear()
					).Sum(s => s.Out);
				YearCategoryAccountOutTotal =
					summaryRepository.Get(
						AccountOut, category, moveInfo.Date.Year
					).Out;
				MonthCategoryAccountOutTotal =
					summaryRepository.Get(
						AccountOut, category, moveInfo.Date.ToMonthYear()
					).Out;
			}

			if (accountInUrl != null)
			{
				AccountIn = accountRepository.GetByUrl(accountInUrl, user);

				AccountInTotal = summaryRepository.GetTotal(AccountIn);
				YearAccountInTotal =
					summaryRepository.Get(
						AccountIn, moveInfo.Date.Year
					).Sum(s => s.In);
				MonthAccountInTotal =
					summaryRepository.Get(
						AccountIn, moveInfo.Date.ToMonthYear()
					).Sum(s => s.In);
				YearCategoryAccountInTotal =
					summaryRepository.Get(
						AccountIn, category, moveInfo.Date.Year
					).In;
				MonthCategoryAccountInTotal =
					summaryRepository.Get(
						AccountIn, category, moveInfo.Date.ToMonthYear()
					).In;
			}
		}

		[Given(@"I pass an id of Move that doesn't exist")]
		public void GivenIPassAnIdOfMoveThatDoesNotExist()
		{
			id = 0;
		}

		[Given(@"I pass valid Move ID")]
		public void GivenIPassValidMoveID()
		{
			id = moveInfo.ID;
		}
		#endregion
	}
}
