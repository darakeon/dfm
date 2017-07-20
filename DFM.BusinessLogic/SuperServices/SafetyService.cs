using System;
using DFM.BusinessLogic.Exceptions;
using DFM.BusinessLogic.Services;
using DFM.Email;
using DFM.Entities;
using DFM.Entities.Enums;

namespace DFM.BusinessLogic.SuperServices
{
    public class SafetyService
    {
        private readonly UserService userService;
        private readonly SecurityService securityService;

        internal SafetyService(UserService userService, SecurityService securityService)
        {
            this.userService = userService;
            this.securityService = securityService;
        }

        public void PasswordReset(String email, Format format)
        {
            createAndSend(email, SecurityAction.PasswordReset, format);
        }

        private void createAndSend(String email, SecurityAction action, Format format)
        {
            var user = userService.SelectByEmail(email);

            if (user == null)
                throw DFMCoreException.WithMessage(ExceptionPossibilities.WrongUserEmail);

            securityService.CreateAndSend(user, action, format);
        }


        internal Security SaveOrUpdate(Security security)
        {
            validateSecurity(security);

            return SaveOrUpdate(security);
        }



        public void UserActivate(String token)
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




        private void validateSecurity(Security security)
        {
            var currentUser = userService.SelectById(security.User.ID);

            if (currentUser == null || currentUser.Email != security.User.Email)
                throw DFMCoreException.WithMessage(ExceptionPossibilities.WrongUserEmail);
        }



        public User SaveAndSendVerify(User user, Format format)
        {
            user = userService.SaveOrUpdate(user);

            securityService.SendUserVerify(user, format);

            return user;
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

        public void SendUserVerify(User user, Format format)
        {
            securityService.SendUserVerify(user, format);
        }
    }
}
