using System;
using DFM.BusinessLogic.Exceptions;

namespace DFM.MVC.Models
{
    public class AdminModel : BaseModel
    {
        internal void CloseAccount(String name)
        {
            try
            {
                Admin.CloseAccount(name);
            }
            catch (DFMCoreException)// e)
            {
                //message = MultiLanguage.Dictionary[e];
            }

            //else
            //    account = null;

            //message = account == null
            //    ? MultiLanguage.Dictionary["AccountNotFound"]
            //    : String.Format(MultiLanguage.Dictionary["AccountClosed"], account.Name);
        }


        
        internal void Delete(String name)
        {
            // TODO: implement messages on page head
            //String message;

            try
            {
                Admin.DeleteAccount(name);
                //else
                //    account = null;

                //message = account == null
                //    ? MultiLanguage.Dictionary["AccountNotFound"]
                //    : String.Format(MultiLanguage.Dictionary["AccountDeleted"], account.Name);
            }
            catch (DFMCoreException)// e)
            {
                //message = MultiLanguage.Dictionary[e];
            }


        }


        internal void Disable(String name)
        {
            Admin.DisableCategory(name);
            //else
            //    category = null;

            // TODO: implement messages on page head
            //var message = category == null
            //    ? MultiLanguage.Dictionary["CategoryNotFound"]
            //    : String.Format(MultiLanguage.Dictionary["CategoryDisabled"], category.Name);
        }



        internal void Enable(String name)
        {
            Admin.EnableCategory(name);
            //else
            //    category = null;

            // TODO: implement messages on page head
            //var message = category == null
            //    ? MultiLanguage.Dictionary["CategoryNotFound"]
            //    : String.Format(MultiLanguage.Dictionary["CategoryEnabled"], category.Name);
        }




    }
}