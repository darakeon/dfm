using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using DFM.BusinessLogic.Exceptions;
using DFM.BusinessLogic.InterfacesAndBases;
using DFM.Entities;
using DFM.MVC.Helpers.Global;

namespace DFM.MVC.Models
{
	public class UsersSignUpModel : BaseSiteModel, IPasswordForm
	{
		public UsersSignUpModel()
		{
			Contract = safe.GetContract();
			EnableWizard = true;
		}

		[Required(ErrorMessage = "*")]
		public String Email { get; set; }

		[Required(ErrorMessage = "*")]
		public String Password { get; set; }

		[Required(ErrorMessage = "*")]
		public String RetypePassword { get; set; }

		[RegularExpression("True", ErrorMessage = "*")]
		public Boolean Accept { get; set; }

		public Boolean EnableWizard { get; set; }

		public Contract Contract { get; }



		internal IList<String> ValidateAndSendVerify(ModelStateDictionary modelState)
		{
			var errors = new List<String>();

			try
			{
				safe.SaveUserAndSendVerify(Email, this, Accept, EnableWizard, MultiLanguage.Language);
			}
			catch (DFMCoreException e)
			{
				errors.Add(MultiLanguage.Dictionary[e]);
			}

			return errors;
		}


	}
}