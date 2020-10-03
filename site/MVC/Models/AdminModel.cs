using System;
using DFM.BusinessLogic.Exceptions;

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
	}
}
