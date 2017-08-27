using System;
using System.ComponentModel.DataAnnotations;
using DFM.Entities.Enums;

namespace DFM.MVC.Models
{
    public class TokenPasswordResetModel : BaseModel
    {
        [Required(ErrorMessage = "*")]
        public String Password { get; set; }

        [Required(ErrorMessage = "*")]
        public String RetypePassword { get; set; }


        internal void TestSecurityToken(String token)
        {
            Safe.TestSecurityToken(token, SecurityAction.PasswordReset);
        }

        internal void PasswordReset(String token)
        {
            Safe.PasswordReset(token, Password);
        }


    }
}