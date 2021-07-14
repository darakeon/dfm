using System;
using System.Collections.Generic;
using DFM.BusinessLogic.Exceptions;

namespace DFM.MVC.Models
{
	public class ConfigsMiscModel : BaseSiteModel, ConfigsModel
	{
		public String BackTo { get; set; }
		public String Password { get; set; }

		public IList<String> Save()
		{
			var errors = new List<String>();

			try
			{
				safe.ReMisc(Password);
				errorAlert.Add("ConfigChanged");
			}
			catch (CoreError e)
			{
				errors.Add(translator[e]);
			}

			return errors;
		}
	}
}
