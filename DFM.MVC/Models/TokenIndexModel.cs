using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Ak.MVC.Forms;
using DFM.Entities.Enums;
using DFM.MVC.Helpers;

namespace DFM.MVC.Models
{
    public class TokenIndexModel
    {
        public TokenIndexModel()
        {
            SecurityActionList = SelectListExtension.CreateSelect(
                                    MultiLanguage.GetEnumNames<SecurityAction>());
        }

        [Required(ErrorMessage = "*")]
        public String Token { get; set; }

        [Required(ErrorMessage = "*")]
        public SecurityAction SecurityAction { get; set; }

        public SelectList SecurityActionList { get; set; }

    }
}