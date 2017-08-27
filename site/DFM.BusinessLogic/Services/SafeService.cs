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
        private readonly UserRepository userRepository;
        private readonly SecurityRepository securityRepository;
        private readonly TicketRepository ticketRepository;

        internal SafeService(ServiceAccess serviceAccess, UserRepository userRepository, SecurityRepository securityRepository, TicketRepository ticketRepository)
            : base(serviceAccess)
        {
            this.userRepository = userRepository;
            this.securityRepository = securityRepository;
            this.ticketRepository = ticketRepository;
        }



        public void SendPasswordReset(String email)
        {
            var user = userRepository.GetByEmail(email);

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

                user = userRepository.Save(user);

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
            var user = userRepository.GetByEmail(email);

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

            userRepository.ValidateSecurity(security);
            security = securityRepository.SaveOrUpdate(security);

            securityRepository.SendEmail(security);
        }



        public void ActivateUser(String token)
        {
            BeginTransaction();

            try
            {
                var security = securityRepository.ValidateAndGet(token, SecurityAction.UserVerification);

                userRepository.Activate(security.User);

                securityRepository.Disable(token);

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
                var security = securityRepository.ValidateAndGet(token, SecurityAction.PasswordReset);

                security.User.Password = password;

                userRepository.ChangePassword(security.User);

                securityRepository.Disable(token);

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
            securityRepository.ValidateAndGet(token, securityAction);
        }

        public void DisableToken(String token)
        {
            BeginTransaction();

            try
            {
                securityRepository.Disable(token);

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
            var ticket = ticketRepository.GetByKey(ticketKey);

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
                var user = userRepository.ValidateAndGet(email, password);

                var ticket = ticketRepository.GetByKey(ticketKey);

                if (ticket == null)
                {
                    ticket = ticketRepository.Create(user, ticketKey);
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
                var ticket = ticketRepository.GetByKey(ticketKey);

                if (ticket != null && ticket.Active)
                {
                    ticket.Key += DateTime.UtcNow.ToString("yyyyMMddHHmmssffffff");
                    ticket.Active = false;
                    ticket.Expiration = DateTime.UtcNow;

                    ticketRepository.Disable(ticket);
                }

                CommitTransaction();
            }
            catch (Exception)
            {
                RollbackTransaction();
                throw;
            }
        }



        internal void VerifyUser()
        {
            if (Parent.Current.User == null || !Parent.Current.User.Active)
                throw DFMCoreException.WithMessage(ExceptionPossibilities.Unauthorized);
        }

    }
}
