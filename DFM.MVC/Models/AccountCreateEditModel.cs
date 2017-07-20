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


        public Boolean HasLimit
        {
            get { return Account.RedLimit != null || Account.YellowLimit != null; }
            set { setLimit(value); }
        }


        private void setLimit(Boolean hasLimit)
        {
            if (hasLimit)
            {
                if (Account.RedLimit == null)
                    Account.RedLimit = 0;
                    
                if (Account.YellowLimit == null)
                    Account.YellowLimit = 0;
            }
            else
            {
                Account.RedLimit = null;
                Account.YellowLimit = null;
            }
        }

    }
}