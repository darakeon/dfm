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
                var username = Authenticate.AuthenticatedUser();
                return UserData.SelectByLogin(username);
            }
        }

        public static Boolean IsAuthenticated
        {
            get { return User != null; }
        }

    }
}