using System;
using DFM.Entities;
using DFM.Entities.Enums;
using DFM.Repositories;
using DFM.MVC.Authentication;
using DFM.BusinessLogic.Exceptions;

namespace DFM.MVC.Helpers.Controllers
{
    public class AccountSelector
    {
        public Account AccountIn { get; set; }
        public Account AccountOut { get; set; }

        public AccountSelector(MoveNature nature, String currentAccountName, String chosenAccountName)
        {
            Account currentAccount = null;
            Account chosenAccount = null;

            if (!String.IsNullOrEmpty(currentAccountName))
            {
                currentAccount = Services.Admin.SelectAccountByName(currentAccountName, Current.User);

                //TODO: this shouldn't be here
                if (currentAccount == null)
                    throw DFMCoreException.WithMessage(ExceptionPossibilities.InvalidAccount);
            }

            if (!String.IsNullOrEmpty(chosenAccountName))
            {
                chosenAccount = Services.Admin.SelectAccountByName(chosenAccountName, Current.User);

                //TODO: this shouldn't be here
                if (chosenAccount == null)
                    throw DFMCoreException.WithMessage(ExceptionPossibilities.InvalidAccount);
            }

            switch (nature)
            {
                case MoveNature.In:
                    AccountIn = currentAccount; 
                    break;
                
                case MoveNature.Out:
                    AccountOut = currentAccount; 
                    break;

                case MoveNature.Transfer:
                    AccountOut = currentAccount;
                    AccountIn = chosenAccount;
                    break;
            }
        }

    }
}