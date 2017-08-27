using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Ak.MVC.Forms;
using DFM.BusinessLogic.Exceptions;
using DFM.Entities.Enums;
using DFM.MVC.Helpers;

namespace DFM.MVC.Models
{
    public class TokenIndexModel : BaseModel
    {
        public TokenIndexModel()
        {
            SecurityActionList = SelectListExtension.CreateSelect(
                                    MultiLanguage.GetEnumNames<SecurityAction>());
        }



        private String token;

        [Required(ErrorMessage = "*")]
        public String Token
        {
            get { return token; }
            set { token = (value ?? "").Trim(); }
        }


        [Required(ErrorMessage = "*")]
        public SecurityAction SecurityAction { get; set; }

        public SelectList SecurityActionList { get; set; }


        internal IList<String> Test()
        {
            var errors = new List<String>();

            try
            {
                Safe.TestSecurityToken(Token, SecurityAction);
            }
            catch (DFMCoreException e)
            {
                errors.Add(MultiLanguage.Dictionary[e]);
            }

            if (SecurityAction != SecurityAction.PasswordReset 
                && SecurityAction != SecurityAction.UserVerification)
            {
                errors.Add(MultiLanguage.Dictionary["NotRecognizedAction"]);
            }

            return errors;
        }


    }
}