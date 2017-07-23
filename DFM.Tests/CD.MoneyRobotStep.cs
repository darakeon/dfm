using System;
using DFM.BusinessLogic.Exceptions;
using TechTalk.SpecFlow;
using DFM.Entities;
using DFM.Entities.Enums;
using DFM.Entities.Extensions;
using NUnit.Framework;
using DFM.Entities.Bases;

namespace DFM.Tests
{
    [Binding]
    public class MoneyRobotStep : BaseStep
    {
        private const String accountOutName = "account out";
        private const String accountInName = "account in";

        private static Account accountOut
        {
            get { return Get<Account>("AccountOut"); }
            set { Set("AccountOut", value); }
        }

        private static Account accountIn
        {
            get { return Get<Account>("AccountIn"); }
            set { Set("AccountIn", value); }
        }

        private static BaseMove move
        {
            get { return Get<BaseMove>("Move"); }
            set { Set("Move", value); }
        }

        [Given(@"I have two accounts")]
        public void GivenIHaveTwoAccounts()
        {
            accountOut = GetOrCreateAccount(accountOutName);
            accountIn = GetOrCreateAccount(accountInName);
        }

        [Given(@"I have this move to create")]
        public void GivenIHaveThisMoveToCreate(Table table)
        {
            var moveData = table.Rows[0];

            move = new Move { Description = moveData["Description"] };

            if (moveData["Date"] != null)
                move.Date = DateTime.Parse(moveData["Date"]);

            if (moveData["Nature"] != null)
                move.Nature = EnumX.Parse<MoveNature>(moveData["Nature"]);

            // TODO: use this, delete above
            //if (moveData["Value"] != null)
            //    move.Value = Int32.Parse(moveData["Value"]);

            if (moveData["Value"] != null)
            {
                var detail = new Detail
                    {
                        Description = move.Description,
                        Amount = 1,
                        Value = Int32.Parse(moveData["Value"])
                    };

                move.DetailList.Add(detail);
            }

        }

        [Given(@"it has no Details")]
        public void GivenItHasNoDetails()
        {
            // TODO: empty detaillist
            Assert.AreEqual(1, move.DetailList.Count); 
        }

        [Given(@"the move has this details")]
        public void GivenTheMoveHasThisDetails(Table table)
        {
            foreach (var detailData in table.Rows)
            {
                var detail = new Detail { Description = detailData["Description"] };

                if (detailData["Value"] != null)
                    detail.Value = Int32.Parse(detailData["Value"]);

                if (detailData["Amount"] != null)
                    detail.Amount = Int16.Parse(detailData["Amount"]);

                move.DetailList.Add(detail);
            }
        }

        [Given(@"it has a Category")]
        public void GivenItHasACategory()
        {
            move.Category = Category;
        }

        [Given(@"it has no Category")]
        public void GivenItHasNoCategory()
        {
            move.Category = null;
        }

        [Given(@"it has an unknown Category")]
        public void GivenItHasAnUnknownCategory()
        {
            move.Category = new Category { Name = "unknown", User = User };
        }

        [Given(@"it has an Account Out")]
        public void GivenItHasAnAccountOut()
        {
            ScenarioContext.Current.Pending();
        }

        [Given(@"it has no Account Out")]
        public void GivenItHasNoAccountOut()
        {
            ScenarioContext.Current.Pending();
        }

        [Given(@"it has an unknown Account Out")]
        public void GivenItHasAnUnknownAccountOut()
        {
            ScenarioContext.Current.Pending();
        }

        [Given(@"it has an Account In")]
        public void GivenItHasAnAccountIn()
        {
            ScenarioContext.Current.Pending();
        }

        [Given(@"it has no Account In")]
        public void GivenItHasNoAccountIn()
        {
            ScenarioContext.Current.Pending();
        }

        [Given(@"it has an unknown Account In")]
        public void GivenItHasAnUnknownAccountIn()
        {
            ScenarioContext.Current.Pending();
        }

        [Given(@"it has an Account In equal to Out")]
        public void GivenItHasAnAccountInEqualToOut()
        {
            ScenarioContext.Current.Pending();
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
