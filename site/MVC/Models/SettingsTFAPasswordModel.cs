using System;
using System.Collections.Generic;
using DFM.BusinessLogic.Exceptions;
using DFM.BusinessLogic.Response;

namespace DFM.MVC.Models
{
	public class SettingsTFAPasswordModel : BaseSiteModel, SettingsModel
	{
		public Boolean Use => !current.TFAPassword;

		public TFACheck TFA { get; set; }

		public String BackTo => null;

		public IList<String> Save()
		{
			var errors = new List<String>();

			try
			{
				auth.UseTFAAsPassword(Use, TFA);
				errorAlert.Add("");
			}
			catch (CoreError e)
			{
				errors.Add(translator[e]);
			}

			return errors;
		}
	}
}
