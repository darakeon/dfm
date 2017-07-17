using System;
using System.ComponentModel.DataAnnotations;
using DFM.Core.Entities;

namespace DFM.MVC.Models
{
    public class AccountCreateEditModel : BaseLoggedModel
    {
        public AccountCreateEditModel()
        {
            Account = new Account();
        }



        public Account Account { get; set; }

        [Required(ErrorMessage = "*")]
        public String Name
        {
            get { return Account.Name; } 
            set { Account.Name = value; }
        }

    }
}