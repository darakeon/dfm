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
using Keon.Util.Crypto;
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

				if (user.Control.ProcessingDeletion)
					throw Error.UserDeleted.Throw();

				if (user.Control.WipeRequest != null)
					throw Error.UserAskedWipe.Throw();

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
				user.Control.MiscDna = Misc.RandomDNA();

				user = repos.User.Save(user);

				if (info.AcceptedContract)
					acceptContract(user);

				sendUserVerify(user);
			});
		}

		public void SendUserVerify()
		{
			SendUserVerify(parent.Current.Email);
		}

		public void SendUserVerify(String email)
		{
			inTransaction("SendUserVerify", () =>
			{
				var user = repos.User.GetByEmail(email);

				if (user == null)
					throw Error.InvalidUser.Throw();

				if (user.Control.ProcessingDeletion)
					throw Error.UserDeleted.Throw();

				if (user.Control.WipeRequest != null)
					throw Error.UserAskedWipe.Throw();

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
				var security = repos.Security.ValidateAndGet(
					token, SecurityAction.UserVerification
				);

				if (security.User.Control.ProcessingDeletion)
					throw Error.UserDeleted.Throw();

				if (security.User.Control.WipeRequest != null)
					throw Error.UserAskedWipe.Throw();

				repos.Control.Activate(security.User);

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

				var user = security.User;

				if (user.Control.ProcessingDeletion)
					throw Error.UserDeleted.Throw();

				if (user.Control.WipeRequest != null)
					throw Error.UserAskedWipe.Throw();

				user.Password = reset.Password;

				repos.User.ChangePassword(user);

				repos.Security.Disable(reset.Token);
			});
		}

		public void TestSecurityToken(String token, SecurityAction securityAction)
		{
			var security = repos.Security.ValidateAndGet(token, securityAction);

			if (security.User.Control.ProcessingDeletion)
				throw Error.UserDeleted.Throw();

			if (security.User.Control.WipeRequest != null)
				throw Error.UserAskedWipe.Throw();
		}

		public void DisableToken(String token)
		{
			inTransaction("DisableToken",
				() => repos.Security.Disable(token)
			);
		}

		public SessionInfo GetSession(String ticketKey)
		{
			var user = getUserByTicket(ticketKey);

			if (user.Control.ProcessingDeletion)
				throw Error.UserDeleted.Throw();

			if (user.Control.WipeRequest != null)
				throw Error.UserAskedWipe.Throw();

			return new(user);
		}

		private User getUserByTicket(String ticketKey)
		{
			var ticket = repos.Ticket.GetByKey(ticketKey);

			if (ticket is not {Active: true})
				throw Error.Uninvited.Throw();

			var user = ticket.User;

			if (!user.Control.ActiveOrAllowedPeriod())
				throw Error.DisabledUser.Throw();

			return user;
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

			if (user.Control.ProcessingDeletion)
				throw Error.UserDeleted.Throw();

			if (user.Control.WipeRequest != null)
				throw Error.UserAskedWipe.Throw();

			if (String.IsNullOrEmpty(info.TicketKey))
				info.TicketKey = Token.New();

			var ticket = repos.Ticket.GetByKey(info.TicketKey);

			if (ticket == null)
				ticket = repos.Ticket.Create(user, info.TicketKey, info.TicketType);
			else if (ticket.User.Email != info.Email)
				throw Error.Uninvited.Throw();

			if (auth.UsedTFAPassword)
				repos.Ticket.ValidateTFA(ticket);

			return ticket.Key;
		}

		private void addPasswordError(String email)
		{
			var user = repos.User.GetByEmail(email);

			if (user == null)
				return;

			var control = user.Control;

			inTransaction("AddPasswordError", () =>
			{
				control.WrongLogin++;

				if (control.WrongPassExceeded())
					control.Active = false;

				repos.Control.SaveOrUpdate(control);
			});

			if (control.WrongPassExceeded())
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

				if (ticket.User.ID != user?.ID)
					throw Error.Uninvited.Throw();

				if (ticket.User.Control.ProcessingDeletion)
					throw Error.UserDeleted.Throw();

				if (ticket.User.Control.WipeRequest != null)
					throw Error.UserAskedWipe.Throw();

				if (ticket.Active)
					repos.Ticket.Disable(ticket);
			});
		}

		internal User VerifyUser()
		{
			var user = GetCurrent();
			VerifyUser(user);

			if (!parent.Current.IsVerified)
				throw Error.TFANotVerified.Throw();

			return user;
		}

		internal void VerifyUser(User user)
		{
			if (user == null || !user.Control.ActiveOrAllowedPeriod())
				throw Error.Uninvited.Throw();

			if (user.Control.ProcessingDeletion)
				throw Error.UserDeleted.Throw();

			if (user.Control.WipeRequest != null)
				throw Error.UserAskedWipe.Throw();

			if (!IsLastContractAccepted(user))
				throw Error.NotSignedLastContract.Throw();
		}

		public IList<TicketInfo> ListLogins()
		{
			VerifyUser();

			var user = GetCurrent();
			var currentTicket = parent.Current.TicketKey;
			var tickets = repos.Ticket.List(user);

			return tickets
				.Select(
					t => TicketInfo.Convert(t, currentTicket)
				)
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
				repos.Control.Deactivate(user);
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
			var user = GetCurrent();

			if (user.Control.ProcessingDeletion)
				throw Error.UserDeleted.Throw();

			if (user.Control.WipeRequest != null)
				throw Error.UserAskedWipe.Throw();

			return IsLastContractAccepted(user);
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
			if (user.Control.ProcessingDeletion)
				throw Error.UserDeleted.Throw();

			if (user.Control.WipeRequest != null)
				throw Error.UserAskedWipe.Throw();

			var contract = getContract();
			var acceptedNow = repos.Acceptance.Accept(user, contract);

			if (!acceptedNow) return;

			var control = user.Control;
			repos.Control.ResetWarnCounter(control);
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

				if (user.Control.ProcessingDeletion)
					throw Error.UserDeleted.Throw();

				if (user.Control.WipeRequest != null)
					throw Error.UserAskedWipe.Throw();

				repos.User.SaveTFA(user, info.Secret);
			});
		}

		public void RemoveTFA(String currentPassword)
		{
			inTransaction("RemoveTFA", () =>
			{
				var user = GetCurrent();

				if (user.Control.ProcessingDeletion)
					throw Error.UserDeleted.Throw();

				if (user.Control.WipeRequest != null)
					throw Error.UserAskedWipe.Throw();

				if (!repos.User.VerifyPassword(user, currentPassword))
					throw Error.TFAWrongPassword.Throw();

				repos.User.SaveTFA(user, null);
			});
		}

		public void ValidateTicketTFA(String code)
		{
			inTransaction("ValidateTicketTFA", () =>
			{
				var user = GetCurrent();
				var secret = user.TFASecret;

				if (secret == null)
					throw Error.TFANotConfigured.Throw();

				if (user.Control.ProcessingDeletion)
					throw Error.UserDeleted.Throw();

				if (user.Control.WipeRequest != null)
					throw Error.UserAskedWipe.Throw();

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

			if (ticket.User.Control.ProcessingDeletion)
				throw Error.UserDeleted.Throw();

			if (ticket.User.Control.WipeRequest != null)
				throw Error.UserAskedWipe.Throw();

			return String.IsNullOrEmpty(ticket.User.TFASecret)
				|| ticket.ValidTFA;
		}

		public Boolean VerifyTicketType(TicketType type)
		{
			var ticket = repos.Ticket.GetByKey(parent.Current.TicketKey);

			if (ticket == null)
				throw Error.Uninvited.Throw();

			if (ticket.User.Control.ProcessingDeletion)
				throw Error.UserDeleted.Throw();

			if (ticket.User.Control.WipeRequest != null)
				throw Error.UserAskedWipe.Throw();

			return ticket.Type == type;
		}

		internal User GetCurrent()
		{
			return getUserByTicket(parent.Current.TicketKey);
		}

		public void SaveAccess()
		{
			var key = parent.Current.TicketKey;
			var ticket = repos.Ticket.GetByKey(key);

			if (ticket == null)
				return;

			var user = ticket.User;

			if (user.Control.ProcessingDeletion)
				throw Error.UserDeleted.Throw();

			if (user.Control.WipeRequest != null)
				throw Error.UserAskedWipe.Throw();

			inTransaction("SaveAccess", () =>
			{
				ticket.LastAccess = DateTime.UtcNow;
				repos.Ticket.SaveOrUpdate(ticket);

				repos.Control.SaveAccess(user.Control);
			});
		}

		public void UseTFAAsPassword(Boolean use)
		{
			inTransaction("UseTFAAsPassword", () =>
			{
				var user = GetCurrent();

				if (user.Control.ProcessingDeletion)
					throw Error.UserDeleted.Throw();

				if (user.Control.WipeRequest != null)
					throw Error.UserAskedWipe.Throw();

				repos.User.UseTFAAsPassword(user, use);
			});
		}

		public void AskWipe(String password)
		{
			VerifyUser();

			inTransaction("AskWipe", () =>
			{
				var user = GetCurrent();

				var validPassword = password != null
				                    && Crypt.Check(password, user.Password);

				if (!validPassword)
					throw Error.WrongPassword.Throw();

				repos.Control.RequestWipe(user);
			});
		}

		public void ReMisc(String password)
		{
			VerifyUser();

			inTransaction("ReMisc", () =>
			{
				var user = GetCurrent();

				var validPassword = password != null
					&& Crypt.Check(password, user.Password);

				if (!validPassword)
					throw Error.WrongPassword.Throw();

				repos.Control.ReMisc(user);
			});
		}
	}
}
