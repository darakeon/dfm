using System;
using Ak.MVC.Authentication;
using DFM.Core.Database;
using DFM.Core.Entities;

namespace DFM.MVC.Authentication
{
    public class Current
    {
        private static readonly UserData userData = new UserData();

        private static User user;

        public static User User
        {
            get
            {
                if (user == null)
                {
                    var username = Authenticate.AuthenticatedUser();
                    user = userData.SelectByLogin(username);
                }

                return user;
            }
        }



        public static Boolean IsAuthenticated
        {
            get { return User != null; }
        }
    }
}