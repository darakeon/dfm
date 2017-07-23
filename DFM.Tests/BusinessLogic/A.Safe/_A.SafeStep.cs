using System;
using DFM.BusinessLogic.Exceptions;
using DFM.Entities;
using DFM.Entities.Enums;
using DFM.Generic;
using DFM.Tests.BusinessLogic.Helpers;
using NUnit.Framework;
using TechTalk.SpecFlow;
using TK = DFM.Generic.Token;

namespace DFM.Tests.BusinessLogic.A.Safe
{
    [Binding]
    public class SafeStep : BaseStep
    {
        #region Variables
        private static User otherUser
        {
            get { return Get<User>("OtherUser"); }
            set { Set("OtherUser", value); }
        }

        private static String email
        {
            get { return Get<String>("Email"); }
            set { Set("Email", value); }
        }

        private static String password
        {
            get { return Get<String>("Password"); }
            set { Set("Password", value); }
        }

        private static String newPassword
        {
            get { return Get<String>("NewPassword"); }
            set { Set("NewPassword", value); }
        }

        private static String token
        {
            get { return Get<String>("Token"); }
            set { Set("Token", value); }
        }

        private static SecurityAction action
        {
            get { return Get<SecurityAction>("Action"); }
            set { Set("Action", value); }
        }
        #endregion


        #region SaveUserAndSendVerify
        [Given(@"I have this user to create")]
        public void GivenIHaveThisUserToCreate(Table table)
        {
            email = table.Rows[0]["Email"];
            password = table.Rows[0]["Password"];
        }

        [When(@"I try to save the user")]
        public void WhenITryToSaveTheUser()
        {
            try
            {
                SA.Safe.SaveUserAndSendVerify(email, password);
            }
            catch (DFMCoreException e)
            {
                Error = e;
            }
        }

        [Given(@"I already have created this user")]
        public void GivenIAlreadyHaveCreatedThisUser()
        {
            otherUser = new User
                                {
                                    Email = email, 
                                    Password = password + "_diff",
                                };

            SA.Safe.SaveUserAndSendVerify(otherUser.Email, otherUser.Password);
        }

        [Then(@"the user will not be saved")]
        public void ThenTheUserWillNotBeSaved()
        {
            User savedUser = null;

            try
            {
                savedUser = SA.Safe.ValidateAndGet(email, password);
            }
            catch (DFMCoreException e)
            {
                Assert.AreEqual(ExceptionPossibilities.InvalidUser, e.Type);
            }

            Assert.IsNull(savedUser);
        }

        [Then(@"the user will not be changed")]
        public void ThenTheUserWillNotBeChanged()
        {
            var savedUser = SA.Safe.ValidateAndGet(otherUser.Email, otherUser.Password);

            Assert.IsNotNull(savedUser);
        }

        [Then(@"the user will be saved")]
        public void ThenTheUserWillBeSaved()
        {
            var savedUser = SA.Safe.ValidateAndGet(email, password);

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
                SA.Safe.SendPasswordReset(email);
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
            SA.Safe.SendPasswordReset(User.Email);

            token = DBHelper.GetLastTokenForUser(User, SecurityAction.PasswordReset);
        }
        
        [When(@"I try to activate the user")]
        public void WhenITryToActivateTheUser()
        {
            try
            {
                SA.Safe.ActivateUser(token);
            }
            catch (DFMCoreException e)
            {
                Error = e;
            }
        }

        [Then(@"the user will not be activated")]
        public void ThenTheUserWillNotBeActivated()
        {
            User = SA.Safe.ValidateAndGet(UserEmail, UserPassword);

            Assert.IsFalse(User.Active);
        }

        [Then(@"the user will be activated")]
        public void ThenTheUserWillBeActivated()
        {
            User = SA.Safe.ValidateAndGet(UserEmail, UserPassword);

            Assert.IsTrue(User.Active);
        }
        #endregion

        #region ValidateAndGetUser
        [Given(@"I have this user data")]
        public void GivenIHaveThisUserData(Table table)
        {
            email = table.Rows[0]["Email"];
            password = table.Rows[0]["Password"];
        }

        [When(@"I try to get the user")]
        public void WhenITryToGetTheUser()
        {
            User = null;

            try
            {
                User = SA.Safe.ValidateAndGet(email, password);
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
            User = null;

            try
            {
                User = SA.Safe.SelectUserByEmail(email);
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
            token = DBHelper.GetLastTokenForUser(User, SecurityAction.UserVerification);
        }

        [Given(@"I pass this password: (.*)")]
        public void GivenIPassThisPassword(String passedPassword)
        {
            newPassword = passedPassword;
        }

        [Given(@"I pass no password")]
        public void GivenIPassNoPassword()
        {
            newPassword = null;
        }
        

        [When(@"I try to reset the password")]
        public void WhenITryToResetThePassword()
        {
            try
            {
                SA.Safe.PasswordReset(token, newPassword);
            }
            catch (DFMCoreException e)
            {
                Error = e;
            }
        }

        [Then(@"the password will not be changed")]
        public void ThenThePasswordWillNotBeChanged()
        {
            User = null;
            Error = null;

            try
            {
                User = SA.Safe.ValidateAndGet(UserEmail, UserPassword);
            }
            catch (DFMCoreException e)
            {
                Error = e;
            }

            Assert.IsNull(Error);
            Assert.IsNotNull(User);
        }

        [Then(@"the password will be changed")]
        public void ThenThePasswordWillBeChanged()
        {
            User = null;
            Error = null;

            UserPassword = newPassword;

            try
            {
                User = SA.Safe.ValidateAndGet(UserEmail, UserPassword);
            }
            catch (DFMCoreException e)
            {
                Error = e;
            }

            Assert.IsNull(Error);
            Assert.IsNotNull(User);
        }
        #endregion

        #region TestSecurityToken
        [Given(@"I pass a token of ([A-Za-z]+) with action ([A-Za-z]+)")]
        public void GivenIPassATokenOfUserVerificationWithActionPasswordReset(String strTokenOf, String strAction)
        {
            var tokenOf = EnumX.Parse<SecurityAction>(strTokenOf);
            token = DBHelper.GetLastTokenForUser(User, tokenOf);

            action = EnumX.Parse<SecurityAction>(strAction);
        }

        [When(@"I test the token")]
        public void WhenITestTheToken()
        {
            try
            {
                SA.Safe.TestSecurityToken(token, action);
            }
            catch (DFMCoreException e)
            {
                Error = e;
            }
        }
        #endregion
        
        #region DeactivateToken
        [When(@"I try do deactivate the token")]
        public void WhenITryDoDeactivateTheToken()
        {
            try
            {
                SA.Safe.DeactivateToken(token);
            }
            catch (DFMCoreException e)
            {
                Error = e;
            }
        }

        [Then(@"the token will not be valid anymore")]
        public void ThenTheTokenWillNotBeValidAnymore()
        {
            try
            {
                SA.Safe.TestSecurityToken(token, action);
            }
            catch(DFMCoreException e)
            {
                Error = e;
            }

            Assert.IsNotNull(Error);
            Assert.AreEqual(ExceptionPossibilities.InvalidToken, Error.Type);
        }
        #endregion


        #region MoreThanOne
        [Given(@"I have a token for its password reset")]
        public void GivenIHaveATokenForItsPasswordReset()
        {
            SA.Safe.SendPasswordReset(User.Email);
        }

        [Given(@"I pass an invalid token")]
        public void GivenIPassAnInvalidToken()
        {
            token = TK.New();
        }

        [Given(@"I pass an e-mail that doesn't exist")]
        public void GivenIPassAnEMailThatDoesnTExist()
        {
            email = "dont_exist@dontflymoney.com";
        }

        [Given(@"I pass valid e-mail")]
        public void GivenIPassValidEMail()
        {
            email = User.Email;
        }

        [Given(@"I pass a valid ([A-Za-z]+) token")]
        public void GivenIPassTheValidToken(String strAction)
        {
            action = EnumX.Parse<SecurityAction>(strAction);
            token = DBHelper.GetLastTokenForUser(User, action);
        }

        [Given(@"I have a token for its actvation")]
        public void GivenIHaveATokenForItsActvation()
        {
            SA.Safe.SendUserVerify(User.Email);
        }

        
        [When(@"I try to send the e-mail of user verify")]
        public void WhenITryToSendTheEMailOfUserVerify()
        {
            try
            {
                SA.Safe.SendUserVerify(email);
            }
            catch (DFMCoreException e)
            {
                Error = e;
            }
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
            Assert.AreEqual(email, User.Email);
        }
        #endregion


    }
}
