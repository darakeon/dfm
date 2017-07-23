using System;
using System.ComponentModel.DataAnnotations;
using DFM.Entities;
using DFM.Generic;

namespace DFM.MVC.Models
{
    public class AccountCreateEditModel : BaseLoggedModel
    {
        public AccountCreateEditModel()
        {
            Account = new Account();
        }

        public AccountCreateEditModel(OperationType type) : this()
        {
            Type = type;
        }



        public OperationType Type { get; set; }

        public Account Account { get; set; }


        private String name;

        [Required(ErrorMessage = "*")]
        public String Name
        {
            get
            {
                switch (Type)
                {
                    case OperationType.Creation:
                        return Account.Name;
                    case OperationType.Update:
                        return name ?? Account.Name;
                    default:
                        throw new NotImplementedException();
                }
            }
            set
            {
                switch (Type)
                {
                    case OperationType.Creation:
                        Account.Name = value;
                        break;
                    case OperationType.Update:
                        name = value;
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }
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