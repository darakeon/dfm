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
		private readonly Func<PathType, String> getPath;

		public SecurityRepository(Current.GetUrl getUrl, Func<PathType, String> getPath)
		{
			this.getUrl = getUrl;
			this.getPath = getPath;
		}

		internal void CreateAndSendToken(User user, SecurityAction action, PathType path)
		{
			var security = Create(user, action, path);

			sendEmail(security);

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

		internal Security Create(User user, SecurityAction action, PathType type)
		{
			var security = new Security
			{
				Action = action,
				Active = true,
				Expire = user.Now().AddMonths(1),
				User = user,
				Path = getPath(type),
			};

			security.CreateToken();

			return SaveOrUpdate(security);
		}

		private void sendEmail(Security security)
		{
			var pathDisable = getPath(PathType.DisableToken);

			var dic = new Dictionary<String, String>
			{
				{ "Url", getUrl() },
				{ "Token", security.Token },
				{ "Date", security.Expire.AddDays(-1).ToShortDateString() },
				{ "PathAction", security.Path },
				{ "PathDisable", pathDisable },
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

		internal Security ValidateAndGet(String token, SecurityAction securityAction)
		{
			var securityToken = GetByToken(token);

			if (securityToken == null || securityToken.Action != securityAction)
				throw Error.InvalidToken.Throw();

			return securityToken;
		}

	}
}
