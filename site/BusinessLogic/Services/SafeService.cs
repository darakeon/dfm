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
using DFM.BusinessLogic.Response;
using DFM.Entities.Bases;
using Keon.Util.Extensions;
using Keon.TwoFactorAuth;

namespace DFM.BusinessLogic.Services
{
	public class SafeService : BaseService, ISafeService<SignInInfo, SessionInfo>
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
					return;

				createAndSendToken(user, SecurityAction.PasswordReset, getPath(PathType.PasswordReset));
			});
		}
		
		public void SaveUserAndSendVerify(SignUpInfo info)
		{
			InTransaction(() =>
			{
				info.VerifyPassword();

				var user = info.GetEntity();

				user = userRepository.Save(user);

				if (info.AcceptedContract)
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
					throw Error.InvalidUser.Throw();

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

			security = securityRepository.Save(security);

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
				securityRepository.Save(other);
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

		public void PasswordReset(PasswordResetInfo reset)
		{
			reset.VerifyPassword();

			InTransaction(() =>
			{
				var security = securityRepository.ValidateAndGet(
					reset.Token,
					SecurityAction.PasswordReset
				);

				security.User.Password = reset.Password;

				userRepository.ChangePassword(security.User);

				securityRepository.Disable(reset.Token);
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


		public SessionInfo GetSessionByTicket(String ticketKey)
		{
			return new SessionInfo(
				getUserByTicket(ticketKey)
			);
		}

		private User getUserByTicket(String ticketKey)
		{
			var ticket = ticketRepository.GetByKey(ticketKey);

			if (ticket == null || !ticket.Active)
				throw Error.Uninvited.Throw();

			if (!ticket.User.Active)
				throw Error.DisabledUser.Throw();

			return ticket.User;
		}

		public String ValidateUserAndCreateTicket(SignInInfo info)
		{
			return InTransaction(
				() => validateUserAndCreateTicket(info),
				() => addPasswordError(info.Email)
			);
		}

		private String validateUserAndCreateTicket(SignInInfo info)
		{
			var user = userRepository.ValidateAndGet(info.Email, info.Password);

			if (String.IsNullOrEmpty(info.TicketKey))
				info.TicketKey = Token.New();

			var ticket = ticketRepository.GetByKey(info.TicketKey);

			if (ticket == null)
			{
				ticket = ticketRepository.Create(user, info.TicketKey, info.TicketType);
			}
			else if (ticket.User.Email != info.Email)
			{
				throw Error.Uninvited.Throw();
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
				throw Error.DisabledUser.Throw();
		}

		public void DisableTicket(String ticketKey)
		{
			InTransaction(() =>
			{
				if (ticketKey == null) return;

				var user = GetCurrent();

				var ticket = ticketKey.Length == Defaults.TicketShowedPart
					? ticketRepository.GetByPartOfKey(user, ticketKey)
					: ticketRepository.GetByKey(ticketKey);

				if (ticket == null) return;

				if (ticket.User.ID != user.ID)
					throw Error.Uninvited.Throw();

				if (ticket.Active)
					ticketRepository.Disable(ticket);
			});
		}

		internal void VerifyUser()
		{
			var user = GetCurrent();

			if (user == null || !user.Active)
				throw Error.Uninvited.Throw();

			if (!parent.Current.IsVerified)
				throw Error.TFANotVerified.Throw();

			if (!IsLastContractAccepted())
				throw Error.NotSignedLastContract.Throw();
		}



		public IList<TicketInfo> ListLogins()
		{
			VerifyUser();

			var user = GetCurrent();
			var tickets = ticketRepository.List(user);

			return tickets
				.Select(TicketInfo.Convert)
				.ToList();
		}

		public void ChangePassword(ChangePasswordInfo info)
		{
			VerifyUser();

			info.VerifyPassword();

			var user = GetCurrent();

			if (!userRepository.VerifyPassword(user, info.CurrentPassword))
				throw Error.WrongPassword.Throw();

			InTransaction(() =>
			{
				user.Password = info.Password;
				userRepository.ChangePassword(user);

				var ticketList = ticketRepository.List(user);

				foreach (var ticket in ticketList)
				{
					ticketRepository.Disable(ticket);
				}
			});
		}



		public void UpdateEmail(String password, String email)
		{
			VerifyUser();

			var user = GetCurrent();

			if (!userRepository.VerifyPassword(user, password))
				throw Error.WrongPassword.Throw();

			InTransaction(() =>
			{
				user = userRepository.UpdateEmail(user.ID, email);
				sendUserVerify(user);
			});
		}



		public ContractInfo GetContract()
		{
			var contract = getContract();

			if (contract == null)
				return null;

			return new ContractInfo(contract);
		}

		private Contract getContract()
		{
			return contractRepository.GetContract();
		}


		public Boolean IsLastContractAccepted()
		{
			var contract = getContract();

			if (contract == null)
				return true;

			var user = GetCurrent();

			var acceptance = InTransaction(() =>
				acceptanceRepository.GetOrCreate(user, contract)
			);

			return acceptance?.Accepted ?? false;
		}


		public void AcceptContract()
		{
			InTransaction(() =>
				acceptContract(GetCurrent())
			);
		}

		private void acceptContract(User user)
		{
			var contract = getContract();
			acceptanceRepository.Accept(user, contract);
		}


		public void UpdateTFA(TFAInfo info)
		{
			InTransaction(() =>
			{
				if (String.IsNullOrEmpty(info.Secret))
					throw Error.TFAEmptySecret.Throw();

				var codes = CodeGenerator.Generate(info.Secret, 2);

				if (!codes.Contains(info.Code))
					throw Error.TFAWrongCode.Throw();

				var user = GetCurrent();

				if (!userRepository.VerifyPassword(user, info.Password))
					throw Error.TFAWrongPassword.Throw();

				userRepository.SaveTFA(user, info.Secret);
			});
		}

		public void RemoveTFA(String currentPassword)
		{
			InTransaction(() =>
			{
				var user = GetCurrent();

				if (!userRepository.VerifyPassword(user, currentPassword))
					throw Error.TFAWrongPassword.Throw();

				userRepository.SaveTFA(user, null);
			});
		}

		public void ValidateTicketTFA(String code)
		{
			InTransaction(() =>
			{
				var secret = GetCurrent().TFASecret;

				if (secret == null)
					throw Error.TFANotConfigured.Throw();

				var codes = CodeGenerator.Generate(secret, 2);

				if (!codes.Contains(code))
					throw Error.TFAWrongCode.Throw();

				var ticket = ticketRepository.GetByKey(parent.Current.TicketKey);
				ticket.ValidTFA = true;
				ticketRepository.SaveOrUpdate(ticket);
			});
		}

		public Boolean VerifyTicket()
		{
			var ticket = ticketRepository.GetByKey(parent.Current.TicketKey);

			if (ticket == null)
				throw Error.Uninvited.Throw();

			return String.IsNullOrEmpty(ticket.User.TFASecret)
				|| ticket.ValidTFA;
		}

		public Boolean VerifyTicket(TicketType type)
		{
			var ticket = ticketRepository.GetByKey(parent.Current.TicketKey);

			if (ticket == null)
				throw Error.Uninvited.Throw();

			return ticket.Type == type;
		}

		internal User GetCurrent()
		{
			if (!parent.Current.IsAuthenticated)
				return null;

			return getUserByTicket(parent.Current.TicketKey);
		}
	}
}
