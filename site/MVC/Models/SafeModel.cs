using System;
using DFM.BusinessLogic.Exceptions;
using DFM.Entities.Enums;

namespace DFM.MVC.Models
{
	public class SafeModel : BaseSiteModel
	{
		internal Boolean Disable(String token)
		{
			try
			{
				safe.DisableToken(token);
			}
			catch (CoreError)
			{
				return false;
			}

			return true;
		}

		internal Boolean TestAndActivate(String token)
		{
			try
			{
				safe.TestSecurityToken(token, SecurityAction.UserVerification);
			}
			catch (CoreError)
			{
				return false;
			}

			safe.ActivateUser(token);

			return true;
		}

		internal void LogOff()
		{
			logout();
		}

		internal void DisableLogin(String key)
		{
			safe.DisableTicket(key);
		}
	}
}
