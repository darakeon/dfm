using System;
using System.ComponentModel.DataAnnotations;
using DFM.BusinessLogic.Exceptions;

namespace DFM.API.Models
{
    internal class UsersLoginModel : BaseApiModel
    {
        [Required(ErrorMessage = "*")]
        public string Email { get; set; }

        [Required(ErrorMessage = "*")]
        public string Password { get; set; }

        public bool RememberMe { get; set; }

        internal string LogOn()
        {
	        try
	        {
		        return current.Set(Email, Password, RememberMe);
	        }
	        catch (CoreError e)
	        {
		        if (e.Type == Error.DisabledUser)
			        outside.SendUserVerify(Email);

		        throw;
	        }
        }
    }
}
