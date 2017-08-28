using System;
using DFM.BusinessLogic.Exceptions;
using DFM.MVC.Helpers.Global;

namespace DFM.MVC.Models
{
	public class AdminModel : BaseModel
	{
		internal void CloseAccount(String url)
		{
			try
			{
				Admin.CloseAccount(url);
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
				Admin.DeleteAccount(url);
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
				Admin.DisableCategory(name);
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
				Admin.EnableCategory(name);
			}
			catch (DFMCoreException e)
			{
				ErrorAlert.Add(e.Type);
			}
		}




	}
}