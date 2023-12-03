using System;
using System.ComponentModel.DataAnnotations;

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
            return login(Email, Password, RememberMe);
        }
    }
}
