using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using DFM.BusinessLogic.Exceptions;
using DFM.Entities;
using DFM.MVC.Helpers.Global;

namespace DFM.MVC.Models
{
    public class UserSignUpModel : BaseModel
    {
        public UserSignUpModel()
        {
            User = new User
            {
                Config = new Config
                {
                    Language = MultiLanguage.Language
                }
            };
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



        internal IList<String> ValidateAndSendVerify(ModelStateDictionary modelState)
        {
            var errors = new List<String>();

            if (Password != RetypePassword)
                errors.Add(MultiLanguage.Dictionary["RetypeWrong"]);

            if (!errors.Any())
            {
                try
                {
                    Safe.SaveUserAndSendVerify(User.Email, User.Password);
                }
                catch (DFMCoreException e)
                {
                    errors.Add(MultiLanguage.Dictionary[e]);
                }
            }

            return errors;
        }



    }
}