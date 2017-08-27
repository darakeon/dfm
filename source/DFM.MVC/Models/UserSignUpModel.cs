using System;
using System.ComponentModel.DataAnnotations;
using DFM.Entities;
using DFM.MVC.Helpers;

namespace DFM.MVC.Models
{
    public class UserSignUpModel : BaseModel
    {
        public UserSignUpModel()
        {
            User = new User { Language = MultiLanguage.Language };
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



        internal void SaveUserAndSendVerify()
        {
            Safe.SaveUserAndSendVerify(User.Email, User.Password);
        }


    }
}