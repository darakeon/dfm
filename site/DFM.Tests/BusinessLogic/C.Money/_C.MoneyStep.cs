using System;
using System.Linq;
using DFM.BusinessLogic.Exceptions;
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

        private static String newAccountOutUrl
        {
            get { return "new_" + AccountOutUrl; }
        }

        private static String newAccountInUrl
        {
            get { return "new_" + AccountInUrl; }
        }

        private static String newCategoryName
        {
            get { return "new " + MainCategoryName; }
        }

        private static Double newAccountOutTotal
        {
            get { return Get<Double>("NewAccountOutTotal"); }
            set { Set("NewAccountOutTotal", value); }
        }

        private static Double newYearCategoryAccountOutTotal
        {
            get { return Get<Double>("NewYearCategoryAccountOutTotal"); }
            set { Set("NewYearCategoryAccountOutTotal", value); }
        }

        private static Double newMonthCategoryAccountOutTotal
        {
            get { return Get<Double>("NewMonthCategoryAccountOutTotal"); }
            set { Set("NewMonthCategoryAccountOutTotal", value); }
        }

        private static Double newAccountInTotal
        {
            get { return Get<Double>("NewAccountInTotal"); }
            set { Set("NewAccountInTotal", value); }
        }

        private static Double newYearCategoryAccountInTotal
        {
            get { return Get<Double>("NewYearCategoryAccountInTotal"); }
            set { Set("NewYearCategoryAccountInTotal", value); }
        }

        private static Double newMonthCategoryAccountInTotal
        {
            get { return Get<Double>("NewMonthCategoryAccountInTotal"); }
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

            // TODO: use this, delete above
            //if (moveData["Value"] != null)
            //    move.Value = Int32.Parse(moveData["Value"]);

            if (!String.IsNullOrEmpty(moveData["Value"]))
            {
                var newDetail = new Detail
                {
                    Description = Move.Description,
                    Amount = 1,
                    Value = Int32.Parse(moveData["Value"])
                };

                Move.DetailList.Add(newDetail);
            }
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
            ConfigHelper.ActivateEmailSystem();
            ConfigHelper.ActivateEmailForUser(SA);
            ConfigHelper.BreakTheEmailSystem();

            try
            {
                var accountOutUrl = AccountOut == null ? null : AccountOut.Url;
                var accountInUrl = AccountIn == null ? null : AccountIn.Url;

                var result = SA.Money.SaveOrUpdateMove(Move, accountOutUrl, accountInUrl, CategoryName);

                EmailError = result.Error;
            }
            catch (DFMCoreException e)
            {
                Error = e;
            }

            ConfigHelper.FixTheEmailSystem();
            ConfigHelper.DeactivateEmailForUser(SA);
            ConfigHelper.DeactivateEmailSystem();
        }

        [When(@"I try to save the move with e-mail system ok")]
        public void WhenITryToSaveTheMoveWithEMailSystemOk()
        {
            ConfigHelper.ActivateEmailSystem();
            ConfigHelper.ActivateEmailForUser(SA);

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

            ConfigHelper.DeactivateEmailForUser(SA);
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
            Assert.IsTrue(EmailError);
        }

        [Then(@"I will receive no notification")]
        public void ThenIWillReceiveNoNotification()
        {
            Assert.IsFalse(EmailError);
        }
        #endregion

        #region UpdateMove
        [When(@"I change the move date in (\-?\d+) (\w+)")]
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

                newAccountOutTotal = AccountOut.Sum();
                newYearCategoryAccountOutTotal = year.GetOrCreateSummary(Category).Out;
                newMonthCategoryAccountOutTotal = month.GetOrCreateSummary(Category).Out;
            }

            if (AccountIn != null)
            {
                var year = AccountIn[Move.Date.Year, true];
                var month = year[Move.Date.Month, true];

                newAccountInTotal = AccountIn.Sum();
                newYearCategoryAccountInTotal = year.GetOrCreateSummary(Category).In;
                newMonthCategoryAccountInTotal = month.GetOrCreateSummary(Category).In;
            }
        }


        [When(@"I change the category of the move")]
        public void GivenIChangeTheCategoryOfTheMove()
        {
            Category = GetOrCreateCategory(newCategoryName);
        }


        [When(@"I change the account out of the move")]
        public void GivenIChangeTheAccountOutOfTheMove()
        {
            AccountOut = GetOrCreateAccount(newAccountOutUrl);

            var year = AccountOut[Move.Date.Year, true];
            var month = year[Move.Date.Month, true];

            newAccountOutTotal = AccountOut.Sum();
            newYearCategoryAccountOutTotal = year.GetOrCreateSummary(Category).Out;
            newMonthCategoryAccountOutTotal = month.GetOrCreateSummary(Category).Out;
        }


        [When(@"I change the account in of the move")]
        public void GivenIChangeTheAccountInOfTheMove()
        {
            AccountIn = GetOrCreateAccount(newAccountInUrl);

            var year = AccountIn[Move.Date.Year, true];
            var month = year[Move.Date.Month, true];

            newAccountInTotal = AccountIn.Sum();
            newYearCategoryAccountInTotal = year.GetOrCreateSummary(Category).In;
            newMonthCategoryAccountInTotal = month.GetOrCreateSummary(Category).In;
        }


        [When(@"I change the move out to in")]
        public void GivenIChangeTheMoveOutToIn()
        {
            Move.Nature = MoveNature.In;

            AccountOut = null;

            AccountIn = GetOrCreateAccount(newAccountInUrl);

            var year = AccountIn[Move.Date.Year, true];
            var month = year[Move.Date.Month, true];

            newAccountInTotal = AccountIn.Sum();
            newYearCategoryAccountInTotal = year.GetOrCreateSummary(Category).In;
            newMonthCategoryAccountInTotal = month.GetOrCreateSummary(Category).In;
        }


        [When(@"I change the move in to out")]
        public void GivenIChangeTheMoveInToOut()
        {
            Move.Nature = MoveNature.Out;

            AccountIn = null;

            AccountOut = GetOrCreateAccount(newAccountOutUrl);

            var year = AccountOut[Move.Date.Year, true];
            var month = year[Move.Date.Month, true];

            newAccountOutTotal = AccountOut.Sum();
            newYearCategoryAccountOutTotal = year.GetOrCreateSummary(Category).Out;
            newMonthCategoryAccountOutTotal = month.GetOrCreateSummary(Category).Out;
        }

        [When(@"I change the move value to (\d+)")]
        public void WhenIChangeTheMoveValueTo(Int32 value)
        {
            // TODO change to value of move
            Move.DetailList[0].Value = value;
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

                SA.Money.SaveOrUpdateMove(Move, accountOutUrl, accountInUrl, Category.Name);
            }
            catch (DFMCoreException e)
            {
                Error = e;
            }
        }


        [Then(@"the old-accountOut value will change in (\-?\d+)")]
        public void ThenTheOldAccountOutValueWillChangeIn(Double value)
        {
            var account = GetOrCreateAccount(AccountOutUrl);

            var currentTotal = account.Sum();

            Assert.AreEqual(AccountOutTotal + value, currentTotal);
        }

        [Then(@"the new-accountOut value will change in (\-?\d+)")]
        public void ThenTheNewAccountOutValueWillChangeIn(Double value)
        {
            var account = GetOrCreateAccount(AccountOut.Name);

            var currentTotal = account.Sum();

            Assert.AreEqual(newAccountOutTotal + value, currentTotal);
        }


        [Then(@"the old-year-category-accountOut value will change in (\-?\d+)")]
        public void ThenTheOldYearCategoryAccountOutValueWillChangeIn(Double value)
        {
            var account = GetOrCreateAccount(AccountOutUrl);

            var summary = account
                [(Int16)oldDate.Year]
                [MainCategoryName];

            Assert.AreEqual(YearCategoryAccountOutTotal + value, summary.Out);
        }

        [Then(@"the new-year-category-accountOut value will change in (\-?\d+)")]
        public void ThenTheNewYearCategoryAccountOutValueWillChangeIn(Double value)
        {

            var account = GetOrCreateAccount(AccountOut.Name);

            var summary = account
                [(Int16)Move.Date.Year]
                [Category.Name];

            Assert.AreEqual(newYearCategoryAccountOutTotal + value, summary.Out);
        }


        [Then(@"the old-month-category-accountOut value will change in (\-?\d+)")]
        public void ThenTheOldMonthCategoryAccountOutValueWillChangeIn(Double value)
        {
            var account = GetOrCreateAccount(AccountOutUrl);

            var summary = account
                [(Int16)oldDate.Year]
                [(Int16)oldDate.Month]
                [MainCategoryName];

            Assert.AreEqual(MonthCategoryAccountOutTotal + value, summary.Out);
        }

        [Then(@"the new-month-category-accountOut value will change in (\-?\d+)")]
        public void ThenTheNewMonthCategoryAccountOutValueWillChangeIn(Double value)
        {
            var account = GetOrCreateAccount(AccountOut.Name);

            var summary = account
                [(Int16)Move.Date.Year]
                [(Int16)Move.Date.Month]
                [Category.Name];

            Assert.AreEqual(newMonthCategoryAccountOutTotal + value, summary.Out);
        }


        [Then(@"the old-accountIn value will change in (\-?\d+)")]
        public void ThenTheOldAccountInValueWillChangeIn(Double value)
        {
            var account = GetOrCreateAccount(AccountInUrl);

            var currentTotal = account.Sum();

            Assert.AreEqual(AccountInTotal + value, currentTotal);
        }

        [Then(@"the new-accountIn value will change in (\-?\d+)")]
        public void ThenTheNewAccountInValueWillChangeIn(Double value)
        {
            var account = GetOrCreateAccount(AccountIn.Name);

            var currentTotal = account.Sum();

            Assert.AreEqual(newAccountInTotal + value, currentTotal);
        }


        [Then(@"the old-year-category-accountIn value will change in (\-?\d+)")]
        public void ThenTheOldYearCategoryAccountInValueWillChangeIn(Double value)
        {
            var account = GetOrCreateAccount(AccountInUrl);

            var summary = account
                [(Int16)oldDate.Year]
                [MainCategoryName];

            Assert.AreEqual(YearCategoryAccountInTotal + value, summary.In);
        }

        [Then(@"the new-year-category-accountIn value will change in (\-?\d+)")]
        public void ThenTheNewYearCategoryAccountInValueWillChangeIn(Double value)
        {

            var account = GetOrCreateAccount(AccountIn.Name);

            var summary = account
                [(Int16)Move.Date.Year]
                [Category.Name];

            Assert.AreEqual(newYearCategoryAccountInTotal + value, summary.In);
        }


        [Then(@"the old-month-category-accountIn value will change in (\-?\d+)")]
        public void ThenTheOldMonthCategoryAccountInValueWillChangeIn(Double value)
        {
            var account = GetOrCreateAccount(AccountInUrl);

            var summary = account
                [(Int16)oldDate.Year]
                [(Int16)oldDate.Month]
                [MainCategoryName];

            Assert.AreEqual(MonthCategoryAccountInTotal + value, summary.In);
        }

        [Then(@"the new-month-category-accountIn value will change in (\-?\d+)")]
        public void ThenTheNewMonthCategoryAccountInValueWillChangeIn(Double value)
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

            Assert.AreEqual(MonthAccountOutTotal, month.Sum());
        }

        [Then(@"the year-accountOut value will not change")]
        public void ThenTheYearAccountOutValueWillNotChange()
        {
            var account = GetOrCreateAccount(AccountOutUrl);

            var year = account
                [(Int16)oldDate.Year];

            Assert.AreEqual(YearAccountOutTotal, year.Sum());
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
        public void GivenIHaveAMoveWithTheseDetails(String nature, Table details)
        {
            var moveNature = EnumX.Parse<MoveNature>(nature);

            makeMove(details, moveNature);
        }

        [Given(@"I have a move with value (\-?\d+) \((\w+)\)")]
        public void GivenIHaveAMoveWithValue(Double value, String nature)
        {
            var moveNature = EnumX.Parse<MoveNature>(nature);

            makeMove(value, moveNature);
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

        private void makeMove(Double value, MoveNature nature = MoveNature.Out)
        {
            makeJustMove(nature);

            // TODO: use this, delete above
            //    move.Value = value;

            var newDetail = new Detail
            {
                Description = Move.Description,
                Amount = 1,
                Value = value,
            };

            Move.DetailList.Add(newDetail);
            
            setMoveExternals(nature);
        }

        private void makeJustMove(MoveNature nature)
        {
            oldDate = Current.User.Now();

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

            Category = GetOrCreateCategory(MainCategoryName);

            SA.Money.SaveOrUpdateMove(Move, accountOutUrl, accountInUrl, Category.Name);

            if (accountOutUrl != null)
            {
                AccountOut = SA.Admin.GetAccountByUrl(accountOutUrl); 

                var year = AccountOut[Move.Date.Year, true];
                var month = year[Move.Date.Month, true];

                AccountOutTotal = AccountOut.Sum();
                YearAccountOutTotal = year.Sum();
                MonthAccountOutTotal = month.Sum();
                YearCategoryAccountOutTotal = year.GetOrCreateSummary(Category).Out;
                MonthCategoryAccountOutTotal = month.GetOrCreateSummary(Category).Out;
            }

            if (accountInUrl != null)
            {
                AccountIn = SA.Admin.GetAccountByUrl(accountInUrl);

                var year = AccountIn[Move.Date.Year, true];
                var month = year[Move.Date.Month, true];

                AccountInTotal = AccountIn.Sum();
                YearAccountInTotal = year.Sum();
                MonthAccountInTotal = month.Sum();
                YearCategoryAccountInTotal = year.GetOrCreateSummary(Category).In;
                MonthCategoryAccountInTotal = month.GetOrCreateSummary(Category).In;
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
