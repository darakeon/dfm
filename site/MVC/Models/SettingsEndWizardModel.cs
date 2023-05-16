using System;
using DFM.BusinessLogic.Exceptions;

namespace DFM.MVC.Models
{
	public class SettingsEndWizardModel : BaseSiteModel
	{
		public SettingsEndWizardModel()
		{
			try
			{
				clip.EndWizard();
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
