using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using DFM.BusinessLogic.Exceptions;
using DFM.Entities.Enums;
using DFM.MVC.Helpers.Global;

namespace DFM.MVC.Models
{
    public class TokensPasswordResetModel : BaseModel
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

            if (Password != RetypePassword)
            {
                errors.Add(MultiLanguage.Dictionary["RetypeWrong"]);
            }

            if (!errors.Any())
            {
                try
                {
                    Safe.PasswordReset(token, Password);
                }
                catch (DFMCoreException e)
                {
                    errors.Add(MultiLanguage.Dictionary[e]);
                }
            }

            return errors;
        }


    }
}