using System;
using System.Linq;
using DFM.Entities.Enums;

namespace DFM.Entities.Extensions
{
    public static class UserExtension
    {
        public static Boolean IsAdm(this User user)
        {
            return user.Email == "[some admin e-mail here]";
        }

        public static Boolean HasPendentActivation(this User user)
        {
            return user.SecurityList
                .Any(s => s.Action == SecurityAction.UserVerification
                     && s.Expire >= DateTime.Now 
                     && s.Active);
        }

    }
}
