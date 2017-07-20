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
            createAndSend(user, SecurityAction.PasswordReset);
        }
      
        
        public User SaveUserAndSendVerify(User user)
        {
            user = userService.SaveOrUpdate(user);

            sendUserVerify(user);

            return user;
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
            createAndSend(user, SecurityAction.UserVerification);
        }


        private void createAndSend(User user, SecurityAction action)
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


        


    }
}
