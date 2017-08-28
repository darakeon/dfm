using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using DFM.BusinessLogic.Exceptions;
using DFM.BusinessLogic.ObjectInterfaces;
using DFM.Entities;
using DFM.MVC.Helpers.Global;

namespace DFM.MVC.Models
{
	public class UsersSignUpModel : BaseModel, IPasswordForm
	{
		public UsersSignUpModel()
		{
			Contract = Safe.GetContract();
		}

		[Required(ErrorMessage = "*")]
		public String Email { get; set; }

		[Required(ErrorMessage = "*")]
		public String Password { get; set; }

		[Required(ErrorMessage = "*")]
		public String RetypePassword { get; set; }

		[RegularExpression("True", ErrorMessage = "*")]
		public Boolean Accept { get; set; }
		
		public Contract Contract { get; private set; }



		internal IList<String> ValidateAndSendVerify(ModelStateDictionary modelState)
		{
			var errors = new List<String>();

			try
			{
				var pathAction = Url.Action("UserVerification", "Tokens");
				var pathDisable = Url.Action("Disable", "Tokens");
				Safe.SaveUserAndSendVerify(Email, this, Accept, MultiLanguage.Language, pathAction, pathDisable);
			}
			catch (DFMCoreException e)
			{
				errors.Add(MultiLanguage.Dictionary[e]);
			}

			return errors;
		}


	}
}