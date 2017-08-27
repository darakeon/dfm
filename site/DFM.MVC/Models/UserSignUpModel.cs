using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using DFM.BusinessLogic.Exceptions;
using DFM.MVC.Helpers.Global;

namespace DFM.MVC.Models
{
    public class UserSignUpModel : BaseModel
    {
        [Required(ErrorMessage = "*")]
        public String Email { get; set; }

        [Required(ErrorMessage = "*")]
        public String Password { get; set; }

        [Required(ErrorMessage = "*")]
        public String RetypePassword { get; set; }



        internal IList<String> ValidateAndSendVerify(ModelStateDictionary modelState)
        {
            var errors = new List<String>();

            if (Password != RetypePassword)
                errors.Add(MultiLanguage.Dictionary["RetypeWrong"]);

            if (!errors.Any())
            {
                try
                {
                    Safe.SaveUserAndSendVerify(Email, Password, MultiLanguage.Language);
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