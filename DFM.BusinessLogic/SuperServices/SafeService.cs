using System;
using DFM.BusinessLogic.Exceptions;
using DFM.BusinessLogic.Services;
using DFM.Entities;
using DFM.Entities.Enums;
using DFM.Authentication;

namespace DFM.BusinessLogic.SuperServices
{
    public class SafeService : BaseSuperService, ISafeService
    {
        private readonly UserService userService;
        private readonly SecurityService securityService;
        private readonly TicketService ticketService;

        internal SafeService(ServiceAccess serviceAccess, UserService userService, SecurityService securityService, TicketService ticketService)
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
            catch (Exception)
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
            catch (DFMCoreException)
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
            catch (Exception)
            {
                RollbackTransaction();
                throw;
            }
        }


        public User GetUserByTicket(String ticketKey)
        {
            var ticket = getTicketByKey(ticketKey);

            if (!ticket.User.Active)
                throw DFMCoreException.WithMessage(ExceptionPossibilities.DisabledUser);

            return ticket.User;
        }

        public String ValidateUserAndGetTicket(String email, String password)
        {
            var user = userService.ValidateAndGet(email, password);

            var ticket = new Ticket
            {
                User = user,
            };

            ticket = ticketService.Create(ticket);

            return ticket.Key;
        }

        public void DisableTicket(String ticketKey)
        {
            var ticket = getTicketByKey(ticketKey);

            ticket.Active = false;
            ticket.Expiration = DateTime.Now;

            ticketService.Disable(ticket);
        }

        private Ticket getTicketByKey(String ticketKey)
        {
            var ticket = ticketService.GetByKey(ticketKey);

            if (ticket == null)
                throw DFMCoreException.WithMessage(ExceptionPossibilities.InvalidTicket);

            if (!ticket.Active)
                throw DFMCoreException.WithMessage(ExceptionPossibilities.InvalidTicket);
            return ticket;
        }


    }
}
