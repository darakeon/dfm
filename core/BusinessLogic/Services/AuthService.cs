﻿using System;
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
			inTransaction("SaveUser", () =>
			{
				info.VerifyPassword();

				if (!PlainText.AcceptLanguage(info.Language))
					throw Error.LanguageUnknown.Throw();

				if (!TZ.IsValid(info.TimeZone))
					throw Error.TimeZoneUnknown.Throw();

				var user = info.GetEntity();
				user.Control.MiscDna = Misc.RandomDNA();
				user.Control.Plan = repos.Plan.GetFree();

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

			checkUserDeletion(user);

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

			checkUserDeletion(user);

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

				checkUserDeletion(ticket.User);

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

			checkPassword(user, info.CurrentPassword);

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

			checkPassword(user, password);

			inTransaction("UpdateEmail", () =>
			{
				user = repos.User.UpdateEmail(user.ID, email);
				repos.Control.Deactivate(user);
				parent.Outside.SendUserVerify(user);
			});
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

				checkPassword(user, info.Password);
				checkUserDeletion(user);
				checkContractAccepted(user);

				repos.User.SaveTFA(user, info.Secret);
			});
		}

		public void RemoveTFA(String currentPassword)
		{
			inTransaction("RemoveTFA", () =>
			{
				var user = GetCurrent();

				checkUserDeletion(user);
				checkContractAccepted(user);

				checkPassword(user, currentPassword);

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

				checkUserDeletion(user);

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

			checkUserDeletion(ticket.User);

			return String.IsNullOrEmpty(ticket.User.TFASecret)
				|| ticket.ValidTFA;
		}

		public Boolean VerifyTicketType(TicketType type)
		{
			var ticket = repos.Ticket.GetByKey(parent.Current.TicketKey);

			if (ticket == null)
				throw Error.Uninvited.Throw();

			checkUserDeletion(ticket.User);

			return ticket.Type == type;
		}

		public void UseTFAAsPassword(Boolean use)
		{
			inTransaction("UseTFAAsPassword", () =>
			{
				var user = GetCurrent();

				checkUserDeletion(user);
				checkContractAccepted(user);

				repos.User.UseTFAAsPassword(user, use);
			});
		}

		public void AskRemoveTFA(String password)
		{
			var user = GetCurrent();
			VerifyUser(user);

			checkPassword(user, password);

			inTransaction(
				"AskRemoveTFA",
				() => repos.Security.CreateAndSendToken(user, SecurityAction.RemoveTFA)
			);
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

			checkContractAccepted(user);
		}

		internal void VerifyUserIgnoreContract(User user)
		{
			if (user == null || !user.Control.ActiveOrAllowedPeriod())
				throw Error.Uninvited.Throw();

			checkUserDeletion(user);
		}

		private void checkPassword(User user, String password)
		{
			if (!repos.User.VerifyPassword(user, password))
				throw Error.WrongPassword.Throw();
		}

		private static void checkUserDeletion(User user)
		{
			if (user.Control.ProcessingDeletion)
				throw Error.UserDeleted.Throw();

			if (user.Control.WipeRequest != null)
				throw Error.UserAskedWipe.Throw();
		}

		private void checkContractAccepted(User user)
		{
			if (!parent.Law.IsLastContractAccepted(user))
				throw Error.NotSignedLastContract.Throw();
		}
	}
}
