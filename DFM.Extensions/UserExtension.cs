using System;
using DFM.Entities;

namespace DFM.Extensions
{
    public static class UserExtension
    {
        public static Boolean IsAdm(this User user)
        {
            return user.Email == "[some admin e-mail here]";
        }
    }
}
