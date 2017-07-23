using System;
using System.Linq;
using DFM.BusinessLogic.Exceptions;
using DFM.Entities;
using DFM.Entities.Enums;
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

        #region SelectMoveById
        [Given(@"I have a move")]
        public void GivenIHaveAMove()
        {
            Account = GetOrCreateAccount(CentralAccountName);

            Category = GetOrCreateCategory(CentralCategoryName);

            Move = new Move
            {
                Description = "Description",
                Date = DateTime.Today,
                Nature = MoveNature.Out,
            };

            // TODO: use this, delete above
            //    move.Value = 10;

            var detail = new Detail
            {
                Description = Move.Description,
                Amount = 1,
                Value = 10,
            };

            Move.DetailList.Add(detail);

            Move = SA.Money.SaveOrUpdateMove((Move)Move, Account, null, Category);
        }

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
            Account = GetOrCreateAccount(CentralAccountName);

            Category = GetOrCreateCategory(CentralCategoryName);

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
