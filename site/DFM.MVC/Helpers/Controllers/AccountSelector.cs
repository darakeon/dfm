using System;
using DFM.Entities.Enums;

namespace DFM.MVC.Helpers.Controllers
{
    public class AccountSelector
    {
        public String AccountInUrl { get; set; }
        public String AccountOutUrl { get; set; }

        public AccountSelector(MoveNature nature, String currentAccountUrl, String chosenAccountUrl)
        {
            switch (nature)
            {
                case MoveNature.In:
                    AccountInUrl = currentAccountUrl; 
                    break;
                
                case MoveNature.Out:
                    AccountOutUrl = currentAccountUrl; 
                    break;

                case MoveNature.Transfer:
                    AccountOutUrl = currentAccountUrl;
                    AccountInUrl = chosenAccountUrl;
                    break;
            }
        }

    }
}