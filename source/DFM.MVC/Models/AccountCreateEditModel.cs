using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DFM.BusinessLogic.Exceptions;
using DFM.Entities;
using DFM.Generic;
using DFM.MVC.Helpers;

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

        public AccountCreateEditModel(OperationType type, String id) : this(type)
        {
            Account = Admin.GetAccountByName(id);
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
                    case OperationType.Edit:
                        return name ?? Account.Name;
                    default:
                        throw new NotImplementedException();
                }
            }
            set
            {
                name = value;

                if (Type == OperationType.Creation)
                    Account.Name = value;
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

        

        internal void ResetAccountName(OperationType type, string id)
        {
            var oldAccount = Admin.GetAccountByName(id);

            Type = type;
            Account.Name = oldAccount.Name;
        }

        

        internal IList<String> CreateOrUpdate()
        {
            var errors = new List<String>();

            try
            {
                Account.User = Current.User;

                if (Type == OperationType.Creation)
                    Admin.CreateAccount(Account);
                else
                    Admin.UpdateAccount(Account, Name);
            }
            catch (DFMCoreException e)
            {
                errors.Add(MultiLanguage.Dictionary[e]);
            }

            return errors;
        }



    }
}