using System;
using Ak.MVC.Authentication;
using Ak.MVC.Route;
using DFM.Core.Database;
using DFM.Core.Entities;
using DFM.Core.Enums;

namespace DFM.MVC.Authentication
{
    public class Current
    {
        private static readonly UserData userData = new UserData();

        public static User User
        {
            get
            {
                var username = Authenticate.AuthenticatedUser();
                return userData.SelectByLogin(username);
            }
        }



        public static Boolean IsAuthenticated
        {
            get { return User != null; }
        }
    }
}