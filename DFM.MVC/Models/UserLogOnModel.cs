using System;
using System.ComponentModel.DataAnnotations;

namespace DFM.MVC.Models
{
    public class UserLogOnModel
    {
        [Required(ErrorMessage = "*")]
        public String Login { get; set; }

        [Required(ErrorMessage = "*")]
        public String Password { get; set; }

        public Boolean RememberMe { get; set; }
    }
}