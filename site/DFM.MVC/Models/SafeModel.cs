using System;
using DFM.BusinessLogic.Exceptions;
using DFM.Entities.Enums;

namespace DFM.MVC.Models
{
    public class SafeModel : BaseModel
    {
        internal Boolean Disable(String token)
        {
            try
            {
                Safe.DisableToken(token);
            }
            catch (DFMCoreException)
            {
                return false;
            }

            return true;
        }



        internal Boolean TestAndActivate(String token)
        {
            try
            {
                Safe.TestSecurityToken(token, SecurityAction.UserVerification);
            }
            catch (DFMCoreException)
            {
                return false;
            }

            Safe.ActivateUser(token);

            return true;
        }



        internal void LogOff()
        {
            Current.Clean();
        }

    }
}