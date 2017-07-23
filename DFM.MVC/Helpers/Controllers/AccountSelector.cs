using System;
using DFM.Entities;
using DFM.Entities.Enums;
using DFM.Repositories;

namespace DFM.MVC.Helpers.Controllers
{
    public class AccountSelector
    {
        public Account AccountIn { get; set; }
        public Account AccountOut { get; set; }

        public AccountSelector(MoveNature nature, Int32 currentAccountID, Int32? chosenAccountID)
        {
            Account currentAccount = null;
            Account chosenAccount = null;

            if (currentAccountID != 0)
                currentAccount = Services.Admin.SelectAccountById(currentAccountID);

            chosenAccountID = chosenAccountID ?? 0;

            if (chosenAccountID != 0)
                chosenAccount = Services.Admin.SelectAccountById(chosenAccountID.Value);

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