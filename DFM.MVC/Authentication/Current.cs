using System;
using System.Web;
using Ak.MVC.Authentication;
using DFM.Core.Database;
using DFM.Core.Entities;

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