using System;
using DFM.BusinessLogic.Exceptions;
using DFM.MVC.Helpers.Global;

namespace DFM.MVC.Models
{
	public class UsersEndWizard : BaseSiteModel
	{
		public UsersEndWizard()
		{
			try
			{
				admin.EndWizard();
			}
			catch (DFMCoreException e)
			{
				Error = Translator.Dictionary[e];
			}
		}

		public String Error { get; }
		public Boolean HasError => !String.IsNullOrEmpty(Error);
	}
}
