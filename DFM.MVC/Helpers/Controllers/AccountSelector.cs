using System;
using DFM.Core;
using DFM.Entities;
using DFM.Core.Enums;

namespace DFM.MVC.Helpers.Controllers
{
    public class AccountSelector
    {
        public Account AccountIn { get; set; }
        public Account AccountOut { get; set; }

        public AccountSelector(MoveNature nature, Int32 currentAccountID, Int32? chosenAccountID)
        {
            var currentAccount = Service.Access.Account.SelectById(currentAccountID);
            var chosenAccount = Service.Access.Account.SelectById(chosenAccountID ?? 0);

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