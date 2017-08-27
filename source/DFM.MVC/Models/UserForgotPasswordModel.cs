using System;
using System.ComponentModel.DataAnnotations;

namespace DFM.MVC.Models
{
    public class UserForgotPasswordModel : BaseModel
    {
        [Required(ErrorMessage = "*")]
        public String Email { get; set; }

        internal void SendPasswordReset()
        {
            Safe.SendPasswordReset(Email);
        }


    }
}