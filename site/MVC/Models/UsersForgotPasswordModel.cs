using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DFM.BusinessLogic.Exceptions;

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
				outside.SendPasswordReset(Email);
			}
			catch (CoreError e)
			{
				errors.Add(translator[e]);
			}

			return errors;
		}


	}
}
