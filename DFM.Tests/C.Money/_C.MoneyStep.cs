using System;
using System.Linq;
using DFM.BusinessLogic.Exceptions;
using DFM.Entities;
using DFM.Entities.Enums;
using DFM.Entities.Extensions;
using DFM.Generic;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace DFM.Tests.C.Money
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

        private static String newAccountOutName
        {
            get { return "new " + AccountOutName; }
        }

        private static String newCategoryName
        {
            get { return "new " + CategoryName; }
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
                SA.Money.SaveOrUpdateMove((Move)Move, AccountOut, AccountIn, MoveCategory);
            }
            catch (DFMCoreException e)
            {
                Error = e;
            }
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

            var newMove = SA.Money.SelectMoveById(Move.ID);

            Assert.IsNotNull(newMove);
        }
        #endregion

        #region UpdateMove
        [Given(@"I change the move date in (\-?\d+) (\w+)")]
        public void GivenIChangeTheMoveDateIn1Day(Int32 count, String frequency)
        {
            switch (frequency)
            {
                case "day": case "days": Move.Date = Move.Date.AddDays(count); break;
                case "month": case "months": Move.Date = Move.Date.AddMonths(count); break;
                case "year": case "years": Move.Date = Move.Date.AddYears(count); break;
            }
        }

        
        [Given(@"I change the account out of the move")]
        public void GivenIChangeTheAccountOutOfTheMove()
        {
            AccountOut = GetOrCreateAccount(newAccountOutName);

            var year = AccountOut[Move.Date.Year] ?? new Year();
            var month = year[Move.Date.Month] ?? new Month();

            newAccountOutTotal = AccountOut.Sum();
            newYearCategoryAccountOutTotal = (year[Category.Name] ?? new Summary()).Out;
            newMonthCategoryAccountOutTotal = (month[Category.Name] ?? new Summary()).Out;
        }

        
        [Given(@"I change the category of the move")]
        public void GivenIChangeTheCategoryOfTheMove()
        {
            Category = GetOrCreateCategory(newCategoryName);
        }
        
        
        [When(@"I update the move")]
        public void WhenIUpdateTheMove()
        {
            try
            {
                SA.Money.SaveOrUpdateMove((Move)Move, AccountOut, AccountIn, Category);
            }
            catch (DFMCoreException e)
            {
                Error = e;
            }
        }


        [Then(@"the old-accountOut value will change in (\-?\d+)")]
        public void ThenTheOldAccountOutValueWillChangeIn(Double value)
        {
            var account = GetOrCreateAccount(AccountOutName);

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
            var account = GetOrCreateAccount(AccountOutName);

            var summary = account
                [(Int16)oldDate.Year]
                [CategoryName];

            Assert.AreEqual(YearCategoryAccountOutTotal + value, summary.Out);
        }

        [Then(@"the new-year-category-accountOut value will change in (\-?\d+)")]
        public void ThenTheNewYearCategoryAccountOutValueWillChangeIn(Double value)
        {

            var account = GetOrCreateAccount(AccountOut.Name);

            var summary = account
                [(Int16)Move.Date.Year]
                [CategoryName];

            Assert.AreEqual(newYearCategoryAccountOutTotal + value, summary.Out);
        }


        [Then(@"the old-month-category-accountOut value will change in (\-?\d+)")]
        public void ThenTheOldMonthCategoryAccountOutValueWillChangeIn(Double value)
        {
            var account = GetOrCreateAccount(AccountOutName);

            var summary = account
                [(Int16)oldDate.Year]
                [(Int16)oldDate.Month]
                [CategoryName];
            
            Assert.AreEqual(MonthCategoryAccountOutTotal + value, summary.Out);
        }

        [Then(@"the new-month-category-accountOut value will change in (\-?\d+)")]
        public void ThenTheNewMonthCategoryAccountOutValueWillChangeIn(Double value)
        {
            var account = GetOrCreateAccount(AccountOut.Name);

            var summary = account
                [(Int16)Move.Date.Year]
                [(Int16)Move.Date.Month]
                [CategoryName];

            Assert.AreEqual(newMonthCategoryAccountOutTotal + value, summary.Out);
        }


        [Then(@"the month-accountOut value will not change")]
        public void ThenTheMonthAccountOutValueWillNotChange()
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"the year-accountOut value will not change")]
        public void ThenTheYearAccountOutValueWillNotChange()
        {
            ScenarioContext.Current.Pending();
        }
        #endregion

        #region SelectMoveById
        [When(@"I try to get the move")]
        public void WhenITryToGetTheMove()
        {
            Move = null;
            Error = null;

            try
            {
                Move = SA.Money.SelectMoveById(id);
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

        #region SelectDetailById
        [Given(@"I have a move with details")]
        public void GivenIHaveAMoveWithDetails()
        {
            Account = GetOrCreateAccount(AccountName);

            Category = GetOrCreateCategory(CategoryName);

            Move = new Move
            {
                Description = "Description",
                Date = DateTime.Today,
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

            Move = SA.Money.SaveOrUpdateMove((Move)Move, Account, null, Category);

            detail = Move.DetailList.First();
        }


        [Given(@"I pass an id of Detail that doesn't exist")]
        public void GivenIPassAnIdOdDetailThatDoesnTExist()
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
                detail = SA.Money.SelectDetailById(id);
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
                SA.Money.SelectMoveById(id);
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

        [Given(@"I have a move with value (\-?\d+)")]
        public void GivenIHaveAMoveWithValue(Double value)
        {
            makeMove(value);
        }

        private void makeMove(Double value)
        {
            oldDate = DateTime.Today;

            Move = new Move
            {
                Description = "Description",
                Date = oldDate,
                Nature = MoveNature.Out,
            };

            // TODO: use this, delete above
            //    move.Value = value;

            var newDetail = new Detail
            {
                Description = Move.Description,
                Amount = 1,
                Value = value,
            };

            Move.DetailList.Add(newDetail);
            
            AccountOut = GetOrCreateAccount(AccountOutName);
            AccountIn = null;
            Category = GetOrCreateCategory(CategoryName);

            SA.Money.SaveOrUpdateMove((Move)Move, AccountOut, AccountIn, Category);

            AccountOut = GetOrCreateAccount(AccountOutName);

            var year = AccountOut[Move.Date.Year] ?? new Year();
            var month = year[Move.Date.Month] ?? new Month();

            AccountOutTotal = AccountOut.Sum();
            YearCategoryAccountOutTotal = (year[Category.Name] ?? new Summary()).Out;
            MonthCategoryAccountOutTotal = (month[Category.Name] ?? new Summary()).Out;
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
