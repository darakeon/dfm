using DFM.BusinessLogic.Exceptions;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace DFM.Tests
{
    [Binding]
    public class MainStep : BaseStep
    {
        [Then(@"I will receive this error")]
        public void ThenIWillReceiveThisError(Table table)
        {
            var error = table.Rows[0]["Error"];

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
            try
            {
                User = Access.Safe.ValidateAndGet(CentralUserEmail, CentralUserPassword);
            }
            catch (DFMCoreException e)
            {
                if (e.Type != ExceptionPossibilities.InvalidUser)
                    throw;

                Access.Safe.SaveUserAndSendVerify(CentralUserEmail, CentralUserPassword);
                User = Access.Safe.ValidateAndGet(CentralUserEmail, CentralUserPassword);
            }
        }

        [Given(@"I have an account")]
        public void GivenIHaveAnAccount()
        {
            ScenarioContext.Current.Pending();
        }

        [Given(@"I have a category")]
        public void GivenIHaveACategory()
        {
            ScenarioContext.Current.Pending();
        }


    }
}
