using System;
using System.Collections.Generic;
using Keon.Util.Extensions;
using DFM.Email;
using DFM.Entities;
using DFM.Entities.Enums;
using DFM.BusinessLogic.Exceptions;
using DFM.BusinessLogic.Helpers;
using Error = DFM.BusinessLogic.Exceptions.Error;

namespace DFM.BusinessLogic.Repositories
{
	internal class SecurityRepository : Repo<Security>
	{
		private readonly Current.GetUrl getUrl;

		public SecurityRepository(Current.GetUrl getUrl)
		{
			this.getUrl = getUrl;
		}

		internal void CreateAndSendToken(User user, SecurityAction action)
		{
			var security = create(user, action);
			sendEmail(security);
			disableOthers(security);
		}

		internal Security Grab(User user, SecurityAction action)
		{
			return NewQuery().Where(
					s => s.User.ID == user.ID
						&& s.Action == action
						&& s.Active
						&& s.Expire >= user.Now()
				).FirstOrDefault ?? create(user, action);
		}

		private Security create(User user, SecurityAction action)
		{
			var security = new Security
			{
				Action = action,
				Active = true,
				Expire = user.Now().AddDays(3),
				User = user,
			};

			security.CreateToken();

			return SaveOrUpdate(security);
		}

		private void sendEmail(Security security)
		{
			var dic = new Dictionary<String, String>
			{
				{ "Url", getUrl() },
				{ "Token", security.Token },
				{ "Date", security.Expire.AddDays(-1).ToShortDateString() },
				{ "PathAction", security.Action.ToString() },
				{ "PathDisable", PathType.DisableToken.ToString() },
			};

			var format = Format.SecurityAction(security.User, security.Action);
			var fileContent = format.Layout.Format(dic);

			var sender = new Sender()
				.To(security.User.Email)
				.Subject(format.Subject)
				.Body(fileContent);

			try
			{
				sender.Send();
			}
			catch (MailError)
			{
				throw Error.FailOnEmailSend.Throw();
			}

			security.Sent = true;
			SaveOrUpdate(security);
		}

		private void disableOthers(Security security)
		{
			var others = Where(
				security.User != null
					? s => s.ID != security.ID
						&& s.User.ID == security.User.ID
						&& s.Active
					: s => s.ID != security.ID
						&& s.Wipe.ID == security.Wipe.ID
						&& s.Active
			);

			foreach (var other in others)
			{
				other.Active = false;
				SaveOrUpdate(other);
			}
		}

		internal Security Create(Wipe wipe)
		{
			var security = create(wipe);
			disableOthers(security);
			return security;
		}

		private Security create(Wipe wipe)
		{
			var security = new Security
			{
				Action = SecurityAction.DeleteCsvData,
				Active = true,
				Expire = DateTime.Now.AddDays(7),
				Wipe = wipe,
			};

			security.CreateToken();

			return SaveOrUpdate(security);
		}

		internal Security GetByToken(String token)
		{
			var security = SingleOrDefault(s => s.Token == token);

			var canBeUsed = security is {Active: true}
				&& security.Expire >= security.User.Now();

			return canBeUsed ? security : null;
		}

		internal void Disable(String token)
		{
			var security = GetByToken(token);

			if (security == null)
				throw Error.InvalidToken.Throw();

			if (security.User.Control.ProcessingDeletion)
				throw Error.UserDeleted.Throw();

			if (security.User.Control.WipeRequest != null)
				throw Error.UserAskedWipe.Throw();

			security.Active = false;
			SaveOrUpdate(security);
		}

		internal Security ValidateAndGet(String token, SecurityAction action)
		{
			var security = GetByToken(token);

			if (security == null || security.Action != action)
				throw Error.InvalidToken.Throw();

			return security;
		}

	}
}
