using System;
using DFM.BusinessLogic.Exceptions;
using DFM.BusinessLogic.Services;
using DFM.Email;
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

            SendPasswordReset(user);
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

        
        
        public User SaveUserAndSendVerify(User user)
        {
            user = userService.SaveOrUpdate(user);

            SendUserVerify(user);

            return user;
        }

        public void ActivateUser(String token)
        {
            var security = securityService.SelectByToken(token);

            if (security.Action != SecurityAction.UserVerification)
                throw DFMCoreException.WithMessage(ExceptionPossibilities.InvalidToken);

            userService.Activate(security.User);

            securityService.Deactivate(token);
        }



        public Boolean SecurityTokenExist(String token)
        {
            return securityService.TokenExist(token);
        }

        public User SelectUserByEmail(String email)
        {
            return userService.SelectByEmail(email);
        }

        public SecurityAction GetSecurityTokenAction(String token)
        {
            return securityService.GetTokenAction(token);
        }

        public void Deactivate(String token)
        {
            securityService.Deactivate(token);
        }

        public User ValidateAndGet(String email, String password)
        {
            return userService.ValidateAndGet(email, password);
        }

        public void SendUserVerify(User user)
        {
            createAndSend(user, SecurityAction.UserVerification);
        }

        public void SendPasswordReset(User user)
        {
            createAndSend(user, SecurityAction.PasswordReset);
        }



        private void createAndSend(User user, SecurityAction action)
        {
            var security = new Security { Action = action, User = user };

            userService.ValidateSecurity(security);
            security = securityService.SaveOrUpdate(security);

            securityService.SendEmail(security);
        }

    }
}
