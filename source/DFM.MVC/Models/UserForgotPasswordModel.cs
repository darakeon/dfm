using System;
using System.ComponentModel.DataAnnotations;

namespace DFM.MVC.Models
{
    public class UserForgotPasswordModel
    {
        [Required(ErrorMessage = "*")]
        public String Email { get; set; }
    }
}