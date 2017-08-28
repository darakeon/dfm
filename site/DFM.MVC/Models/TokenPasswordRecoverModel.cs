using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using DFM.BusinessLogic.Exceptions;
using DFM.BusinessLogic.ObjectInterfaces;
using DFM.Entities.Enums;
using DFM.MVC.Helpers.Global;

namespace DFM.MVC.Models
{
	public class TokensPasswordResetModel : BaseModel, IPasswordForm
	{
		[Required(ErrorMessage = "*")]
		public String Password { get; set; }

		[Required(ErrorMessage = "*")]
		public String RetypePassword { get; set; }


		internal Boolean TestToken(String token)
		{
			try
			{
				Safe.TestSecurityToken(token, SecurityAction.PasswordReset);
			}
			catch (DFMCoreException)
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
				Safe.PasswordReset(token, this);
			}
			catch (DFMCoreException e)
			{
				errors.Add(MultiLanguage.Dictionary[e]);
			}

			return errors;
		}


	}
}