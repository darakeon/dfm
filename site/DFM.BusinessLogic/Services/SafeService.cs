using System;
using System.Collections.Generic;
using DK.MVC.Cookies;
using DFM.BusinessLogic.Exceptions;
using DFM.BusinessLogic.Helpers;
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



		public void SendPasswordReset(String email, String pathAction, String pathDisable)
        {
	        InTransaction(() =>
	        {
		        var user = userRepository.GetByEmail(email);

		        if (user == null)
			        throw DFMCoreException.WithMessage(ExceptionPossibilities.InvalidUser);

		        createAndSendToken(user, SecurityAction.PasswordReset, pathAction, pathDisable);
			});
        }

		public void SaveUserAndSendVerify(String email, String password, String language, String pathAction, String pathDisable)
        {
            InTransaction(() =>
            {
                var user = new User
                {
                    Email = email, 
                    Password = password,
                    Config = new Config
                    {
                        Language = language
                    }
                };

                user = userRepository.Save(user);

                sendUserVerify(user, pathAction, pathDisable);
			});
        }

		public void SendUserVerify(String email, String pathAction, String pathDisable)
        {
            InTransaction(() =>
            {
                var user = userRepository.GetByEmail(email);

                if (user == null)
                    throw DFMCoreException.WithMessage(ExceptionPossibilities.InvalidUser);

                sendUserVerify(user, pathAction, pathDisable);
			});
        }

		private void sendUserVerify(User user, String pathAction, String pathDisable)
        {
            createAndSendToken(user, SecurityAction.UserVerification, pathAction, pathDisable);
        }

		private void createAndSendToken(User user, SecurityAction action, String pathAction, String pathDisable)
        {
            var security = new Security { Action = action, User = user };

            security = securityRepository.SaveOrUpdate(security);

            securityRepository.SendEmail(security, pathAction, pathDisable);

            var others = securityRepository
                .SimpleFilter(
                    s => s.ID != security.ID
                        && s.User.ID == security.User.ID
                        && s.Active
                );

            foreach (var other in others)
            {
                other.Active = false;
                securityRepository.SaveOrUpdate(other);
            }
        }



        public void ActivateUser(String token)
        {
            InTransaction(() =>
            {
                var security = securityRepository.ValidateAndGet(token, SecurityAction.UserVerification);

                userRepository.Activate(security.User);

                securityRepository.Disable(token);
			});

        }

        public void PasswordReset(String token, String password)
        {
            if (String.IsNullOrEmpty(password))
                throw DFMCoreException.WithMessage(ExceptionPossibilities.UserPasswordRequired);

            InTransaction(() =>
            {
                var security = securityRepository.ValidateAndGet(token, SecurityAction.PasswordReset);

                security.User.Password = password;

                userRepository.ChangePassword(security.User);

                securityRepository.Disable(token);
			});
        }


        public void TestSecurityToken(String token, SecurityAction securityAction)
        {
            securityRepository.ValidateAndGet(token, securityAction);
        }

        public void DisableToken(String token)
        {
            InTransaction(() => securityRepository.Disable(token));
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

        public String ValidateUserAndCreateTicket(String email, String password, PseudoTicket pseudoTicket)
        {
			return InTransaction(
				() => validateUserAndCreateTicket(email, password, pseudoTicket),
				() => addPasswordError(email)
			);
        }

	    private string validateUserAndCreateTicket(string email, string password, PseudoTicket pseudoTicket)
	    {
		    var user = userRepository.ValidateAndGet(email, password);

		    var ticket = ticketRepository.GetByKey(pseudoTicket.Key);

		    if (ticket == null)
		    {
			    ticket = ticketRepository.Create(user, pseudoTicket);
		    }
		    else if (ticket.User.Email != email)
		    {
			    throw DFMCoreException.WithMessage(ExceptionPossibilities.Uninvited);
		    }

		    return ticket.Key;
	    }

	    private void addPasswordError(String email)
        {
            var user = userRepository.GetByEmail(email);

            if (user == null)
                return;

            InTransaction(() =>
            {
                user.WrongLogin++;

                if (user.WrongPassExceeded())
                    user.Active = false;

                userRepository.SaveOrUpdate(user);
			});

            if (user.WrongPassExceeded())
                throw DFMCoreException.WithMessage(ExceptionPossibilities.DisabledUser);
        }

        public void DisableTicket(String ticketKey)
        {
            InTransaction(() =>
            {
                var ticket = ticketKey.Length == Defaults.TicketShowedPart
                    ? ticketRepository.GetByPartOfKey(Parent.Current.User, ticketKey)
                    : ticketRepository.GetByKey(ticketKey);

                if (ticket != null && ticket.Active)
                {
                    ticketRepository.Disable(ticket);
                }
            });
        }



        internal void VerifyUser()
        {
            if (Parent.Current.User == null || !Parent.Current.User.Active)
                throw DFMCoreException.WithMessage(ExceptionPossibilities.Unauthorized);
        }



        public IList<Ticket> ListLogins()
        {
            var user = Parent.Current.User;

            var tickets = ticketRepository.List(user);

            var logins = new List<Ticket>();

            foreach (var ticket in tickets)
            {
                var login = new Ticket
                {
                    Active = ticket.Active,
                    Creation = ticket.Creation,
                    Expiration = ticket.Expiration,
                    Key = ticket.Key.Substring(0, Defaults.TicketShowedPart),
                    Type = ticket.Type,
                };

                logins.Add(login);
            }

            return logins;
        }


    }
}
