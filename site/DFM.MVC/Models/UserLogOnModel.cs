using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DFM.BusinessLogic.Exceptions;
using DFM.MVC.Helpers;

namespace DFM.MVC.Models
{
    public class UserLogOnModel : BaseModel
    {
        [Required(ErrorMessage = "*")]
        public String Email { get; set; }

        [Required(ErrorMessage = "*")]
        public String Password { get; set; }

        public Boolean RememberMe { get; set; }



        internal DFMCoreException LogOn()
        {
            try
            {
                Current.Set(Email, Password);
            }
            catch (DFMCoreException e)
            {
                if (e.Type == ExceptionPossibilities.DisabledUser)
                    Safe.SendUserVerify(Email);

                return e;
            }

            return null;
        }



    }
}