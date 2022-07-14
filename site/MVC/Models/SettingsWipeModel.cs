using System;
using System.Collections.Generic;
using DFM.BusinessLogic.Exceptions;

namespace DFM.MVC.Models
{
	public class SettingsWipeModel : BaseSiteModel, SettingsModel
	{
		public String BackTo { get; set; }

		public String Password { get; set; }

		public IList<String> Save()
		{
			var errors = new List<String>();

			try
			{
				safe.AskWipe(Password);
				errorAlert.Add("Wipe_RequestSent");
			}
			catch (CoreError e)
			{
				errors.Add(translator[e]);
			}

			return errors;
		}
	}
}