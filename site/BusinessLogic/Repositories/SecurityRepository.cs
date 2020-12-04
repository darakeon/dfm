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
			var security = Create(user, action);
			sendEmail(security);
			disableOthers(security);
		}

		internal Security Create(User user, SecurityAction action)
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

			var config = security.User.Config;

			var format = Format.SecurityAction(config.Language, config.Theme.Simplify(), security.Action);
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
				s => s.ID != security.ID
				     && s.User.ID == security.User.ID
				     && s.Active
			);

			foreach (var other in others)
			{
				other.Active = false;
				SaveOrUpdate(other);
			}
		}

		internal Security GetByToken(String token)
		{
			var security = SingleOrDefault(s => s.Token == token);

			return security != null
					&& security.Active
					&& security.Expire >= security.User.Now()
				? security
				: null;
		}

		internal void Disable(String token)
		{
			var security = GetByToken(token);

			if (security == null)
				throw Error.InvalidToken.Throw();

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
