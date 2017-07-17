using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Ak.MVC.Forms;
using DFM.MVC.Areas.Accounts.Models;
using DFM.Core.Entities;
using DFM.Core.Enums;

namespace DFM.MVC.Models
{
    public class AccountCreateModel : BaseLoggedModel
    {
        public AccountCreateModel()
        {
            Account = new Account();
            NatureSelectList = SelectListExtension.CreateSelect<AccountNature>();
        }


        public Account Account { get; set; }

        [Required(ErrorMessage = "Mandatory Field")]
        public String Name
        {
            get { return Account.Name; }
            set { Account.Name = value; }
        }


        [Required(ErrorMessage = "Mandatory Field")]
        public AccountNature Nature
        {
            get { return Account.Nature; }
            set { Account.Nature = value; }
        }

        public SelectList NatureSelectList { get; set; }
    }
}