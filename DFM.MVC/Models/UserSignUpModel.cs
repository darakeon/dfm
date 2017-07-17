using System;
using System.ComponentModel.DataAnnotations;
using DFM.Core.Entities;

namespace DFM.MVC.Models
{
    public class UserSignUpModel
    {
        public UserSignUpModel()
        {
            User = new User();
        }

        public User User { get; set; }

        [Required(ErrorMessage = "*")]
        public String Login
        {
            get { return User.Login; }
            set { User.Login = value; }
        }

        [Required(ErrorMessage = "*")]
        public String Password
        {
            get { return User.Password; }
            set { User.Password = value; }
        }

        [Required(ErrorMessage = "*")]
        public String Email
        {
            get { return User.Email; }
            set { User.Email = value; }
        }
    }
}