using System;
using DFM.Repositories;
using DFM.Tests.Helpers;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace DFM.Tests.BusinessLogic
{
    [Binding]
    public class MainStep : BaseStep
    {
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

        [Given(@"I have an active user")]
        public void GivenIHaveAnUser()
        {
            User = GetOrCreateUser(UserEmail, UserPassword, true);

            Current.Set(UserEmail, UserPassword);
        }

        [Given(@"I have an account")]
        public void GivenIHaveAnAccount()
        {
            Account = GetOrCreateAccount(AccountName);
        }

        [Given(@"I have a category")]
        public void GivenIHaveACategory()
        {
            Category = GetOrCreateCategory(CategoryName);
        }



        [AfterScenarioBlock]
        public static void CloseSession()
        {
            NHManager.Close();
        }

        [AfterScenario]
        public void Logoff()
        {
            Current.Clean();
        }



    }
}
