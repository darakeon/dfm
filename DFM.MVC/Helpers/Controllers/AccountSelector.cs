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
            var currentAccount = Services.Admin.SelectAccountById(currentAccountID);
            var chosenAccount = Services.Admin.SelectAccountById(chosenAccountID ?? 0);

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