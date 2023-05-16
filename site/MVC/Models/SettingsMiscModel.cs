using System;
using System.Collections.Generic;
using DFM.BusinessLogic.Exceptions;

namespace DFM.MVC.Models
{
	public class SettingsMiscModel : BaseSiteModel, SettingsModel
	{
		public String BackTo { get; set; }
		public String Password { get; set; }

		public IList<String> Save()
		{
			var errors = new List<String>();

			try
			{
				clip.ReMisc(Password);
				errorAlert.Add("SettingsChanged");
			}
			catch (CoreError e)
			{
				errors.Add(translator[e]);
			}

			return errors;
		}
	}
}
