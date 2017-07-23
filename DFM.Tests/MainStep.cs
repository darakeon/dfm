using System;
using DFM.Repositories;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace DFM.Tests
{
    [Binding]
    public class MainStep : BaseStep
    {
        [Then(@"I will receive this error: ([A-Za-z]+)")]
        public void ThenIWillReceiveThisError(String error)
        {
            Assert.IsNotNull(Error);
            Assert.AreEqual(error, Error.Type.ToString());
        }

        [Then(@"I will receive no error")]
        public void ThenIWillReceiveNoError()
        {
            Assert.IsNull(Error);
        }

        [Given(@"I have an user")]
        public void GivenIHaveAnUser()
        {
            User = GetOrCreateUser(CentralUserEmail, CentralUserPassword);
        }

        [Given(@"I have an account")]
        public void GivenIHaveAnAccount()
        {
            Account = GetOrCreateAccount(CentralAccountName);
        }

        [Given(@"I have a category")]
        public void GivenIHaveACategory()
        {
            Category = GetOrCreateCategory(CentralCategoryName);
        }

        [AfterScenario]
        public static void CloseSession()
        {
            NHManager.Close();
        }

    }
}
