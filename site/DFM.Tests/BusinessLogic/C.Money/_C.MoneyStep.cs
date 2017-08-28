using System;
using System.Collections.Generic;
using System.Linq;
using DFM.BusinessLogic.Exceptions;
using DFM.Email;
using DFM.Entities;
using DFM.Entities.Enums;
using DFM.Entities.Extensions;
using DFM.Generic;
using DFM.Tests.BusinessLogic.Helpers;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace DFM.Tests.BusinessLogic.C.Money
{
    [Binding]
    public class MoneyStep : BaseStep
    {
        #region Variables
		private static Int32 id
		{
			get { return Get<Int32>("ID"); }
			set { Set("ID", value); }
		}

		private static List<Int32> ids
		{
			get { return Get<List<Int32>>("IDs"); }
			set { Set("IDs", value); }
		}

		private static Detail detail
        {
            get { return Get<Detail>("Detail"); }
            set { Set("Detail", value); }
        }

        private static DateTime oldDate
        {
            get { return Get<DateTime>("OldDate"); }
            set { Set("OldDate", value); }
        }

        private static String newAccountOutUrl => "new_" + AccountOutUrl;

	    private static String newAccountInUrl => "new_" + AccountInUrl;

	    private static String newCategoryName => "new " + MainCategoryName;

	    private static Decimal newAccountOutTotal
        {
            get { return Get<Decimal>("NewAccountOutTotal"); }
            set { Set("NewAccountOutTotal", value); }
        }

        private static Decimal newYearCategoryAccountOutTotal
        {
            get { return Get<Decimal>("NewYearCategoryAccountOutTotal"); }
            set { Set("NewYearCategoryAccountOutTotal", value); }
        }

        private static Decimal newMonthCategoryAccountOutTotal
        {
            get { return Get<Decimal>("NewMonthCategoryAccountOutTotal"); }
            set { Set("NewMonthCategoryAccountOutTotal", value); }
        }

        private static Decimal newAccountInTotal
        {
            get { return Get<Decimal>("NewAccountInTotal"); }
            set { Set("NewAccountInTotal", value); }
        }

        private static Decimal newYearCategoryAccountInTotal
        {
            get { return Get<Decimal>("NewYearCategoryAccountInTotal"); }
            set { Set("NewYearCategoryAccountInTotal", value); }
        }

        private static Decimal newMonthCategoryAccountInTotal
        {
            get { return Get<Decimal>("NewMonthCategoryAccountInTotal"); }
            set { Set("NewMonthCategoryAccountInTotal", value); }
        }
        #endregion


        #region SaveMove
        [Given(@"I have this move to create")]
        public void GivenIHaveThisMoveToCreate(Table table)
        {
            var moveData = table.Rows[0];

            Move = new Move { Description = moveData["Description"] };

            if (!String.IsNullOrEmpty(moveData["Date"]))
                Move.Date = DateTime.Parse(moveData["Date"]);

            if (!String.IsNullOrEmpty(moveData["Nature"]))
                Move.Nature = EnumX.Parse<MoveNature>(moveData["Nature"]);

			if (!String.IsNullOrEmpty(moveData["Value"]))
                Move.Value = Decimal.Parse(moveData["Value"]);
        }
        

        [When(@"I try to save the move")]
        public void WhenITryToSaveTheMove()
        {
            try
            {
                var accountOutUrl = AccountOut == null ? null : AccountOut.Url;
                var accountInUrl = AccountIn == null ? null : AccountIn.Url;

                SA.Money.SaveOrUpdateMove(Move, accountOutUrl, accountInUrl, CategoryName);
            }
            catch (DFMCoreException e)
            {
                Error = e;
            }
        }

        [When(@"I try to save the move with e-mail system out")]
        public void WhenITryToSaveTheMoveWithEMailSystemOut()
        {
            ConfigHelper.ActivateMoveEmailForUser(SA);
            ConfigHelper.BreakTheEmailSystem();

            try
            {
                var accountOutUrl = AccountOut == null ? null : AccountOut.Url;
                var accountInUrl = AccountIn == null ? null : AccountIn.Url;

                var result = SA.Money.SaveOrUpdateMove(Move, accountOutUrl, accountInUrl, CategoryName);
                CurrentEmailStatus = result.Error;
            }
            catch (DFMCoreException e)
            {
                Error = e;
            }

            ConfigHelper.FixTheEmailSystem();
            ConfigHelper.DeactivateMoveEmailForUser(SA);
        }

        [When(@"I try to save the move with e-mail system ok")]
        public void WhenITryToSaveTheMoveWithEMailSystemOk()
        {
            ConfigHelper.ActivateEmailSystem();
            ConfigHelper.ActivateMoveEmailForUser(SA);

            try
            {
                var accountOutUrl = AccountOut == null ? null : AccountOut.Url;
                var accountInUrl = AccountIn == null ? null : AccountIn.Url;

                var result = SA.Money.SaveOrUpdateMove(Move, accountOutUrl, accountInUrl, CategoryName);
                CurrentEmailStatus = result.Error;
            }
            catch (DFMCoreException e)
            {
                Error = e;
            }

            ConfigHelper.DeactivateMoveEmailForUser(SA);
            ConfigHelper.DeactivateEmailSystem();
        }



        [Then(@"the move will not be saved")]
        public void ThenTheMoveWillNotBeSaved()
        {
            Assert.AreEqual(0, Move.ID);
        }

        [Then(@"the move will be saved")]
        public void ThenTheMoveWillBeSaved()
        {
            Assert.AreNotEqual(0, Move.ID);

            var newMove = SA.Money.GetMoveById(Move.ID);

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
        [When(@"I change the move date in (\-?\d+\.?\d*) (\w+)")]
        public void GivenIChangeTheMoveDateIn1Day(Int32 count, String frequency)
        {
            switch (frequency)
            {
                case "day": case "days": Move.Date = Move.Date.AddDays(count); break;
                case "month": case "months": Move.Date = Move.Date.AddMonths(count); break;
                case "year": case "years": Move.Date = Move.Date.AddYears(count); break;
            }

            if (AccountOut != null)
            {
                var year = AccountOut[Move.Date.Year, true];
                var month = year[Move.Date.Month, true];

                newAccountOutTotal = AccountOut.Total();
                newYearCategoryAccountOutTotal = (year[MainCategoryName] ?? new Summary()).Out;
                newMonthCategoryAccountOutTotal = (month[MainCategoryName] ?? new Summary()).Out;
            }

            if (AccountIn != null)
            {
                var year = AccountIn[Move.Date.Year, true];
                var month = year[Move.Date.Month, true];

                newAccountInTotal = AccountIn.Total();
                newYearCategoryAccountInTotal = (year[MainCategoryName] ?? new Summary()).In;
                newMonthCategoryAccountInTotal = (month[MainCategoryName] ?? new Summary()).In;
            }
        }


        [When(@"I change the category of the move")]
        public void GivenIChangeTheCategoryOfTheMove()
        {
            Category = GetOrCreateCategory(newCategoryName);
            CategoryName = newCategoryName;
        }


        [When(@"I change the account out of the move")]
        public void GivenIChangeTheAccountOutOfTheMove()
        {
            AccountOut = GetOrCreateAccount(newAccountOutUrl);

            var year = AccountOut[Move.Date.Year, true];
            var month = year[Move.Date.Month, true];

            newAccountOutTotal = AccountOut.Total();
            newYearCategoryAccountOutTotal = (year[MainCategoryName] ?? new Summary()).Out;
            newMonthCategoryAccountOutTotal = (month[MainCategoryName] ?? new Summary()).Out;
        }


        [When(@"I change the account in of the move")]
        public void GivenIChangeTheAccountInOfTheMove()
        {
            AccountIn = GetOrCreateAccount(newAccountInUrl);

            var year = AccountIn[Move.Date.Year, true];
            var month = year[Move.Date.Month, true];

            newAccountInTotal = AccountIn.Total();
            newYearCategoryAccountInTotal = (year[MainCategoryName] ?? new Summary()).In;
            newMonthCategoryAccountInTotal = (month[MainCategoryName] ?? new Summary()).In;
        }


        [When(@"I change the move out to in")]
        public void GivenIChangeTheMoveOutToIn()
        {
            Move.Nature = MoveNature.In;

            AccountOut = null;

            AccountIn = GetOrCreateAccount(newAccountInUrl);

            var year = AccountIn[Move.Date.Year, true];
            var month = year[Move.Date.Month, true];

            newAccountInTotal = AccountIn.Total();
            newYearCategoryAccountInTotal = (year[MainCategoryName] ?? new Summary()).In;
            newMonthCategoryAccountInTotal = (month[MainCategoryName] ?? new Summary()).In;
        }


        [When(@"I change the move in to out")]
        public void GivenIChangeTheMoveInToOut()
        {
            Move.Nature = MoveNature.Out;

            AccountIn = null;

            AccountOut = GetOrCreateAccount(newAccountOutUrl);

            var year = AccountOut[Move.Date.Year, true];
            var month = year[Move.Date.Month, true];

            newAccountOutTotal = AccountOut.Total();
            newYearCategoryAccountOutTotal = (year[MainCategoryName] ?? new Summary()).Out;
            newMonthCategoryAccountOutTotal = (month[MainCategoryName] ?? new Summary()).Out;
        }

        [When(@"I change the move value to (\d+)")]
        public void WhenIChangeTheMoveValueTo(Int32 value)
        {
            Move.Value = value;
        }

        [When(@"I add these details to the move")]
        public void WhenIAddTheseDetailsToTheMove(Table details)
        {
            foreach (var detailData in details.Rows)
            {
                var newDetail = GetDetailFromTable(detailData);

                Move.DetailList.Add(newDetail);
            }

        }


        
        [When(@"I update the move")]
        public void WhenIUpdateTheMove()
        {
            try
            {
                var accountOutUrl = AccountOut == null ? null : AccountOut.Url;
                var accountInUrl = AccountIn == null ? null : AccountIn.Url;

                SA.Money.SaveOrUpdateMove(Move, accountOutUrl, accountInUrl, CategoryName);
            }
            catch (DFMCoreException e)
            {
                Error = e;
            }
        }


        [Then(@"the old-accountOut value will change in (\-?\d+\.?\d*)")]
        public void ThenTheOldAccountOutValueWillChangeIn(Decimal value)
        {
            var account = GetOrCreateAccount(AccountOutUrl);

            var currentTotal = account.Total();

            Assert.AreEqual(AccountOutTotal + value, currentTotal);
        }

        [Then(@"the new-accountOut value will change in (\-?\d+\.?\d*)")]
        public void ThenTheNewAccountOutValueWillChangeIn(Decimal value)
        {
            var account = GetOrCreateAccount(AccountOut.Name);

            var currentTotal = account.Total();

            Assert.AreEqual(newAccountOutTotal + value, currentTotal);
        }


        [Then(@"the old-year-category-accountOut value will change in (\-?\d+\.?\d*)")]
        public void ThenTheOldYearCategoryAccountOutValueWillChangeIn(Decimal value)
        {
            var account = GetOrCreateAccount(AccountOutUrl);

            var summary = account
                [(Int16)oldDate.Year]
                [MainCategoryName];

            Assert.AreEqual(YearCategoryAccountOutTotal + value, summary.Out);
        }

        [Then(@"the new-year-category-accountOut value will change in (\-?\d+\.?\d*)")]
        public void ThenTheNewYearCategoryAccountOutValueWillChangeIn(Decimal value)
        {

            var account = GetOrCreateAccount(AccountOut.Name);

            var summary = account
                [(Int16)Move.Date.Year]
                [Category.Name];

            Assert.AreEqual(newYearCategoryAccountOutTotal + value, summary.Out);
        }


        [Then(@"the old-month-category-accountOut value will change in (\-?\d+\.?\d*)")]
        public void ThenTheOldMonthCategoryAccountOutValueWillChangeIn(Decimal value)
        {
            var account = GetOrCreateAccount(AccountOutUrl);

            var summary = account
                [(Int16)oldDate.Year]
                [(Int16)oldDate.Month]
                [MainCategoryName];

            Assert.AreEqual(MonthCategoryAccountOutTotal + value, summary.Out);
        }

        [Then(@"the new-month-category-accountOut value will change in (\-?\d+\.?\d*)")]
        public void ThenTheNewMonthCategoryAccountOutValueWillChangeIn(Decimal value)
        {
            var account = GetOrCreateAccount(AccountOut.Name);

            var summary = account
                [(Int16)Move.Date.Year]
                [(Int16)Move.Date.Month]
                [Category.Name];

            Assert.AreEqual(newMonthCategoryAccountOutTotal + value, summary.Out);
        }


        [Then(@"the old-accountIn value will change in (\-?\d+\.?\d*)")]
        public void ThenTheOldAccountInValueWillChangeIn(Decimal value)
        {
            var account = GetOrCreateAccount(AccountInUrl);

            var currentTotal = account.Total();

            Assert.AreEqual(AccountInTotal + value, currentTotal);
        }

        [Then(@"the new-accountIn value will change in (\-?\d+\.?\d*)")]
        public void ThenTheNewAccountInValueWillChangeIn(Decimal value)
        {
            var account = GetOrCreateAccount(AccountIn.Name);

            var currentTotal = account.Total();

            Assert.AreEqual(newAccountInTotal + value, currentTotal);
        }


        [Then(@"the old-year-category-accountIn value will change in (\-?\d+\.?\d*)")]
        public void ThenTheOldYearCategoryAccountInValueWillChangeIn(Decimal value)
        {
            var account = GetOrCreateAccount(AccountInUrl);

            var summary = account
                [(Int16)oldDate.Year]
                [MainCategoryName];

            Assert.AreEqual(YearCategoryAccountInTotal + value, summary.In);
        }

        [Then(@"the new-year-category-accountIn value will change in (\-?\d+\.?\d*)")]
        public void ThenTheNewYearCategoryAccountInValueWillChangeIn(Decimal value)
        {

            var account = GetOrCreateAccount(AccountIn.Name);

            var summary = account
                [(Int16)Move.Date.Year]
                [Category.Name];

            Assert.AreEqual(newYearCategoryAccountInTotal + value, summary.In);
        }


        [Then(@"the old-month-category-accountIn value will change in (\-?\d+\.?\d*)")]
        public void ThenTheOldMonthCategoryAccountInValueWillChangeIn(Decimal value)
        {
            var account = GetOrCreateAccount(AccountInUrl);

            var summary = account
                [(Int16)oldDate.Year]
                [(Int16)oldDate.Month]
                [MainCategoryName];

            Assert.AreEqual(MonthCategoryAccountInTotal + value, summary.In);
        }

        [Then(@"the new-month-category-accountIn value will change in (\-?\d+\.?\d*)")]
        public void ThenTheNewMonthCategoryAccountInValueWillChangeIn(Decimal value)
        {
            var account = GetOrCreateAccount(AccountIn.Name);

            var summary = account
                [(Int16)Move.Date.Year]
                [(Int16)Move.Date.Month]
                [Category.Name];

            Assert.AreEqual(newMonthCategoryAccountInTotal + value, summary.In);
        }


        [Then(@"the month-accountOut value will not change")]
        public void ThenTheMonthAccountOutValueWillNotChange()
        {
            var account = GetOrCreateAccount(AccountOutUrl);

            var month = account
                [(Int16)oldDate.Year]
                [(Int16)oldDate.Month];

            Assert.AreEqual(MonthAccountOutTotal, month.Total());
        }

        [Then(@"the year-accountOut value will not change")]
        public void ThenTheYearAccountOutValueWillNotChange()
        {
            var account = GetOrCreateAccount(AccountOutUrl);

            var year = account
                [(Int16)oldDate.Year];

            Assert.AreEqual(YearAccountOutTotal, year.Total());
        }
        #endregion

        #region GetMoveById
        [When(@"I try to get the move")]
        public void WhenITryToGetTheMove()
        {
            Move = null;
            Error = null;

            try
            {
                Move = SA.Money.GetMoveById(id);
            }
            catch (DFMCoreException e)
            {
                Error = e;
            }
        }

        [Then(@"I will receive no move")]
        public void ThenIWillReceiveNoMove()
        {
            Assert.IsNull(Move);
        }

        [Then(@"I will receive the move")]
        public void ThenIWillReceiveTheMove()
        {
            Assert.IsNotNull(Move);
        }
        #endregion

        #region GetDetailById
        [Given(@"I have a move with details")]
        public void GivenIHaveAMoveWithDetails()
        {
            Account = GetOrCreateAccount(MainAccountUrl);

            Category = GetOrCreateCategory(MainCategoryName);

            Move = new Move
            {
                Description = "Description",
                Date = Current.User.Now(),
                Nature = MoveNature.Out,
            };

            for (var d = 0; d < 3; d++)
            {
                var newDetail = new Detail
                {
                    Description = "Detail " + d,
                    Amount = 1,
                    Value = 10,
                };

                Move.DetailList.Add(newDetail);
            }

            var result = SA.Money.SaveOrUpdateMove(Move, Account.Url, null, Category.Name);
            Move = result.Success;

            detail = Move.DetailList.First();
        }


        [Given(@"I pass an id of Detail that doesn't exist")]
        public void GivenIPassAnIdOfDetailThatDoesnTExist()
        {
            id = 0;
        }

        [Given(@"I pass valid Detail ID")]
        public void GivenIPassValidDetailID()
        {
            id = detail.ID;
        }

        [When(@"I try to get the detail")]
        public void WhenITryToGetTheDetail()
        {
            detail = null;
            Error = null;

            try
            {
                detail = SA.Money.GetDetailById(id);
            }
            catch (DFMCoreException e)
            {
                Error = e;
            }
        }

        [Then(@"I will receive no detail")]
        public void ThenIWillReceiveNoDetail()
        {
            Assert.IsNull(detail);
        }

        [Then(@"I will receive the detail")]
        public void ThenIWillReceiveTheDetail()
        {
            Assert.IsNotNull(detail);
        }
        #endregion

		#region DeleteMove
		[Given(@"I run the scheduler and get the move")]
		public void GivenIRunTheSchedulerAndGetTheMove()
		{
			SA.Robot.RunSchedule();
			id = Schedule.MoveList.Last().ID;
		}

		[Given(@"I run the scheduler and get all the moves")]
		public void GivenIRunTheSchedulerAndGetAllTheMoves()
		{
			SA.Robot.RunSchedule();
			ids = Schedule.MoveList.Select(m => m.ID).ToList();
		}

		[When(@"I try to delete the move")]
		public void WhenITryToDeleteTheMove()
		{
			try
			{
				SA.Money.DeleteMove(id);
			}
			catch (DFMCoreException e)
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
					SA.Money.DeleteMove(moveId);
				}
			}
			catch (DFMCoreException e)
			{
				Error = e;
			}
		}

        [When(@"I try to delete the move with e-mail system ok")]
        public void WhenITryToDeleteTheMoveWithEMailSystemOk()
        {
            ConfigHelper.ActivateEmailSystem();
            ConfigHelper.ActivateMoveEmailForUser(SA);

            try
            {
                var result = SA.Money.DeleteMove(id);
                CurrentEmailStatus = result.Error;
            }
            catch (DFMCoreException e)
            {
                Error = e;
            }

            ConfigHelper.DeactivateMoveEmailForUser(SA);
            ConfigHelper.DeactivateEmailSystem();
        }

        [When(@"I try to delete the move with e-mail system out")]
        public void WhenITryToDeleteTheMoveWithEMailSystemOut()
        {
            ConfigHelper.ActivateMoveEmailForUser(SA);
            ConfigHelper.BreakTheEmailSystem();

            try
            {
                var result = SA.Money.DeleteMove(id);
                CurrentEmailStatus = result.Error;
            }
            catch (DFMCoreException e)
            {
                Error = e;
            }

            ConfigHelper.FixTheEmailSystem();
            ConfigHelper.DeactivateMoveEmailForUser(SA);
        }

        [Then(@"the move will be deleted")]
        public void ThenTheMoveWillBeDeleted()
        {
            Error = null;

            try
            {
                SA.Money.GetMoveById(id);
            }
            catch (DFMCoreException e)
            {
                Error = e;
            }

            Assert.IsNotNull(Error);
            Assert.AreEqual(ExceptionPossibilities.InvalidMove, Error.Type);
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

                Move.DetailList.Add(newDetail);
            }

            setMoveExternals(nature);
        }

        private void makeMove(Decimal value, MoveNature nature = MoveNature.Out)
        {
            makeJustMove(nature);

            Move.Value = value;
            
            setMoveExternals(nature);
        }

        private void makeJustMove(MoveNature nature)
        {
            oldDate = new DateTime(2014, 12, 31);

            Move = new Move
            {
                Description = "Description",
                Date = oldDate,
                Nature = nature,
            };
        }

        private void setMoveExternals(MoveNature nature)
        {
            var accountOutUrl = nature == MoveNature.In
                                     ? null : AccountOutUrl;

            var accountInUrl = nature == MoveNature.Out
                                    ? null : AccountInUrl;

            CategoryName = MainCategoryName;

            SA.Money.SaveOrUpdateMove(Move, accountOutUrl, accountInUrl, MainCategoryName);

            if (accountOutUrl != null)
            {
                AccountOut = SA.Admin.GetAccountByUrl(accountOutUrl); 

                var year = AccountOut[Move.Date.Year, true];
                var month = year[Move.Date.Month, true];

                AccountOutTotal = AccountOut.Total();
                YearAccountOutTotal = year.Total();
                MonthAccountOutTotal = month.Total();
                YearCategoryAccountOutTotal = (year[MainCategoryName] ?? new Summary()).Out;
                MonthCategoryAccountOutTotal = (month[MainCategoryName] ?? new Summary()).Out;
            }

            if (accountInUrl != null)
            {
                AccountIn = SA.Admin.GetAccountByUrl(accountInUrl);

                var year = AccountIn[Move.Date.Year, true];
                var month = year[Move.Date.Month, true];

                AccountInTotal = AccountIn.Total();
                YearAccountInTotal = year.Total();
                MonthAccountInTotal = month.Total();
                YearCategoryAccountInTotal = (year[MainCategoryName] ?? new Summary()).In;
                MonthCategoryAccountInTotal = (month[MainCategoryName] ?? new Summary()).In;
            }
        }



        [Given(@"I pass an id of Move that doesn't exist")]
        public void GivenIPassAnIdOfMoveThatDoesnTExist()
        {
            id = 0;
        }

        [Given(@"I pass valid Move ID")]
        public void GivenIPassValidMoveID()
        {
            id = Move.ID;
        }
        #endregion







    }
}
