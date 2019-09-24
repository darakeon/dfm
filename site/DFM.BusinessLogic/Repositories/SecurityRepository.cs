using System;
using System.Collections.Generic;
using Keon.Util.Extensions;
using DFM.Email;
using DFM.Entities;
using DFM.Entities.Enums;
using DFM.BusinessLogic.Exceptions;
using DFM.BusinessLogic.Helpers;
using Keon.NHibernate.Base;
using Error = DFM.BusinessLogic.Exceptions.Error;

namespace DFM.BusinessLogic.Repositories
{
	internal class SecurityRepository : BaseRepositoryLong<Security>
	{
		internal Security Save(Security security)
		{
			return SaveOrUpdate(security, complete);
		}

		private static void complete(Security security)
		{
			if (security.ID != 0) return;

			security.Active = true;
			security.Expire = security.User.Now().AddMonths(1);
			security.CreateToken();
		}



		internal void SendEmail(Security security, String pathAction, String pathDisable)
		{
			var dic = new Dictionary<String, String>
			{
				{ "Url", Site.Url },
				{ "Token", security.Token },
				{ "Date", security.Expire.AddDays(-1).ToShortDateString() },
				{ "PathAction", pathAction },
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
			Save(security);
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

			Save(security);
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
