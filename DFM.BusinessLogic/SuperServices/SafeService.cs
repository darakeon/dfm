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
                throw DFMCoreException.WithMessage(ExceptionPossibilities.InvalidUser);

            sendPasswordReset(user);
        }

        private void sendPasswordReset(User user)
        {
            createAndSendToken(user, SecurityAction.PasswordReset);
        }
      
        
        public void SaveUserAndSendVerify(String email, String password)
        {
            userService.BeginTransaction();

            try
            {
                var user = new User { Email = email, Password = password };

                user = userService.Save(user);

                sendUserVerify(user);

                userService.CommitTransaction();
            }
            catch
            {
                userService.RollbackTransaction();
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
            securityService.BeginTransaction();

            try
            {
                var security = securityService.ValidateAndGet(token, SecurityAction.UserVerification);

                userService.Activate(security.User);

                securityService.Deactivate(token);

                securityService.CommitTransaction();
            }
            catch (Exception)
            {
                securityService.RollbackTransaction();
                throw;
            }

        }


        public void PasswordReset(String token, String password)
        {
            if (String.IsNullOrEmpty(password))
                throw DFMCoreException.WithMessage(ExceptionPossibilities.UserPasswordRequired);

            securityService.BeginTransaction();

            try
            {
                var security = securityService.ValidateAndGet(token, SecurityAction.PasswordReset);

                security.User.Password = password;

                userService.ChangePassword(security.User);

                securityService.Deactivate(token);

                securityService.CommitTransaction();
            }
            catch (DFMCoreException)
            {
                securityService.RollbackTransaction();
                throw;
            }
        }


        public void TestSecurityToken(String token, SecurityAction securityAction)
        {
            securityService.ValidateAndGet(token, securityAction);
        }


        public User SelectUserByEmail(String email)
        {
            var user = userService.SelectByEmail(email);

            if (user == null)
                throw DFMCoreException.WithMessage(ExceptionPossibilities.InvalidUser);

            return user;
        }

        public void DeactivateToken(String token)
        {
            securityService.BeginTransaction();

            try
            {
                securityService.Deactivate(token);

                securityService.CommitTransaction();
            }
            catch (Exception)
            {
                securityService.RollbackTransaction();
                throw;
            }
        }

        public User ValidateAndGet(String email, String password)
        {
            return userService.ValidateAndGet(email, password);
        }






    }
}
