using System;
using System.Linq;
using DFM.Repositories;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace DFM.Tests.BusinessLogic
{
    [Binding]
    public class MainStep : BaseStep
    {
        [Given(@"I have an active user")]
        public void GivenIHaveAnActiveUser()
        {
            CreateUserIfNotExists(UserEmail, UserPassword, true);

            Current.Reset(UserEmail, UserPassword);
        }

        [Given(@"I have an account")]
        public void GivenIHaveAnAccount()
        {
            Account = GetOrCreateAccount(MainAccountUrl);
        }

        [Given(@"I have a category")]
        public void GivenIHaveACategory()
        {
            Category = GetOrCreateCategory(MainCategoryName);
        }

        [Given(@"I pass a valid account url")]
        public void GivenIPassValidAccountName()
        {
            AccountUrl = Account.Url;
        }

        
        [Then(@"I will receive this core error: ([A-Za-z]+)")]
        public void ThenIWillReceiveThisError(String error)
        {
            Assert.IsNotNull(Error);
            Assert.AreEqual(error, Error.Type.ToString());
        }

        [Then(@"I will receive no core error")]
        public void ThenIWillReceiveNoError()
        {
            Assert.IsNull(Error);
        }



        [AfterScenarioBlock]
        public static void CloseSession()
        {
            NHManager.Close();
        }

        [AfterScenario]
        public void CleanSchedulesAndLogoff()
        {
            if (!Current.IsAuthenticated)
                return;

            var pendentSchedules = Current.User.ScheduleList.Where(s => s.Active);

            foreach (var pendentSchedule in pendentSchedules)
            {
                SA.Robot.DisableSchedule(pendentSchedule.ID);
            }

            Current.Clean();
        }



    }
}
