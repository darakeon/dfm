using System;
using System.ComponentModel.DataAnnotations;
using DFM.Entities;
using DFM.MVC.MultiLanguage;

namespace DFM.MVC.Models
{
    public class UserSignUpModel
    {
        public UserSignUpModel()
        {
            User = new User { Language = PlainText.Language };
        }

        public User User { get; set; }

        [Required(ErrorMessage = "*")]
        public String Email
        {
            get { return User.Email; }
            set { User.Email = value; }
        }

        [Required(ErrorMessage = "*")]
        public String Password
        {
            get { return User.Password; }
            set { User.Password = value; }
        }

        [Required(ErrorMessage = "*")]
        public String RetypePassword { get; set; }
    }
}