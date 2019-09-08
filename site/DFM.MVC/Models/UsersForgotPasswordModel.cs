using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DFM.BusinessLogic.Exceptions;
using DFM.MVC.Helpers.Global;

namespace DFM.MVC.Models
{
	public class UsersForgotPasswordModel : BaseSiteModel
	{
		[Required(ErrorMessage = "*")]
		public String Email { get; set; }

		internal IList<String> SendPasswordReset()
		{
			var errors = new List<String>();

			try
			{
				safe.SendPasswordReset(Email);
			}
			catch (CoreError e)
			{
				errors.Add(Translator.Dictionary[e]);
			}

			return errors;
		}


	}
}