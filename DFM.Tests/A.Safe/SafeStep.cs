using System;
using DFM.BusinessLogic.Exceptions;
using DFM.Entities;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace DFM.Tests.A.Safe
{
    [Binding]
    public class SafeStep : BaseStep
    {
        #region Variables
        protected User OtherUser
        {
            get { return Get<User>("OtherUser"); }
            set { Set("OtherUser", value); }
        }
        
        protected String Email
        {
            get { return Get<String>("Email"); }
            set { Set("Email", value); }
        }

        protected String Password
        {
            get { return Get<String>("Password"); }
            set { Set("Password", value); }
        }
        #endregion

        #region SaveUserAndSendVerify
        [Given(@"I have this user to create")]
        public void GivenIHaveThisUserToCreate(Table table)
        {
            Email = table.Rows[0]["Email"];
            Password = table.Rows[0]["Password"];
        }

        [When(@"I try to save the user")]
        public void WhenITryToSaveTheUser()
        {
            try
            {
                Access.Safe.SaveUserAndSendVerify(Email, Password);
            }
            catch (DFMCoreException e)
            {
                Error = e;
            }
        }

        [Given(@"I already have created this user")]
        public void GivenIAlreadyHaveCreatedThisUser()
        {
            OtherUser = new User
                                {
                                    Email = Email, 
                                    Password = Password + "_diff",
                                };

            Access.Safe.SaveUserAndSendVerify(OtherUser.Email, OtherUser.Password);
        }

        [Then(@"the user will not be saved")]
        public void ThenTheUserWillNotBeSaved()
        {
            var savedUser = Access.Safe.SelectUserByEmail(Email);

            Assert.IsNull(savedUser);
        }

        [Then(@"the user will not be changed")]
        public void ThenTheUserWillNotBeChanged()
        {
            var savedUser = Access.Safe.ValidateAndGet(OtherUser.Email, OtherUser.Password);

            Assert.IsNotNull(savedUser);
        }

        [Then(@"the user will be saved")]
        public void ThenTheUserWillBeSaved()
        {
            var savedUser = Access.Safe.ValidateAndGet(Email, Password);

            Assert.IsNotNull(savedUser);
        }
        #endregion



    }
}
