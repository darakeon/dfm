using System;
using DFM.BusinessLogic.Exceptions;

namespace DFM.MVC.Models
{
	public class ConfigsEndWizardModel : BaseSiteModel
	{
		public ConfigsEndWizardModel()
		{
			try
			{
				admin.EndWizard();
			}
			catch (CoreError e)
			{
				Error = translator[e];
			}
		}

		public String Error { get; }
		public Boolean HasError => !String.IsNullOrEmpty(Error);
	}
}
