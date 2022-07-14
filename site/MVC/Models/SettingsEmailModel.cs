using System;
using System.Collections.Generic;
using DFM.BusinessLogic.Exceptions;

namespace DFM.MVC.Models
{
	public class SettingsEmailModel : BaseSiteModel, SettingsModel
	{
		public String BackTo { get; set; }

		public String Email { get; set; }
		public String CurrentPassword { get; set; }

		public IList<String> Save()
		{
			var errors = new List<String>();

			try
			{
				safe.UpdateEmail(CurrentPassword, Email);

				errorAlert.Add("EmailUpdated");
			}
			catch (CoreError e)
			{
				errors.Add(translator[e]);
			}

			return errors;
		}
	}
}
