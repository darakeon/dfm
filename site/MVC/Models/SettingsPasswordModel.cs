using System;
using System.Collections.Generic;
using DFM.BusinessLogic.Exceptions;
using DFM.BusinessLogic.Response;

namespace DFM.MVC.Models
{
	public class SettingsPasswordModel : BaseSiteModel, SettingsModel
	{
		public String CurrentPassword { get; set; }

		public ChangePasswordInfo Password { get; set; }

		public IList<String> Save()
		{
			var errors = new List<String>();

			try
			{
				auth.ChangePassword(Password);
				errorAlert.Add("PasswordChanged");
			}
			catch (CoreError e)
			{
				errors.Add(translator[e]);
			}

			return errors;
		}

		public String BackTo { get; set; }
	}
}
