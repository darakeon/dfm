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

		internal void Delete(String url)
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

		internal void Disable(String name)
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

		internal void Enable(String name)
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
