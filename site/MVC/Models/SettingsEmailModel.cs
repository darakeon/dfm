using System;
using System.Collections.Generic;
using DFM.BusinessLogic.Exceptions;
using DFM.BusinessLogic.Response;

namespace DFM.MVC.Models
{
	public class SettingsEmailModel : BaseSiteModel, SettingsModel
	{
		public String BackTo { get; set; }

		public UpdateEmailInfo Info { get; set; } = new();

		public IList<String> Save()
		{
			var errors = new List<String>();

			try
			{
				auth.UpdateEmail(Info);

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
