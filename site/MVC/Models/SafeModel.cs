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
				outside.DisableToken(token);
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
				outside.TestSecurityToken(token, SecurityAction.UserVerification);
			}
			catch (CoreError)
			{
				return false;
			}

			outside.ActivateUser(token);

			return true;
		}

		internal Boolean DeleteCsvData(String token)
		{
			try
			{
				outside.TestSecurityToken(token, SecurityAction.DeleteCsvData);
			}
			catch (CoreError)
			{
				return false;
			}

			outside.WipeCsv(token);

			return true;
		}

		internal void LogOff()
		{
			logout();
		}

		internal void DisableLogin(String key)
		{
			clip.DisableTip(TipBrowser.DeleteLogins);
			auth.DisableTicket(key);
		}
	}
}
