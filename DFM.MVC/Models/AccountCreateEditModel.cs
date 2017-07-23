using System;
using System.ComponentModel.DataAnnotations;
using DFM.BusinessLogic.Helpers;
using DFM.Entities;

namespace DFM.MVC.Models
{
    public class AccountCreateEditModel : BaseLoggedModel
    {
        public AccountCreateEditModel() { }

        public AccountCreateEditModel(OperationType type = OperationType.Creation) : this()
        {
            Type = type;

            Account = new Account();
        }



        public OperationType Type { get; set; }

        public Account Account { get; set; }

        [Required(ErrorMessage = "*")]
        public String Name
        {
            get { return Account.Name; }
            set { Account.Name = value; }
        }

        [Required(ErrorMessage = "*")]
        public String NewName { get; set; }


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