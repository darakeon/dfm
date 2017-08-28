using System;
using System.ComponentModel.DataAnnotations;
using DFM.BusinessLogic.Exceptions;

namespace DFM.MVC.Models
{
    public class UsersLogOnModel : BaseModel
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
                {
                    e = sendUserVerify() ?? e;
                }

                return e;
            }

            return null;
        }

        private DFMCoreException sendUserVerify()
        {
            try
            {
				var pathAction = Url.Action("UserVerification", "Tokens");
				var pathDisable = Url.Action("Disable", "Tokens");
				Safe.SendUserVerify(Email, pathAction, pathDisable);
            }
            catch (DFMCoreException e)
            {
                return e;
            }

            return null;
        }
    }
}