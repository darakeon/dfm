using System;
using DFM.Entities;
using DFM.Entities.Enums;
using DFM.Entities.Extensions;
using NUnit.Framework;
using TechTalk.SpecFlow;

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

        [Given(@"it has no Details")]
        public void GivenItHasNoDetails()
        {
            // TODO: empty detaillist
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
            MoveCategory = Category;
        }

        [Given(@"it has no Category")]
        public void GivenItHasNoCategory()
        {
            MoveCategory = null;
        }

        [Given(@"it has an unknown Category")]
        public void GivenItHasAnUnknownCategory()
        {
            MoveCategory = new Category { Name = "unknown", User = User };
        }

        [Given(@"it has a disabled Category")]
        public void GivenItHasADisabledCategory()
        {
            MoveCategory = new Category { Name = "disabled", User = User };

            SA.Admin.SaveOrUpdateCategory(MoveCategory);
            SA.Admin.DisableCategory(MoveCategory.ID);
        }


        [Given(@"it has an Account Out")]
        public void GivenItHasAnAccountOut()
        {
            AccountOut = GetOrCreateAccount(AccountOutName);

            var year = AccountOut[Move.Date.Year] ?? new Year();
            var month = year[Move.Date.Month] ?? new Month();

            AccountOutTotal = AccountOut.Sum();
            YearCategoryAccountOutTotal = (year[Category.Name] ?? new Summary()).Out;
            MonthCategoryAccountOutTotal = (month[Category.Name] ?? new Summary()).Out;
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

        [Given(@"it has a closed Account Out")]
        public void GivenItHasAClosedAccountOut()
        {
            AccountOut = new Account { Name = "closed out", User = User };

            SA.Admin.SaveOrUpdateAccount(AccountOut);

            var move = new Move
            {
                Date = DateTime.Now,
                Description = "Description",
                Nature = MoveNature.Out
            };

            // TODO: Remove this, put Value
            var detail = new Detail { Amount = 1, Description = move.Description, Value = 10 };

            move.DetailList.Add(detail);

            SA.Money.SaveOrUpdateMove(move, AccountOut, null, Category);

            SA.Admin.CloseAccount(AccountOut.ID);
        }


        [Given(@"it has an Account In")]
        public void GivenItHasAnAccountIn()
        {
            AccountIn = GetOrCreateAccount(AccountInName);

            var year = AccountIn[Move.Date.Year] ?? new Year();
            var month = year[Move.Date.Month] ?? new Month();

            AccountInTotal = AccountIn.Sum();
            YearCategoryAccountInTotal = (year[Category.Name] ?? new Summary()).In;
            MonthCategoryAccountInTotal = (month[Category.Name] ?? new Summary()).In;
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

        [Given(@"it has a closed Account In")]
        public void GivenItHasAClosedAccountIn()
        {
            AccountIn = new Account { Name = "closed in", User = User };
            SA.Admin.SaveOrUpdateAccount(AccountIn);

            var move = new Move
            {
                Date = DateTime.Now,
                Description = "Description",
                Nature = MoveNature.In
            };

            // TODO: Remove this, put Value
            var detail = new Detail { Amount = 1, Description = move.Description, Value = 10 };

            move.DetailList.Add(detail);

            SA.Money.SaveOrUpdateMove(move, null, AccountIn, Category);

            SA.Admin.CloseAccount(AccountIn.ID);
        }


        [Given(@"it has an Account In equal to Out")]
        public void GivenItHasAnAccountInEqualToOut()
        {
            AccountOut = GetOrCreateAccount(AccountOutName);
            AccountIn = AccountOut;

            var year = AccountIn[Move.Date.Year] ?? new Year();
            var month = year[Move.Date.Month] ?? new Month();

            AccountInTotal = AccountIn.Sum();
            YearCategoryAccountInTotal = (year[Category.Name] ?? new Summary()).In;
            MonthCategoryAccountInTotal = (month[Category.Name] ?? new Summary()).In;
        }



        [Then(@"the accountOut value will not change")]
        public void ThenTheAccountOutValueWillNotChange()
        {
            AccountOut = GetOrCreateAccount(AccountOut.Name);

            Assert.AreEqual(AccountOutTotal, AccountOut.Sum());
        }

        [Then(@"the month-category-accountOut value will not change")]
        public void ThenTheMonthCategoryAccountOutValueWillNotChange()
        {
            AccountOut = GetOrCreateAccount(AccountOut.Name);

            var year = AccountOut[Move.Date.Year] ?? new Year();
            var month = year[Move.Date.Month] ?? new Month();

            var currentTotal = (month[Category.Name] ?? new Summary()).Out;

            Assert.AreEqual(MonthCategoryAccountOutTotal, currentTotal);
        }

        [Then(@"the year-category-accountOut value will not change")]
        public void ThenTheYearCategoryAccountOutValueWillNotChange()
        {
            AccountOut = GetOrCreateAccount(AccountOut.Name);

            var year = AccountOut[Move.Date.Year] ?? new Year();

            var currentTotal = (year[Category.Name] ?? new Summary()).Out;

            Assert.AreEqual(YearCategoryAccountOutTotal, currentTotal);
        }


        [Then(@"the accountIn value will not change")]
        public void ThenTheAccountInValueWillNotChange()
        {
            AccountIn = GetOrCreateAccount(AccountIn.Name);

            Assert.AreEqual(AccountInTotal, AccountIn.Sum());
        }

        [Then(@"the month-category-accountIn value will not change")]
        public void ThenTheMonthCategoryAccountInValueWillNotChange()
        {
            AccountIn = GetOrCreateAccount(AccountIn.Name);

            var year = AccountIn[Move.Date.Year] ?? new Year();
            var month = year[Move.Date.Month] ?? new Month();

            var currentTotal = (month[Category.Name] ?? new Summary()).In;

            Assert.AreEqual(MonthCategoryAccountInTotal, currentTotal);
        }

        [Then(@"the year-category-accountIn value will not change")]
        public void ThenTheYearCategoryAccountInValueWillNotChange()
        {
            AccountIn = GetOrCreateAccount(AccountIn.Name);

            var year = AccountIn[Move.Date.Year] ?? new Year();

            var currentTotal = (year[Category.Name] ?? new Summary()).In;

            Assert.AreEqual(YearCategoryAccountInTotal, currentTotal);
        }

        
        [Then(@"the accountOut value will change in (\-?\d+)")]
        public void ThenTheAccountOutValueWillDecreaseIn(Double change)
        {
            AccountOut = GetOrCreateAccount(AccountOut.Name);

            var currentTotal = AccountOut.Sum();

            Assert.AreEqual(AccountOutTotal + change, currentTotal);
        }

        [Then(@"the month-category-accountOut value will change in (\-?\d+)")]
        public void ThenTheMonthCategoryAccountOutValueWillChangeIn(Double change)
        {
            AccountOut = GetOrCreateAccount(AccountOut.Name);

            var year = AccountOut[Move.Date.Year] ?? new Year();
            var month = year[Move.Date.Month] ?? new Month();

            var currentTotal = (month[Category.Name] ?? new Summary()).Out;

            Assert.AreEqual(MonthCategoryAccountOutTotal + change, currentTotal);
        }

        [Then(@"the year-category-accountOut value will change in (\-?\d+)")]
        public void ThenTheYearCategoryAccountOutValueWillChangeIn(Double change)
        {
            AccountOut = GetOrCreateAccount(AccountOut.Name);

            var year = AccountOut[Move.Date.Year] ?? new Year();

            var currentTotal = (year[Category.Name] ?? new Summary()).Out;

            Assert.AreEqual(YearCategoryAccountOutTotal + change, currentTotal);
        }


        [Then(@"the accountIn value will change in (\-?\d+)")]
        public void ThenTheAccountInValueWillIncreaseIn(Double change)
        {
            AccountIn = GetOrCreateAccount(AccountIn.Name);

            var currentTotal = AccountIn.Sum();

            Assert.AreEqual(AccountInTotal + change, currentTotal);
        }

        [Then(@"the month-category-accountIn value will change in (\-?\d+)")]
        public void ThenTheMonthCategoryAccountInValueWillChangeIn(Double change)
        {
            AccountIn = GetOrCreateAccount(AccountIn.Name);

            var year = AccountIn[Move.Date.Year] ?? new Year();
            var month = year[Move.Date.Month] ?? new Month();

            var currentTotal = (month[Category.Name] ?? new Summary()).In;

            Assert.AreEqual(MonthCategoryAccountInTotal + change, currentTotal);
        }

        [Then(@"the year-category-accountIn value will change in (\-?\d+)")]
        public void ThenTheYearCategoryAccountInValueWillChangeIn(Double change)
        {
            AccountIn = GetOrCreateAccount(AccountIn.Name);

            var year = AccountIn[Move.Date.Year] ?? new Year();

            var currentTotal = (year[Category.Name] ?? new Summary()).In;

            Assert.AreEqual(YearCategoryAccountInTotal + change, currentTotal);
        }








    }
}
