using System;
using DFM.BusinessLogic.Exceptions;
using DFM.Entities.Enums;

namespace DFM.MVC.Models
{
	public class AdminModel : BaseSiteModel
	{
		internal void CloseAccount(String url)
		{
			try
			{
				admin.CloseAccount(url);
			}
			catch (CoreError e)
			{
				errorAlert.Add(e.Type);
			}
		}

		internal void ReopenAccount(String url)
		{
			try
			{
				admin.ReopenAccount(url);
			}
			catch (CoreError e)
			{
				errorAlert.Add(e.Type);
			}
		}

		internal void DeleteAccount(String url)
		{
			try
			{
				admin.DeleteAccount(url);
			}
			catch (CoreError e)
			{
				errorAlert.Add(e.Type);
			}
		}

		internal void DisableCategory(String name)
		{
			try
			{
				admin.DisableCategory(name);
			}
			catch (CoreError e)
			{
				errorAlert.Add(e.Type);
			}
		}

		internal void EnableCategory(String name)
		{
			try
			{
				admin.EnableCategory(name);
			}
			catch (CoreError e)
			{
				errorAlert.Add(e.Type);
			}
		}

		public Boolean TestAndUnsubscribe(String token)
		{
			try
			{
				outside.TestSecurityToken(token, SecurityAction.UnsubscribeMoveMail);
			}
			catch (CoreError)
			{
				return false;
			}

			outside.UnsubscribeMoveMail(token);

			return true;
		}
	}
}
