using System;
using System.Collections.Generic;
using Ak.MVC.Cookies;
using DFM.BusinessLogic.Exceptions;
using DFM.BusinessLogic.Helpers;
using DFM.Entities;
using DFM.Entities.Enums;
using DFM.Generic;
using DFM.Tests.BusinessLogic.Helpers;
using NUnit.Framework;
using TechTalk.SpecFlow;
using TK = Ak.Generic.Extensions.Token;

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

        private static String ticket
        {
            get { return Get<String>("ticket"); }
            set { Set("ticket", value); }
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

        private static IList<Ticket> logins
        {
            get { return Get<IList<Ticket>>("logins"); }
            set { Set("logins", value); }
        }
        #endregion


        #region SaveUserAndSendVerify
        [Given(@"I have this user data")]
        public void GivenIHaveThisUserData(Table table)
        {
            email = table.Rows[0]["Email"];
            password = table.Rows[0]["Password"];
        }

        [Given(@"I already have created this user")]
        public void GivenIAlreadyHaveCreatedThisUser()
        {
            otherUser = new User
            {
                Email = email,
                Password = password + "_diff",
            };

            SA.Safe.SaveUserAndSendVerify(otherUser.Email, otherUser.Password, Defaults.ConfigLanguage);

            var tokenActivate = DBHelper.GetLastTokenForUser(otherUser.Email, otherUser.Password, SecurityAction.UserVerification);

            SA.Safe.ActivateUser(tokenActivate);
        }



        [When(@"I try to save the user")]
        public void WhenITryToSaveTheUser()
        {
            try
            {
                SA.Safe.SaveUserAndSendVerify(email, password, Defaults.ConfigLanguage);
            }
            catch (DFMCoreException e)
            {
                Error = e;
            }
        }
        
        [Then(@"the user will not be saved")]
        public void ThenTheUserWillNotBeSaved()
        {
            ticket = null;
            Error = null;

            try
            {
                ticket = SA.Safe.ValidateUserAndCreateTicket(email, password, Current.Ticket);
            }
            catch (DFMCoreException e)
            {
                Error = e;
            }

            Assert.IsNull(ticket);
            Assert.IsNotNull(Error);
            Assert.AreEqual(ExceptionPossibilities.InvalidUser, Error.Type);
        }

        [Then(@"the user will not be changed")]
        public void ThenTheUserWillNotBeChanged()
        {
            var savedUser = GetSavedUser(otherUser.Email, otherUser.Password);

            Assert.IsNotNull(savedUser);
        }

        [Then(@"the user will be saved")]
        public void ThenTheUserWillBeSaved()
        {
            var tokenActivate = DBHelper.GetLastTokenForUser(email, password, SecurityAction.UserVerification);
            
            SA.Safe.ActivateUser(tokenActivate);

            var savedUser = GetSavedUser(email, password);

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

            token = DBHelper.GetLastTokenForUser(User.Email, User.Password, SecurityAction.PasswordReset);
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
            Error = null;

            try
            {
                SA.Safe.ValidateUserAndCreateTicket(email, password, Current.Ticket);
            }
            catch (DFMCoreException e)
            {
                Error = e;                
            }

            Assert.IsNotNull(Error);
            Assert.AreEqual(ExceptionPossibilities.DisabledUser, Error.Type);
        }

        [Then(@"the user will be activated")]
        public void ThenTheUserWillBeActivated()
        {
            var ticketKey = SA.Safe.ValidateUserAndCreateTicket(email, password, Current.Ticket);
            
            User = SA.Safe.GetUserByTicket(ticketKey);

            Assert.IsTrue(User.Active);
        }
        #endregion

        #region ValidateUserAndGetTicket
        [Given(@"I activate the user")]
        public void GivenIActivateTheUser()
        {
            SA.Safe.SendUserVerify(email);

            var tokenToActivate = DBHelper.GetLastTokenForUser(email, password, SecurityAction.UserVerification);

            SA.Safe.ActivateUser(tokenToActivate);
        }

        [When(@"I try to get the ticket")]
        public void WhenITryToGetTheTicket()
        {
            ticket = null;
            Error = null;

            try
            {
                ticket = SA.Safe.ValidateUserAndCreateTicket(email, password, MyCookie.Get());
            }
            catch (DFMCoreException e)
            {
                Error = e;
            }
        }

        [Given(@"I tried to get the ticket (\d+) times")]
        [When(@"I try to get the ticket (\d+) times")]
        public void WhenITryToGetTheTicketSomeTimes(Int32 times)
        {
            ticket = null;

            for (var t = 1; t < times; t++)
            {
                try { SA.Safe.ValidateUserAndCreateTicket(email, password, MyCookie.Get()); }
                catch (DFMCoreException) { }
            }

            try
            {
                SA.Safe.ValidateUserAndCreateTicket(email, password, MyCookie.Get());
            }
            catch (DFMCoreException e)
            {
                Error = e;
            }
        }

        [Then(@"I will receive no ticket")]
        public void ThenIWillReceiveNoTicket()
        {
            Assert.IsNull(ticket);
        }

        [Then(@"I will receive the ticket")]
        public void ThenIWillReceiveTheTicket()
        {
            Assert.IsNotNull(ticket);

            var user = SA.Safe.GetUserByTicket(ticket);

            Assert.AreEqual(email, user.Email);
        }
        #endregion
        
        #region GetUserByTicket
        [When(@"I try to get the user")]
        public void WhenITryToGetTheUser()
        {
            User = null;

            try
            {
                User = SA.Safe.GetUserByTicket(ticket);
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
            token = DBHelper.GetLastTokenForUser(User.Email, User.Password, SecurityAction.UserVerification);
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
            Error = null;

            try
            {
                SA.Safe.ValidateUserAndCreateTicket(email, password, Current.Ticket);
            }
            catch (DFMCoreException e)
            {
                if (e.Type != ExceptionPossibilities.DisabledUser)
                    Error = e;
            }

            Assert.IsNull(Error);
        }

        [Then(@"the password will be changed")]
        public void ThenThePasswordWillBeChanged()
        {
            Error = null;

            try
            {
                SA.Safe.ValidateUserAndCreateTicket(email, newPassword, Current.Ticket);
            }
            catch (DFMCoreException e)
            {
                if (e.Type != ExceptionPossibilities.DisabledUser)
                    Error = e;
            }

            Assert.IsNull(Error);
        }
        #endregion

        #region TestSecurityToken
        [Given(@"I pass a token of ([A-Za-z]+) with action ([A-Za-z]+)")]
        public void GivenIPassATokenOfUserVerificationWithActionPasswordReset(String strTokenOf, String strAction)
        {
            var tokenOf = EnumX.Parse<SecurityAction>(strTokenOf);
            token = DBHelper.GetLastTokenForUser(email, password, tokenOf);

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
        
        #region DisableToken
        [When(@"I try do disable the token")]
        public void WhenITryDoDisableTheToken()
        {
            try
            {
                SA.Safe.DisableToken(token);
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

        #region Disable Ticket
        [When(@"I try to disable the ticket")]
        public void WhenITryToDisableTheTicket()
        {
            try
            {
                SA.Safe.DisableTicket(ticket);
            }
            catch (DFMCoreException e)
            {
                Error = e;
            }
        }

        [Then(@"the ticket will not be valid anymore")]
        public void ThenTheTicketWillNotBeValidAnymore()
        {
            Error = null;

            try
            {
                SA.Safe.GetUserByTicket(ticket);
            }
            catch (DFMCoreException e)
            {
                Error = e;
            }

            Assert.IsNotNull(Error);
            Assert.AreEqual(ExceptionPossibilities.Uninvited, Error.Type);
        }
        #endregion

        #region ListLogins
        [Given(@"I login the user")]
        public void GivenILoginTheUser()
        {
            Current.Set(UserEmail, UserPassword);
        }

        [Given(@"I logoff the user")]
        public void GivenILogoffTheUser()
        {
            Current.Clean();
        }

        [When(@"I ask for current active logins")]
        public void WhenIAskForCurrentActiveLogins()
        {
            try
            {
                logins = SA.Safe.ListLogins();
            }
            catch (DFMCoreException e)
            {
                Error = e;
            }
        }

        [Then(@"they will be active")]
        public void ThenTheyWillBeActive()
        {
            foreach (var login in logins)
            {
                Assert.IsTrue(login.Active);
            }
        }
        
        [Then(@"they will not have sensible information")]
        public void ThenTheyWillNotHaveSensibleInformation()
        {
            foreach (var login in logins)
            {
                Assert.AreEqual(Defaults.TicketShowedPart, login.Key.Length);
                Assert.AreEqual(0, login.ID);
                Assert.IsNull(login.User);
            }
        }
        #endregion



        #region MoreThanOne
        [Given(@"I have this user created")]
        public void GivenIHaveThisUserToCreate(Table table)
        {
            email = table.Rows[0]["Email"];
            password = table.Rows[0]["Password"];

            CreateUserIfNotExists(email, password);
        }

        [Given(@"I have this user created and activated")]
        public void GivenIHaveThisUserToCreateAndActivate(Table table)
        {
            email = table.Rows[0]["Email"];
            password = table.Rows[0]["Password"];

            CreateUserIfNotExists(email, password, true);
        }

        [Given(@"I have a token for its activation")]
        public void GivenIHaveATokenForItsActivation()
        {
            SA.Safe.SendUserVerify(email);
        }

        [Given(@"I have a token for its password reset")]
        public void GivenIHaveATokenForItsPasswordReset()
        {
            SA.Safe.SendPasswordReset(email);
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

        [Given(@"I pass a ticket that doesn't exist")]
        public void GivenIPassATicketThatDoesnTExist()
        {
            ticket = TK.New();
        }

        [Given(@"I pass a ticket that is already disabled")]
        public void GivenIPassATicketThatIsAlreadyInvalid()
        {
            SA.Safe.DisableTicket(ticket);
        }

        [Given(@"I pass a ticket that is of this disabled user")]
        public void GivenIPassATicketThatIsOfThisDisabledUser()
        {
            ticket = SA.Safe.ValidateUserAndCreateTicket(email, password, Current.Ticket);
        }

        [Given(@"I pass a ticket that exist")]
        public void GivenIPassATicketThatExist()
        {
            ticket = DBHelper.GetLastTicketForUser(email, password);
        }

        [Given(@"I pass a valid ([A-Za-z]+) token")]
        public void GivenIPassTheValidToken(String strAction)
        {
            action = EnumX.Parse<SecurityAction>(strAction);
            token = DBHelper.GetLastTokenForUser(email, password, action);
        }

        [Given(@"I have a ticket of this user")]
        public void GivenIHaveATicketOfThisUser()
        {
            ticket = SA.Safe.ValidateUserAndCreateTicket(email, password, MyCookie.Get());
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
