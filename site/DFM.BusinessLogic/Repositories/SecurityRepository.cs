using System;
using System.Collections.Generic;
using DK.Generic.Extensions;
using DFM.Email;
using DFM.Entities;
using DFM.Entities.Enums;
using DFM.BusinessLogic.Exceptions;
using DFM.BusinessLogic.Helpers;
using ExceptionPossibilities = DFM.BusinessLogic.Exceptions.ExceptionPossibilities;

namespace DFM.BusinessLogic.Repositories
{
	internal class SecurityRepository : BaseRepository<Security>
	{
		internal Security SaveOrUpdate(Security security)
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
				{ "Url", Dfm.Url },
				{ "Token", security.Token },
				{ "Date", security.Expire.AddDays(-1).ToShortDateString() },
				{ "PathAction", pathAction },
				{ "PathDisable", pathDisable },
			};

			var format = Format.SecurityAction(security.User.Config.Language, security.Action);
			var fileContent = format.Layout.Format(dic);

			var sender = new Sender()
				.To(security.User.Email)
				.Subject(format.Subject)
				.Body(fileContent);

			try
			{
				sender.Send();
			}
			catch (DFMEmailException)
			{
				DFMCoreException.WithMessage(ExceptionPossibilities.FailOnEmailSend);
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


		
		internal Boolean TokenExist(String token)
		{
			return GetByToken(token) != null;
		}



		internal void Disable(String token)
		{
			var security = GetByToken(token);

			if (security == null)
				throw DFMCoreException.WithMessage(ExceptionPossibilities.InvalidToken);

			security.Active = false;

			SaveOrUpdate(security);
		}




		internal Security ValidateAndGet(String token, SecurityAction securityAction)
		{
			var securityToken = GetByToken(token);

			if (securityToken == null || securityToken.Action != securityAction)
				throw DFMCoreException.WithMessage(ExceptionPossibilities.InvalidToken);

			return securityToken;
		}

	}
}
