using System;
using DFM.BusinessLogic.Exceptions;
using DFM.BusinessLogic.Services;
using DFM.Entities;
using DFM.Entities.Enums;

namespace DFM.BusinessLogic.SuperServices
{
    public class SafeService
    {
        private readonly UserService userService;
        private readonly SecurityService securityService;

        internal SafeService(UserService userService, SecurityService securityService)
        {
            this.userService = userService;
            this.securityService = securityService;
        }



        public void SendPasswordReset(String email)
        {
            var user = userService.SelectByEmail(email);

            if (user == null)
                throw DFMCoreException.WithMessage(ExceptionPossibilities.WrongUserEmail);

            sendPasswordReset(user);
        }

        private void sendPasswordReset(User user)
        {
            createAndSendToken(user, SecurityAction.PasswordReset);
        }
      
        
        public User SaveUserAndSendVerify(User user)
        {
            var transaction = userService.BeginTransaction();

            try
            {
                user = userService.SaveOrUpdate(user);

                sendUserVerify(user);

                userService.CommitTransaction(transaction);

                return user;
            }
            catch
            {
                userService.RollbackTransaction(transaction);
                throw;
            }
        }

        public void SendUserVerify(String email)
        {
            var user = SelectUserByEmail(email);

            if (user == null)
                throw DFMCoreException.WithMessage(ExceptionPossibilities.InvalidUser);

            sendUserVerify(user);
        }

        private void sendUserVerify(User user)
        {
            createAndSendToken(user, SecurityAction.UserVerification);
        }


        private void createAndSendToken(User user, SecurityAction action)
        {
            var security = new Security { Action = action, User = user };

            userService.ValidateSecurity(security);
            security = securityService.SaveOrUpdate(security);

            securityService.SendEmail(security);
        }




        public void ActivateUser(String token)
        {
            var security = securityService.SelectByToken(token);

            if (security.Action != SecurityAction.UserVerification)
                throw DFMCoreException.WithMessage(ExceptionPossibilities.InvalidToken);

            userService.Activate(security.User);

            securityService.Deactivate(token);
        }


        public void PasswordReset(String token, String password)
        {
            var security = securityService.SelectByToken(token);

            if (security.Action != SecurityAction.PasswordReset)
                throw DFMCoreException.WithMessage(ExceptionPossibilities.InvalidToken);

            security.User.Password = password;

            userService.ChangePassword(security.User);

            securityService.Deactivate(token);
        }


        public void TestSecurityToken(String token, SecurityAction securityAction)
        {
            securityService.TestSecurityToken(token, securityAction);
        }


        public User SelectUserByEmail(String email)
        {
            return userService.SelectByEmail(email);
        }

        public void DeactivateToken(String token)
        {
            securityService.Deactivate(token);
        }

        public User ValidateAndGet(String email, String password)
        {
            return userService.ValidateAndGet(email, password);
        }






    }
}
