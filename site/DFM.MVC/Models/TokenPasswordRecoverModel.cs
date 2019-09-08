using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DFM.BusinessLogic.Exceptions;
using DFM.BusinessLogic.InterfacesAndBases;
using DFM.Entities.Enums;
using DFM.MVC.Helpers.Global;

namespace DFM.MVC.Models
{
	public class TokensPasswordResetModel : BaseSiteModel, IPasswordForm
	{
		[Required(ErrorMessage = "*")]
		public String Password { get; set; }

		[Required(ErrorMessage = "*")]
		public String RetypePassword { get; set; }


		internal Boolean TestToken(String token)
		{
			try
			{
				safe.TestSecurityToken(token, SecurityAction.PasswordReset);
			}
			catch (CoreError)
			{
				return false;
			}

			return true;
		}

		internal IList<String> PasswordReset(String token)
		{
			var errors = new List<String>();

			try
			{
				safe.PasswordReset(token, this);
			}
			catch (CoreError e)
			{
				errors.Add(Translator.Dictionary[e]);
			}

			return errors;
		}


	}
}