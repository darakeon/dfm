using System;
using Ak.MVC.Authentication;
using DFM.Entities;
using DFM.Entities.Extensions;
using DFM.Repositories;

namespace DFM.MVC.Authentication
{
    public class Current
    {
        public static User User
        {
            get
            {
                if (!Authenticate.IsAuthenticated)
                    return null;
                
                var username = Authenticate.Username;
                return Services.Safe.SelectUserByEmail(username);
            }
        }

        public static Boolean IsAuthenticated
        {
            get { return User != null; }
        }


        public static String Language {
            get { return User == null ? null : User.Language; }
        }

        
        public static Boolean IsAdm
        {
            get { return IsAuthenticated && User.IsAdm(); }
        }

    }
}