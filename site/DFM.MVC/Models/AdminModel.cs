using System;
using DFM.BusinessLogic.Exceptions;
using DFM.MVC.Helpers.Global;

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
			catch (DFMCoreException e)
			{
				ErrorAlert.Add(e.Type);
			}
		}



		internal void Delete(String url)
		{
			try
			{
				admin.DeleteAccount(url);
			}
			catch (DFMCoreException e)
			{
				ErrorAlert.Add(e.Type);
			}
		}


		internal void Disable(String name)
		{
			try
			{
				admin.DisableCategory(name);
			}
			catch (DFMCoreException e)
			{
				ErrorAlert.Add(e.Type);
			}
		}



		internal void Enable(String name)
		{
			try
			{
				admin.EnableCategory(name);
			}
			catch (DFMCoreException e)
			{
				ErrorAlert.Add(e.Type);
			}
		}




	}
}