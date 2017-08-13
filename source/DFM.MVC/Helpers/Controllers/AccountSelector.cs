using System;
using DFM.Entities.Enums;

namespace DFM.MVC.Helpers.Controllers
{
    public class AccountSelector
    {
        public String AccountInName { get; set; }
        public String AccountOutName { get; set; }

        public AccountSelector(MoveNature nature, String currentAccountName, String chosenAccountName)
        {
            switch (nature)
            {
                case MoveNature.In:
                    AccountInName = currentAccountName; 
                    break;
                
                case MoveNature.Out:
                    AccountOutName = currentAccountName; 
                    break;

                case MoveNature.Transfer:
                    AccountOutName = currentAccountName;
                    AccountInName = chosenAccountName;
                    break;
            }
        }

    }
}