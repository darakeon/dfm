using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DFM.BusinessLogic.Exceptions;
using DFM.BusinessLogic.Response;
using DFM.Entities.Enums;

namespace DFM.MVC.Models
{
	public class TokensPasswordResetModel : BaseSiteModel
	{
		[Required(ErrorMessage = "*")]
		public String Password
		{
			get => info.Password;
			set => info.Password = value;
		}

		[Required(ErrorMessage = "*")]
		public String RetypePassword
		{
			get => info.RetypePassword;
			set => info.RetypePassword = value;
		}

		private readonly PasswordResetInfo info =
			new PasswordResetInfo();

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
				info.Token = token;

				safe.ResetPassword(info);
			}
			catch (CoreError e)
			{
				errors.Add(translator[e]);
			}

			return errors;
		}


	}
}
