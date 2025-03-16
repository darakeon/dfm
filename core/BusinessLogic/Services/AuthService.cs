using System;
using System.Collections.Generic;
using System.Linq;
using DFM.Authentication;
using DFM.BusinessLogic.Exceptions;
using DFM.BusinessLogic.Repositories;
using DFM.BusinessLogic.Response;
using DFM.BusinessLogic.Validators;
using DFM.Entities;
using DFM.Entities.Bases;
using DFM.Entities.Enums;
using DFM.Generic.Datetime;
using DFM.Language;
using Keon.Util.Extensions;

namespace DFM.BusinessLogic.Services
{
	public class AuthService: Service, IAuthService<SignInInfo, SessionInfo>
	{
		internal AuthService(ServiceAccess serviceAccess, Repos repos, Valids valids)
			: base(serviceAccess, repos, valids) { }

		public void SaveUser(SignUpInfo info)
		{
			info.VerifyPassword();

			if (!PlainText.AcceptLanguage(info.Language))
				throw Error.LanguageUnknown.Throw();

			if (!TZ.IsValid(info.TimeZone))
				throw Error.TimeZoneUnknown.Throw();

			var user = info.GetEntity();
			user.Control.MiscDna = Misc.RandomDNA();
			user.Control.Plan = repos.Plan.GetFree();

			inTransaction("SaveUser", () =>
			{
				user = repos.User.Save(user);

				if (info.AcceptedContract)
					parent.Law.AcceptContract(user);

				parent.Outside.SendUserVerify(user);
			});
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

			valids.User.CheckUserDeletion(user);

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

		public SessionInfo GetSession(String ticketKey)
		{
			var user = getUserByTicket(ticketKey);

			valids.User.CheckUserDeletion(user);

			return new(user);
		}

		private User getUserByTicket(String ticketKey, Boolean allowDisabled = false)
		{
			var ticket = repos.Ticket.GetByKey(ticketKey);

			if (ticket is not {Active: true})
				throw Error.Uninvited.Throw();

			var user = ticket.User;

			if (!allowDisabled && !user.Control.ActiveOrAllowedPeriod())
				throw Error.DisabledUser.Throw();

			return user;
		}

		public void DisableTicket(String ticketKey)
		{
			inTransaction("DisableTicket", () =>
			{
				if (ticketKey == null) return;

				var user = GetCurrent(true);

				var ticket = ticketKey.Length == Defaults.TicketShowedPart
					? repos.Ticket.GetByPartOfKey(user, ticketKey)
					: repos.Ticket.GetByKey(ticketKey);

				if (ticket == null) return;

				if (ticket.User.ID != user?.ID)
					throw Error.Uninvited.Throw();

				valids.User.CheckUserDeletion(ticket.User);

				if (ticket.Active)
					repos.Ticket.Disable(ticket);
			});
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

			valids.User.CheckPassword(user, info.CurrentPassword);

			if (user.HasTFA())
				checkTFA(user, info);

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

		private void checkTFA(User user, ITFAForm info)
		{
			checkTFA(user, info.TFACode);
		}

		private void checkTFA(User user, String code)
		{
			inTransaction(
				"ChangePassword_CheckTFA",
				() =>
				{
					valids.User.CheckTFA(user, code);
					repos.Control.ResetWrongTFA(user);
				},
				() => inTransaction(
					"WrongTFA",
					() => repos.Control.WrongTFA(user)
				)
			);
		}

		public void UpdateEmail(UpdateEmailInfo info)
		{
			VerifyUser();

			var user = GetCurrent();

			valids.User.CheckPassword(user, info.Password);

			if (user.HasTFA())
				checkTFA(user, info);

			inTransaction("UpdateEmail", () =>
			{
				user = repos.User.UpdateEmail(user.ID, info.Email);
				repos.Control.Deactivate(user);
				parent.Outside.SendUserVerify(user);
			});
		}

		public void UpdateTFA(TFAInfo info)
		{
			if (String.IsNullOrEmpty(info.Secret))
				throw Error.TFAEmptySecret.Throw();

			valids.User.CheckTFA(info);

			var user = GetCurrent();

			valids.User.CheckPassword(user, info.Password);
			valids.User.CheckUserDeletion(user);
			parent.Law.CheckContractAccepted(user);

			inTransaction("UpdateTFA", () =>
			{
				repos.User.SaveTFA(user, info.Secret);
				repos.Control.UnsetWarning(user);
			});
		}

		public void RemoveTFA(TFACheck info)
		{
			var user = GetCurrent();

			valids.User.CheckUserDeletion(user);
			parent.Law.CheckContractAccepted(user);

			valids.User.CheckPassword(user, info.Password);

			valids.User.CheckTFAConfigured(user);
			checkTFA(user, info);

			inTransaction(
				"RemoveTFA",
				() => repos.User.ClearTFA(user, false)
			);
		}

		public void ValidateTicketTFA(String code)
		{
			var user = GetCurrent();

			valids.User.CheckTFAConfigured(user);
			valids.User.CheckUserDeletion(user);
			checkTFA(user, code);

			inTransaction("ValidateTicketTFA", () =>
			{
				var ticket = repos.Ticket.GetByKey(parent.Current.TicketKey);
				repos.Ticket.ValidateTFA(ticket);
			});
		}

		public Boolean VerifyTicketTFA()
		{
			var ticket = repos.Ticket.GetByKey(parent.Current.TicketKey);

			if (ticket == null)
				throw Error.Uninvited.Throw();

			valids.User.CheckUserDeletion(ticket.User);

			return !ticket.User.HasTFA()
				|| ticket.ValidTFA;
		}

		public Boolean VerifyTicketType(TicketType type)
		{
			var ticket = repos.Ticket.GetByKey(parent.Current.TicketKey);

			if (ticket == null)
				throw Error.Uninvited.Throw();

			valids.User.CheckUserDeletion(ticket.User);

			return ticket.Type == type;
		}

		public void UseTFAAsPassword(Boolean use, TFACheck info)
		{
			var user = GetCurrent();

			valids.User.CheckUserDeletion(user);
			parent.Law.CheckContractAccepted(user);

			valids.User.CheckTFAConfigured(user);

			valids.User.CheckPassword(user, info.Password);
			checkTFA(user, info);

			inTransaction(
				"UseTFAAsPassword",
				() => repos.User.UseTFAAsPassword(user, use)
			);
		}

		public void AskRemoveTFA(String password)
		{
			var user = GetCurrent();
			VerifyUser(user);

			valids.User.CheckPassword(user, password);

			valids.User.CheckTFAConfigured(user);

			inTransaction(
				"AskRemoveTFA",
				() => repos.Security.CreateAndSendToken(user, SecurityAction.RemoveTFA)
			);
		}

		public void RemoveTFAByToken(String token)
		{
			var security = repos.Security.ValidateAndGet(
				token, SecurityAction.RemoveTFA
			);

			parent.Auth.VerifyUser(security.User);

			var currentUserEmail = parent.Current.Email;
			if (currentUserEmail != security.User.Email)
				throw Error.Uninvited.Throw();

			valids.User.CheckTFAConfigured(security.User);

			inTransaction("RemoveTFAByToken", () =>
			{
				repos.User.ClearTFA(security.User, true);
				repos.Control.SetWarning(security.User);
				repos.Security.Disable(token);
			});
		}

		internal User GetCurrent(Boolean allowDisabled = false)
		{
			return getUserByTicket(parent.Current.TicketKey, allowDisabled);
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
			VerifyUserIgnoreContract(user);

			parent.Law.CheckContractAccepted(user);
		}

		internal void VerifyUserIgnoreContract(User user)
		{
			if (user == null || !user.Control.ActiveOrAllowedPeriod())
				throw Error.Uninvited.Throw();

			valids.User.CheckUserDeletion(user);
		}
	}
}
