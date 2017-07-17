using System;
using System.ComponentModel.DataAnnotations;

namespace DFM.MVC.Models
{
    public class UserLogOnModel
    {
        [Required(ErrorMessage = "*")]
        public String Email { get; set; }

        [Required(ErrorMessage = "*")]
        public String Password { get; set; }

        public Boolean RememberMe { get; set; }
    }
}