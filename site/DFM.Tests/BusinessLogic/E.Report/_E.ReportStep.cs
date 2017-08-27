using System;
using System.Collections.Generic;
using System.Linq;
using DFM.BusinessLogic.Exceptions;
using DFM.Entities;
using DFM.Entities.Enums;
using DFM.Entities.Extensions;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace DFM.Tests.BusinessLogic.E.Report
{
    [Binding]
    public class ReportStep : BaseStep
    {
        #region Variables

        private static Int16 month
        {
            get { return Get<Int16>("Month"); }
            set { Set("Month", value); }
        }

        private static Int16 year
        {
            get { return Get<Int16>("Year"); }
            set { Set("Year", value); }
        }

        private static IList<Move> monthReport
        {
            get { return Get<IList<Move>>("MonthReport"); }
            set { Set("MonthReport", value); }
        }

        private static Year yearReport
        {
            get { return Get<Year>("YearReport"); }
            set { Set("YearReport", value); }
        }
        #endregion

        #region GetMonthReport
        [When(@"I try to get the month report")]
        public void WhenITryToGetTheMonthReport()
        {
            try
            {
                monthReport = SA.Report.GetMonthReport(AccountUrl, month, year);
            }
            catch (DFMCoreException e)
            {
                Error = e;
            }
        }

        [Then(@"I will receive no month report")]
        public void ThenIWillReceiveNoMonthReport()
        {
            Assert.IsNull(monthReport);
        }

        [Then(@"I will receive the month report")]
        public void ThenIWillReceiveTheMonthReport()
        {
            Assert.IsNotNull(monthReport);
        }

        [Then(@"its sum value will be equal to its moves sum value")]
        public void ThenItsSumValueWillBeEqualToItsMovesSumValue()
        {
            var expected = Account[year][month]
                .SummaryList.Sum(s => s.Value());
            
            var actual = monthReport.Sum(m => 
                    m.AccOut() != null 
                            && m.AccOut().ID == Account.ID
                        ? - m.Value()
                        : m.Value()
                );

            Assert.AreEqual(expected, actual);
        }
        #endregion

        #region GetYearReport
        [When(@"I try to get the year report")]
        public void WhenITryToGetTheYearReport()
        {
            try
            {
                yearReport = SA.Report.GetYearReport(AccountUrl, year);
            }
            catch (DFMCoreException e)
            {
                Error = e;
            }
        }

        [Then(@"I will receive no year report")]
        public void ThenIWillReceiveNoYearReport()
        {
            Assert.IsNull(yearReport);
        }

        [Then(@"I will receive the year report")]
        public void ThenIWillReceiveTheYearReport()
        {
            Assert.IsNotNull(yearReport);
        }

        [Then(@"its sum value will be equal to its months sum value")]
        public void ThenItsSumValueWillBeEqualToItsMonthsSumValue()
        {
            var expected = Account[year].SummaryList.Sum(s => s.Value());

            var actual = yearReport.MonthList.Sum(m =>
                    m.SummaryList.Sum(s => s.Value())
                );

            Assert.AreEqual(expected, actual);
        }
        #endregion



        #region MoreThanOne
        [Given(@"I have moves of")]
        public void GivenIHaveMovesOf(Table table)
        {
            Category = GetOrCreateCategory(MainCategoryName);

            foreach (var row in table.Rows)
            {
                var date = DateTime.Parse(row["Date"]);

                var move = new Move
                {
                    Description = "Description",
                    Date = date,
                    Nature = MoveNature.Out,
                };


                // TODO: use this, delete above
                //    move.Value = 10;
                var detail = new Detail
                {
                    Description = move.Description,
                    Amount = 1,
                    Value = 10,
                };

                move.DetailList.Add(detail);

                SA.Money.SaveOrUpdateMove(move, Account.Url, null, Category.Name);
            }
        }

        [Given(@"I pass an invalid account url")]
        public void GivenIPassAnInvalidAccountName()
        {
            AccountUrl = "invalid";
        }

        [Given(@"I pass this date")]
        public void GivenIPassThisDate(Table table)
        {
            var dateData = table.Rows[0];

            if (table.Header.Contains("Month"))
                month = Int16.Parse(dateData["Month"]);

            year = Int16.Parse(dateData["Year"]);
        }

        #endregion


    }
}
