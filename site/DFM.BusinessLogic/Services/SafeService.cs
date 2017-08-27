using System;
using DFM.BusinessLogic.Exceptions;
using DFM.BusinessLogic.Repositories;
using DFM.Entities;
using DFM.Entities.Enums;
using DFM.Authentication;

namespace DFM.BusinessLogic.Services
{
    public class SafeService : BaseService, ISafeService
    {
        private readonly UserRepository userService;
        private readonly SecurityRepository securityService;
        private readonly TicketRepository ticketService;

        internal SafeService(ServiceAccess serviceAccess, UserRepository userService, SecurityRepository securityService, TicketRepository ticketService)
            : base(serviceAccess)
        {
            this.userService = userService;
            this.securityService = securityService;
            this.ticketService = ticketService;
        }



        public void SendPasswordReset(String email)
        {
            var user = userService.GetByEmail(email);

            if (user == null)
                throw DFMCoreException.WithMessage(ExceptionPossibilities.InvalidUser);

            createAndSendToken(user, SecurityAction.PasswordReset);
        }
        
        public void SaveUserAndSendVerify(String email, String password)
        {
            BeginTransaction();

            try
            {
                var user = new User { Email = email, Password = password };

                user = userService.Save(user);

                sendUserVerify(user);

                CommitTransaction();
            }
            catch
            {
                RollbackTransaction();
                throw;
            }
        }

        public void SendUserVerify(String email)
        {
            var user = userService.GetByEmail(email);

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
            BeginTransaction();

            try
            {
                var security = securityService.ValidateAndGet(token, SecurityAction.UserVerification);

                userService.Activate(security.User);

                securityService.Disable(token);

                CommitTransaction();
            }
            catch
            {
                RollbackTransaction();
                throw;
            }

        }

        public void PasswordReset(String token, String password)
        {
            if (String.IsNullOrEmpty(password))
                throw DFMCoreException.WithMessage(ExceptionPossibilities.UserPasswordRequired);

            BeginTransaction();

            try
            {
                var security = securityService.ValidateAndGet(token, SecurityAction.PasswordReset);

                security.User.Password = password;

                userService.ChangePassword(security.User);

                securityService.Disable(token);

                CommitTransaction();
            }
            catch
            {
                RollbackTransaction();
                throw;
            }
        }


        public void TestSecurityToken(String token, SecurityAction securityAction)
        {
            securityService.ValidateAndGet(token, securityAction);
        }

        public void DisableToken(String token)
        {
            BeginTransaction();

            try
            {
                securityService.Disable(token);

                CommitTransaction();
            }
            catch
            {
                RollbackTransaction();
                throw;
            }
        }


        public User GetUserByTicket(String ticketKey)
        {
            var ticket = ticketService.GetByKey(ticketKey);

            if (ticket == null || !ticket.Active)
                throw DFMCoreException.WithMessage(ExceptionPossibilities.Uninvited);

            if (!ticket.User.Active)
                throw DFMCoreException.WithMessage(ExceptionPossibilities.DisabledUser);

            return ticket.User;
        }

        public String ValidateUserAndCreateTicket(String email, String password, String ticketKey)
        {
            BeginTransaction();
            
            try
            {
                var user = userService.ValidateAndGet(email, password);

                var ticket = ticketService.GetByKey(ticketKey);

                if (ticket == null)
                {
                    ticket = ticketService.Create(user, ticketKey);
                }
                else if (ticket.User.Email != email)
                {
                    throw DFMCoreException.WithMessage(ExceptionPossibilities.Uninvited);
                }

                CommitTransaction();

                return ticket.Key;
            }
            catch
            {
                RollbackTransaction();
                throw;
            }
        }

        public void DisableTicket(String ticketKey)
        {
            BeginTransaction();

            try
            {
                var ticket = ticketService.GetByKey(ticketKey);

                if (ticket != null && ticket.Active)
                {
                    ticket.Key += DateTime.Now.ToString("yyyyMMddHHmmssffffff");
                    ticket.Active = false;
                    ticket.Expiration = DateTime.Now;

                    ticketService.Disable(ticket);
                }

                CommitTransaction();
            }
            catch (Exception)
            {
                RollbackTransaction();
                throw;
            }
        }


    }
}
