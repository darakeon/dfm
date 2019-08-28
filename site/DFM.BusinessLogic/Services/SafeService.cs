using System;
using System.Collections.Generic;
using System.Linq;
using DFM.BusinessLogic.Exceptions;
using DFM.BusinessLogic.Helpers;
using DFM.BusinessLogic.Repositories;
using DFM.Entities;
using DFM.Entities.Enums;
using DFM.Authentication;
using DFM.BusinessLogic.InterfacesAndBases;
using Keon.Util.Extensions;
using Keon.TwoFactorAuth;
using Ticket = DFM.Entities.Ticket;

namespace DFM.BusinessLogic.Services
{
	public class SafeService : BaseService, ISafeService
	{
		private readonly UserRepository userRepository;
		private readonly SecurityRepository securityRepository;
		private readonly TicketRepository ticketRepository;
		private readonly ContractRepository contractRepository;
		private readonly AcceptanceRepository acceptanceRepository;

		private readonly Func<PathType, String> getPath;

		internal SafeService(ServiceAccess serviceAccess
			, UserRepository userRepository, SecurityRepository securityRepository, TicketRepository ticketRepository, ContractRepository contractRepository, AcceptanceRepository acceptanceRepository
			, Func<PathType, String> getPath
			)
			: base(serviceAccess)
		{
			this.userRepository = userRepository;
			this.securityRepository = securityRepository;
			this.ticketRepository = ticketRepository;
			this.contractRepository = contractRepository;
			this.acceptanceRepository = acceptanceRepository;

			this.getPath = getPath;
		}



		public void SendPasswordReset(String email)
		{
			InTransaction(() =>
			{
				var user = userRepository.GetByEmail(email);

				if (user == null)
					throw DFMCoreException.WithMessage(DfMError.InvalidUser);

				createAndSendToken(user, SecurityAction.PasswordReset, getPath(PathType.PasswordReset));
			});
		}

		public void SaveUserAndSendVerify(String email, IPasswordForm passwordForm, Boolean acceptedContract, Boolean enableWizard, String language)
		{
			InTransaction(() =>
			{
				passwordForm.Verify();

				var user = new User
				{
					Email = email,
					Password = passwordForm.Password,
					Config = new Config
					{
						Language = language,
						Wizard = enableWizard,
					}
				};

				user = userRepository.Save(user);

				if (acceptedContract)
				{
					acceptContract(user);
				}

				sendUserVerify(user);
			});
		}

		public void SendUserVerify(String email)
		{
			InTransaction(() =>
			{
				var user = userRepository.GetByEmail(email);

				if (user == null)
					throw DFMCoreException.WithMessage(DfMError.InvalidUser);

				sendUserVerify(user);
			});
		}

		private void sendUserVerify(User user)
		{
			createAndSendToken(user, SecurityAction.UserVerification, getPath(PathType.UserVerification));
		}

		private void createAndSendToken(User user, SecurityAction action, String pathAction)
		{
			var security = new Security { Action = action, User = user };

			security = securityRepository.SaveOrUpdate(security);

			securityRepository.SendEmail(security, pathAction, getPath(PathType.DisableToken));

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

		public void PasswordReset(String token, IPasswordForm passwordForm)
		{
			passwordForm.Verify();

			InTransaction(() =>
			{
				var security = securityRepository.ValidateAndGet(token, SecurityAction.PasswordReset);

				security.User.Password = passwordForm.Password;

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
				throw DFMCoreException.WithMessage(DfMError.Uninvited);

			if (!ticket.User.Active)
				throw DFMCoreException.WithMessage(DfMError.DisabledUser);

			return ticket.User;
		}

		public String ValidateUserAndCreateTicket(String email, String password, String ticketKey, TicketType ticketType)
		{
			return InTransaction(
				() => validateUserAndCreateTicket(email, password, ticketKey, ticketType),
				() => addPasswordError(email)
			);
		}

		private String validateUserAndCreateTicket(String email, String password, String ticketKey, TicketType ticketType)
		{
			var user = userRepository.ValidateAndGet(email, password);

			ticketKey = ticketKey ?? Token.New();
			var ticket = ticketRepository.GetByKey(ticketKey);

			if (ticket == null)
			{
				ticket = ticketRepository.Create(user, ticketKey, ticketType);
			}
			else if (ticket.User.Email != email)
			{
				throw DFMCoreException.WithMessage(DfMError.Uninvited);
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
				throw DFMCoreException.WithMessage(DfMError.DisabledUser);
		}

		public void DisableTicket(String ticketKey)
		{
			InTransaction(() =>
			{
				if (ticketKey == null) return;

				var ticket = ticketKey.Length == Defaults.TICKET_SHOWED_PART
					? ticketRepository.GetByPartOfKey(Parent.Current.User, ticketKey)
					: ticketRepository.GetByKey(ticketKey);

				if (ticket == null) return;

				if (ticket.User.ID != Parent.Current.User.ID)
					DFMCoreException.WithMessage(DfMError.Uninvited);

				if (ticket.Active)
					ticketRepository.Disable(ticket);
			});
		}

		internal void VerifyUser()
		{
			if (Parent.Current.User == null || !Parent.Current.User.Active)
				throw DFMCoreException.WithMessage(DfMError.Uninvited);

			if (!Parent.Current.IsVerified)
				throw DFMCoreException.WithMessage(DfMError.TFANotVerified);

			if (!IsLastContractAccepted())
				throw DFMCoreException.WithMessage(DfMError.NotSignedLastContract);
		}



		public IList<Ticket> ListLogins()
		{
			VerifyUser();

			var user = Parent.Current.User;

			var tickets = ticketRepository.List(user);

			return tickets.Select(getLogin).ToList();
		}

		private static Ticket getLogin(Ticket ticket)
		{
			return new Ticket
			{
				Active = ticket.Active,
				Creation = ticket.Creation,
				Expiration = ticket.Expiration,
				Key = ticket.Key.Substring(0, Defaults.TICKET_SHOWED_PART),
				Type = ticket.Type,
			};
		}



		public void ChangePassword(String currentPassword, IPasswordForm passwordForm)
		{
			VerifyUser();

			passwordForm.Verify();

			var user = Parent.Current.User;

			if (!userRepository.VerifyPassword(user, currentPassword))
				throw DFMCoreException.WithMessage(DfMError.WrongPassword);

			InTransaction(() =>
			{
				user.Password = passwordForm.Password;
				userRepository.ChangePassword(user);

				var ticketList = ticketRepository.List(user);

				foreach (var ticket in ticketList)
				{
					ticketRepository.Disable(ticket);
				}
			});
		}



		public void UpdateEmail(String currentPassword, String email)
		{
			VerifyUser();

			var user = Parent.Current.User;

			if (!userRepository.VerifyPassword(user, currentPassword))
				throw DFMCoreException.WithMessage(DfMError.WrongPassword);

			InTransaction(() =>
			{
				user = userRepository.UpdateEmail(user.ID, email);
				sendUserVerify(user);
			});
		}



		public Contract GetContract()
		{
			return contractRepository.GetContract();
		}


		public Boolean IsLastContractAccepted()
		{
			var contract = GetContract();

			if (contract == null)
				return true;

			var user = Parent.Current.User;

			var acceptance = InTransaction(() =>
				acceptanceRepository.GetOrCreate(user, contract)
			);

			return acceptance?.Accepted ?? false;
		}


		public void AcceptContract()
		{
			InTransaction(() =>
				acceptContract(Parent.Current.User)
			);
		}

		private void acceptContract(User user)
		{
			var contract = GetContract();
			acceptanceRepository.Accept(user, contract);
		}


		public void UpdateTFA(String secret, String code, String currentPassword)
		{
			InTransaction(() =>
			{
				if (String.IsNullOrEmpty(secret))
					DFMCoreException.WithMessage(DfMError.TFAEmptySecret);

				var codes = CodeGenerator.Generate(secret, 2);

				if (!codes.Contains(code))
					DFMCoreException.WithMessage(DfMError.TFAWrongCode);

				var user = Parent.Current.User;

				if (!userRepository.VerifyPassword(user, currentPassword))
					DFMCoreException.WithMessage(DfMError.TFAWrongPassword);

				userRepository.SaveTFA(user, secret);
			});
		}

		public void RemoveTFA(String currentPassword)
		{
			InTransaction(() =>
			{
				var user = Parent.Current.User;

				if (!userRepository.VerifyPassword(user, currentPassword))
					DFMCoreException.WithMessage(DfMError.TFAWrongPassword);

				userRepository.SaveTFA(user, null);
			});
		}

		public void ValidateTicketTFA(String code)
		{
			InTransaction(() =>
			{
				var secret = Parent.Current.User.TFASecret;

				if (secret == null)
					DFMCoreException.WithMessage(DfMError.TFANotConfigured);

				var codes = CodeGenerator.Generate(secret, 2);

				if (!codes.Contains(code))
					DFMCoreException.WithMessage(DfMError.TFAWrongCode);

				var ticket = ticketRepository.GetByKey(Parent.Current.TicketKey);
				ticket.ValidTFA = true;
				ticketRepository.SaveOrUpdate(ticket);
			});
		}

		public Boolean VerifyTicket()
		{
			var ticket = ticketRepository.GetByKey(Parent.Current.TicketKey);

			if (ticket == null)
			{
				DFMCoreException.WithMessage(DfMError.Uninvited);
				return false;
			}

			return String.IsNullOrEmpty(ticket.User.TFASecret)
				|| ticket.ValidTFA;
		}

		public Boolean VerifyTicket(TicketType type)
		{
			var ticket = ticketRepository.GetByKey(Parent.Current.TicketKey);

			if (ticket == null)
			{
				DFMCoreException.WithMessage(DfMError.Uninvited);
				return false;
			}

			return ticket.Type == type;
		}
	}
}
