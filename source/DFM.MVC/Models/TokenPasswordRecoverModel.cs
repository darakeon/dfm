using System;
using System.ComponentModel.DataAnnotations;

namespace DFM.MVC.Models
{
    public class TokenPasswordResetModel
    {
        [Required(ErrorMessage = "*")]
        public String Password { get; set; }

        [Required(ErrorMessage = "*")]
        public String RetypePassword { get; set; }
    }
}