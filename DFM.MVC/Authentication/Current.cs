using System;
using Ak.MVC.Authentication;
using DFM.Core.Database;
using DFM.Core.Entities;

namespace DFM.MVC.Authentication
{
    public class Current
    {
        public static User User
        {
            get
            {
                if (!Authenticate.IsAuthenticated())
                    return null;
                
                var username = Authenticate.GetUsername();
                return UserData.SelectByEmail(username);
            }
        }

        public static Boolean IsAuthenticated
        {
            get { return User != null; }
        }


        public static String Language {
            get { return User == null ? null : User.Language; }
        }
    }
}