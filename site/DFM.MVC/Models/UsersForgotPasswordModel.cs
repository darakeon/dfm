using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DFM.BusinessLogic.Exceptions;
using DFM.MVC.Helpers.Global;

namespace DFM.MVC.Models
{
    public class UsersForgotPasswordModel : BaseModel
    {
        [Required(ErrorMessage = "*")]
        public String Email { get; set; }

        internal IList<String> SendPasswordReset()
        {
            var errors = new List<String>();

            try
            {
				var pathAction = Url.Action("PasswordReset", "Tokens");
				var pathDisable = Url.Action("Disable", "Tokens");
				Safe.SendPasswordReset(Email, pathAction, pathDisable);
            }
            catch (DFMCoreException e)
            {
                errors.Add(MultiLanguage.Dictionary[e]);
            }

            return errors;
        }


    }
}