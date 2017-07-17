using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Ak.MVC.Forms;
using DFM.Core.Entities;
using DFM.Core.Enums;
using DFM.MVC.MultiLanguage;

namespace DFM.MVC.Models
{
    public class AccountCreateEditModel : BaseLoggedModel
    {
        public AccountCreateEditModel()
        {
            Account = new Account();

            NatureSelectList = 
                SelectListExtension.CreateSelect(PlainText.GetEnumNames<AccountNature>());
        }



        public Account Account { get; set; }

        [Required(ErrorMessage = "*")]
        public String Name
        {
            get { return Account.Name; } 
            set { Account.Name = value; }
        }


        [Required(ErrorMessage = "*")]
        public AccountNature Nature
        {
            get { return Account.Nature; } 
            set { Account.Nature = value; }
        }
        public SelectList NatureSelectList { get; set; }
    }
}