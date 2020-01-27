using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Keon.MVC.Forms;
using DFM.BusinessLogic.Exceptions;
using DFM.Entities.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DFM.MVC.Models
{
	public class TokensIndexModel : BaseSiteModel
	{
		public TokensIndexModel()
		{
			SecurityActionList =
				SelectListExtension.CreateSelect(
					translator.GetEnumNames<SecurityAction>()
				);
		}

		private String token;

		[Required(ErrorMessage = "*")]
		public String Token
		{
			get => token;
			set => token = (value ?? "").Trim();
		}


		[Required(ErrorMessage = "*")]
		public SecurityAction SecurityAction { get; set; }

		public SelectList SecurityActionList { get; set; }


		internal IList<String> Test()
		{
			var errors = new List<String>();

			try
			{
				safe.TestSecurityToken(Token, SecurityAction);
			}
			catch (CoreError e)
			{
				errors.Add(translator[e]);
			}

			if (SecurityAction != SecurityAction.PasswordReset
				&& SecurityAction != SecurityAction.UserVerification)
			{
				errors.Add(translator["NotRecognizedAction"]);
			}

			return errors;
		}


	}
}
