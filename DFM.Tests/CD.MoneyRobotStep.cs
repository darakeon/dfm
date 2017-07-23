using System;
using TechTalk.SpecFlow;
using DFM.Entities;
using DFM.Entities.Enums;
using DFM.Entities.Extensions;
using NUnit.Framework;

namespace DFM.Tests
{
    [Binding]
    public class MoneyRobotStep : BaseStep
    {
        [Given(@"I have two accounts")]
        public void GivenIHaveTwoAccounts()
        {
            GetOrCreateAccount(AccountOutName);
            GetOrCreateAccount(AccountInName);
        }

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
                var detail = new Detail
                    {
                        Description = Move.Description,
                        Amount = 1,
                        Value = Int32.Parse(moveData["Value"])
                    };

                Move.DetailList.Add(detail);
            }

        }

        [Given(@"it has no Details")]
        public void GivenItHasNoDetails()
        {
            // TODO: empty detaillist
            Assert.AreEqual(1, Move.DetailList.Count); 
        }

        [Given(@"the move has this details")]
        public void GivenTheMoveHasThisDetails(Table table)
        {
            foreach (var detailData in table.Rows)
            {
                var detail = new Detail { Description = detailData["Description"] };

                if (!String.IsNullOrEmpty(detailData["Value"]))
                    detail.Value = Int32.Parse(detailData["Value"]);

                if (!String.IsNullOrEmpty(detailData["Amount"]))
                    detail.Amount = Int16.Parse(detailData["Amount"]);

                Move.DetailList.Add(detail);
            }
        }

        [Given(@"it has a Category")]
        public void GivenItHasACategory()
        {
            Move.Category = Category;
        }

        [Given(@"it has no Category")]
        public void GivenItHasNoCategory()
        {
            Move.Category = null;
        }

        [Given(@"it has an unknown Category")]
        public void GivenItHasAnUnknownCategory()
        {
            Move.Category = new Category { Name = "unknown", User = User };
        }

        [Given(@"it has an Account Out")]
        public void GivenItHasAnAccountOut()
        {
            AccountOut = GetOrCreateAccount(AccountOutName);
        }

        [Given(@"it has no Account Out")]
        public void GivenItHasNoAccountOut()
        {
            AccountOut = null;
        }

        [Given(@"it has an unknown Account Out")]
        public void GivenItHasAnUnknownAccountOut()
        {
            AccountOut = new Account { Name = "unknown", User = User };
        }

        [Given(@"it has an Account In")]
        public void GivenItHasAnAccountIn()
        {
            AccountIn = GetOrCreateAccount(AccountInName);
        }

        [Given(@"it has no Account In")]
        public void GivenItHasNoAccountIn()
        {
            AccountIn = null;
        }

        [Given(@"it has an unknown Account In")]
        public void GivenItHasAnUnknownAccountIn()
        {
            AccountIn = new Account { Name = "unknown", User = User };
        }

        [Given(@"it has an Account In equal to Out")]
        public void GivenItHasAnAccountInEqualToOut()
        {
            AccountOut = GetOrCreateAccount(AccountOutName);
            AccountIn = AccountOut;
        }

        [Then(@"the month-category-accountOut value will decrease in (\d+)")]
        public void ThenTheMonthCategoryAccountOutValueWillDecreaseIn(Double decrease)
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"the year-category-accountOut value will decrease in (\d+)")]
        public void ThenTheYearCategoryAccountOutValueWillDecreaseIn(Double decrease)
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"the month-category-accountOut value will increase in (\d+)")]
        public void ThenTheMonthCategoryAccountOutValueWillIncreaseIn(Double increase)
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"the year-category-accountOut value will increase in (\d+)")]
        public void ThenTheYearCategoryAccountOutValueWillIncreaseIn(Double increase)
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"the month-category-accountOut value will decrease in (\d+) plus the months until now")]
        public void ThenTheMonthCategoryAccountOutValueWillDecreaseInPlusTheMonthsUntilNow(Double decrease)
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"the year-category-accountOut value will decrease in (\d+) plus the months until now")]
        public void ThenTheYearCategoryAccountOutValueWillDecreaseInPlusTheMonthsUntilNow(Double decrease)
        {
            ScenarioContext.Current.Pending();
        }

    }
}
