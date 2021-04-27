using System;
using System.Collections.Generic;
using System.Linq;
using DFM.BusinessLogic.Exceptions;
using DFM.BusinessLogic.Repositories;
using DFM.Entities;
using DFM.Entities.Enums;
using DFM.Authentication;
using DFM.BusinessLogic.Response;
using DFM.Entities.Bases;
using Keon.Util.Extensions;

namespace DFM.BusinessLogic.Services
{
	public class SafeService : Service, ISafeService<SignInInfo, SessionInfo>
	{
		internal SafeService(ServiceAccess serviceAccess, Repos repos)
			: base(serviceAccess, repos) { }

		public void SendPasswordReset(String email)
		{
			inTransaction("SendPasswordReset", () =>
			{
				var user = repos.User.GetByEmail(email);

				if (user == null)
					return;

				repos.Security.CreateAndSendToken(
					user, SecurityAction.PasswordReset
				);
			});
		}
		
		public void SaveUser(SignUpInfo info)
		{
			inTransaction("SaveUser", () =>
			{
				info.VerifyPassword();

				var user = info.GetEntity();

				user = repos.User.Save(user);

				if (info.AcceptedContract)
				{
					acceptContract(user);
				}

				sendUserVerify(user);
			});
		}

		public void SendUserVerify(String email)
		{
			inTransaction("SendUserVerify", () =>
			{
				var user = repos.User.GetByEmail(email);

				if (user == null)
					throw Error.InvalidUser.Throw();

				sendUserVerify(user);
			});
		}

		private void sendUserVerify(User user)
		{
			repos.Security.CreateAndSendToken(
				user, SecurityAction.UserVerification
			);
		}

		public void ActivateUser(String token)
		{
			inTransaction("ActivateUser", () =>
			{
				var security = repos.Security.ValidateAndGet(token, SecurityAction.UserVerification);

				repos.User.Activate(security.User);

				repos.Security.Disable(token);
			});
		}

		public void ResetPassword(PasswordResetInfo reset)
		{
			reset.VerifyPassword();

			inTransaction("PasswordReset", () =>
			{
				var security = repos.Security.ValidateAndGet(
					reset.Token,
					SecurityAction.PasswordReset
				);

				security.User.Password = reset.Password;

				repos.User.ChangePassword(security.User);

				repos.Security.Disable(reset.Token);
			});
		}

		public void TestSecurityToken(String token, SecurityAction securityAction)
		{
			repos.Security.ValidateAndGet(token, securityAction);
		}

		public void DisableToken(String token)
		{
			inTransaction("DisableToken", () => repos.Security.Disable(token));
		}

		public SessionInfo GetSession(String ticketKey)
		{
			return new(
				getUserByTicket(ticketKey)
			);
		}

		private User getUserByTicket(String ticketKey)
		{
			var ticket = repos.Ticket.GetByKey(ticketKey);

			if (ticket == null || !ticket.Active)
				throw Error.Uninvited.Throw();

			if (!ticket.User.Active)
				throw Error.DisabledUser.Throw();

			return ticket.User;
		}

		public String CreateTicket(SignInInfo info)
		{
			return inTransaction("CreateTicket",
				() => createTicket(info),
				() => addPasswordError(info.Email)
			);
		}

		private String createTicket(SignInInfo info)
		{
			var auth = repos.User.ValidateAndGet(info.Email, info.Password);
			var user = auth.User;

			if (String.IsNullOrEmpty(info.TicketKey))
				info.TicketKey = Token.New();

			var ticket = repos.Ticket.GetByKey(info.TicketKey);

			if (ticket == null)
			{
				ticket = repos.Ticket.Create(user, info.TicketKey, info.TicketType);
			}
			else if (ticket.User.Email != info.Email)
			{
				throw Error.Uninvited.Throw();
			}

			if (auth.UsedTFAPassword)
			{
				repos.Ticket.ValidateTFA(ticket);
			}

			return ticket.Key;
		}

		private void addPasswordError(String email)
		{
			var user = repos.User.GetByEmail(email);

			if (user == null)
				return;

			inTransaction("AddPasswordError", () =>
			{
				user.WrongLogin++;

				if (user.WrongPassExceeded())
					user.Active = false;

				repos.User.SaveOrUpdate(user);
			});

			if (user.WrongPassExceeded())
				throw Error.DisabledUser.Throw();
		}

		public void DisableTicket(String ticketKey)
		{
			inTransaction("DisableTicket", () =>
			{
				if (ticketKey == null) return;

				var user = GetCurrent();

				var ticket = ticketKey.Length == Defaults.TicketShowedPart
					? repos.Ticket.GetByPartOfKey(user, ticketKey)
					: repos.Ticket.GetByKey(ticketKey);

				if (ticket == null) return;

				if (ticket.User.ID != user.ID)
					throw Error.Uninvited.Throw();

				if (ticket.Active)
					repos.Ticket.Disable(ticket);
			});
		}

		internal void VerifyUser()
		{
			VerifyUser(GetCurrent());
		}

		internal void VerifyUser(User user)
		{
			if (user == null || !user.Active)
				throw Error.Uninvited.Throw();

			if (!parent.Current.IsVerified)
				throw Error.TFANotVerified.Throw();

			if (!IsLastContractAccepted(user))
				throw Error.NotSignedLastContract.Throw();
		}

		public IList<TicketInfo> ListLogins()
		{
			VerifyUser();

			var user = GetCurrent();
			var tickets = repos.Ticket.List(user);

			return tickets
				.Select(TicketInfo.Convert)
				.ToList();
		}

		public void ChangePassword(ChangePasswordInfo info)
		{
			VerifyUser();

			info.VerifyPassword();

			var user = GetCurrent();

			if (!repos.User.VerifyPassword(user, info.CurrentPassword))
				throw Error.WrongPassword.Throw();

			inTransaction("ChangePassword", () =>
			{
				user.Password = info.Password;
				repos.User.ChangePassword(user);

				var ticketList = repos.Ticket.List(user);

				foreach (var ticket in ticketList)
				{
					if (ticket.Key != parent.Current.TicketKey)
					{
						repos.Ticket.Disable(ticket);
					}
				}
			});
		}

		public void UpdateEmail(String password, String email)
		{
			VerifyUser();

			var user = GetCurrent();

			if (!repos.User.VerifyPassword(user, password))
				throw Error.WrongPassword.Throw();

			inTransaction("UpdateEmail", () =>
			{
				user = repos.User.UpdateEmail(user.ID, email);
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
			return repos.Contract.GetContract();
		}

		public Boolean IsLastContractAccepted()
		{
			return IsLastContractAccepted(GetCurrent());
		}

		internal Boolean IsLastContractAccepted(User user)
		{
			var contract = getContract();

			if (contract == null)
				return true;

			var acceptance = inTransaction("CreateAcceptance", () =>
				repos.Acceptance.GetOrCreate(user, contract)
			);

			return acceptance?.Accepted ?? false;
		}

		public void AcceptContract()
		{
			inTransaction("AcceptContract", () =>
				acceptContract(GetCurrent())
			);
		}

		private void acceptContract(User user)
		{
			var contract = getContract();
			repos.Acceptance.Accept(user, contract);
		}

		public void UpdateTFA(TFAInfo info)
		{
			inTransaction("UpdateTFA", () =>
			{
				if (String.IsNullOrEmpty(info.Secret))
					throw Error.TFAEmptySecret.Throw();

				if (!repos.User.IsValid(info.Secret, info.Code))
					throw Error.TFAWrongCode.Throw();

				var user = GetCurrent();

				if (!repos.User.VerifyPassword(user, info.Password))
					throw Error.TFAWrongPassword.Throw();

				repos.User.SaveTFA(user, info.Secret);
			});
		}

		public void RemoveTFA(String currentPassword)
		{
			inTransaction("RemoveTFA", () =>
			{
				var user = GetCurrent();

				if (!repos.User.VerifyPassword(user, currentPassword))
					throw Error.TFAWrongPassword.Throw();

				repos.User.SaveTFA(user, null);
			});
		}

		public void ValidateTicketTFA(String code)
		{
			inTransaction("ValidateTicketTFA", () =>
			{
				var secret = GetCurrent().TFASecret;

				if (secret == null)
					throw Error.TFANotConfigured.Throw();

				if (!repos.User.IsValid(secret, code))
					throw Error.TFAWrongCode.Throw();

				var ticket = repos.Ticket.GetByKey(parent.Current.TicketKey);
				repos.Ticket.ValidateTFA(ticket);
			});
		}

		public Boolean VerifyTicketTFA()
		{
			var ticket = repos.Ticket.GetByKey(parent.Current.TicketKey);

			if (ticket == null)
				throw Error.Uninvited.Throw();

			return String.IsNullOrEmpty(ticket.User.TFASecret)
				|| ticket.ValidTFA;
		}

		public Boolean VerifyTicketType(TicketType type)
		{
			var ticket = repos.Ticket.GetByKey(parent.Current.TicketKey);

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

		public void SaveAccess()
		{
			var key = parent.Current.TicketKey;
			var ticket = repos.Ticket.GetByKey(key);

			if (ticket == null)
				return;

			inTransaction("SaveAccess", () =>
			{
				ticket.LastAccess = DateTime.Now;
				repos.Ticket.SaveOrUpdate(ticket);
			});
		}

		public void UseTFAAsPassword(Boolean use)
		{
			inTransaction("UseTFAAsPassword", () =>
			{
				var user = GetCurrent();
				repos.User.UseTFAAsPassword(user, use);
			});
		}
	}
}
