using System;
using DFM.BusinessLogic.Exceptions;
using DFM.Entities;
using DFM.Entities.Enums;
using DFM.Tests.Helpers;
using NUnit.Framework;
using TechTalk.SpecFlow;
using TK = DFM.Generic.Token;

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

        protected String Token
        {
            get { return Get<String>("Token"); }
            set { Set("Token", value); }
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

        #region SendUserVerify
        /* Use same tests of others */
        #endregion

        #region SendPasswordReset
        [When(@"I try to send the e-mail of password reset")]
        public void WhenITryToSendTheEMailOfPasswordReset()
        {
            try
            {
                Access.Safe.SendPasswordReset(Email);
            }
            catch (DFMCoreException e)
            {
                Error = e;
            }
        }
        #endregion
        
        #region ActivateUser
        [Given(@"I pass a token of password reset")]
        public void GivenIPassATokenOfPasswordReset()
        {
            Access.Safe.SendPasswordReset(User.Email);

            Token = DBHelper.GetLastTokenForUser(User, SecurityAction.PasswordReset);
        }
        
        [When(@"I try to activate the user")]
        public void WhenITryToActivateTheUser()
        {
            try
            {
                Access.Safe.ActivateUser(Token);
            }
            catch (DFMCoreException e)
            {
                Error = e;
            }
        }

        [Then(@"the user will not be activated")]
        public void ThenTheUserWillNotBeActivated()
        {
            User = Access.Safe.ValidateAndGet(CentralUserEmail, CentralUserPassword);

            Assert.IsFalse(User.Active);
        }

        [Then(@"the user will be activated")]
        public void ThenTheUserWillBeActivated()
        {
            User = Access.Safe.ValidateAndGet(CentralUserEmail, CentralUserPassword);

            Assert.IsTrue(User.Active);
        }
        #endregion

        #region ValidateAndGetUser
        [Given(@"I have this user data")]
        public void GivenIHaveThisUserData(Table table)
        {
            Email = table.Rows[0]["Email"];
            Password = table.Rows[0]["Password"];
        }

        [When(@"I try to get the user")]
        public void WhenITryToGetTheUser()
        {
            try
            {
                User = null;
                User = Access.Safe.ValidateAndGet(Email, Password);
            }
            catch (DFMCoreException e)
            {
                Error = e;
            }
        }
        #endregion
        
        #region SelectUserByEmail
        [When(@"I try to get the user without password")]
        public void WhenITryToGetTheUserWithoutPassword()
        {
            try
            {
                User = null;
                User = Access.Safe.SelectUserByEmail(Email);
            }
            catch (DFMCoreException e)
            {
                Error = e;
            }
        }
        #endregion

        #region PasswordReset
        [Given(@"I pass a token of user verification")]
        public void GivenIPassATokenOfUserVerification()
        {
            ScenarioContext.Current.Pending();
        }

        [Given(@"I pass no password")]
        public void GivenIPassNoPassword()
        {
            ScenarioContext.Current.Pending();
        }
        

        [When(@"I try to reset the password")]
        public void WhenITryToResetThePassword()
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"the password will not be changed")]
        public void ThenThePasswordWillNotBeChanged()
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"the password will be changed")]
        public void ThenThePasswordWillBeChanged()
        {
            ScenarioContext.Current.Pending();
        }
        #endregion

        #region TestSecurityToken
        [Given(@"I pass a token of user verification with action password reset")]
        public void GivenIPassATokenOfUserVerificationWithActionPasswordReset()
        {
            ScenarioContext.Current.Pending();
        }

        [Given(@"I pass a token of password reset with action user verification")]
        public void GivenIPassATokenOfPasswordResetWithActionUserVerification()
        {
            ScenarioContext.Current.Pending();
        }

        [Given(@"I pass a token of user verification with right action")]
        public void GivenIPassATokenOfUserVerificationWithRightAction()
        {
            ScenarioContext.Current.Pending();
        }

        [Given(@"I pass a token of password reset with right action")]
        public void GivenIPassATokenOfPasswordResetWithRightAction()
        {
            ScenarioContext.Current.Pending();
        }

        [When(@"I test the token")]
        public void WhenITestTheToken()
        {
            ScenarioContext.Current.Pending();
        }
        #endregion
        
        #region DeactivateToken
        [Given(@"I pass a valid token")]
        public void GivenIPassAValidToken()
        {
            ScenarioContext.Current.Pending();
        }

        [When(@"I try do deactivate the token")]
        public void WhenITryDoDeactivateTheToken()
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"the token will not be valid anymore")]
        public void ThenTheTokenWillNotBeValidAnymore()
        {
            ScenarioContext.Current.Pending();
        }
        #endregion


        #region MoreThanOne
        [Given(@"I have a token for its password reset")]
        public void GivenIHaveATokenForItsPasswordReset()
        {
            ScenarioContext.Current.Pending();
        }

        [Given(@"I pass an invalid token")]
        public void GivenIPassAnInvalidToken()
        {
            Token = TK.New();
        }

        [Given(@"I pass an e-mail that doesn't exist")]
        public void GivenIPassAnEMailThatDoesnTExist()
        {
            Email = "dont_exist@dontflymoney.com";
        }

        [Given(@"I pass valid e-mail")]
        public void GivenIPassValidEMail()
        {
            Email = User.Email;
        }

        [Given(@"I pass the valid token")]
        public void GivenIPassTheValidToken()
        {
            ScenarioContext.Current.Pending();
        }

        [Given(@"I have a token for its actvation")]
        public void GivenIHaveATokenForItsActvation()
        {
            Access.Safe.SendUserVerify(User.Email);
        }

        
        [When(@"I try to send the e-mail of user verify")]
        public void WhenITryToSendTheEMailOfUserVerify()
        {
            ScenarioContext.Current.Pending();
        }


        [Then(@"I will receive no user")]
        public void ThenIWillReceiveNoUser()
        {
            Assert.IsNull(User);
        }

        [Then(@"I will receive the user")]
        public void ThenIWillReceiveTheUser()
        {
            Assert.IsNotNull(User);
            Assert.AreEqual(Email, User.Email);
        }
        #endregion


    }
}
