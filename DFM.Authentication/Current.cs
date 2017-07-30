using System;
using DFM.Entities;
using DFM.Entities.Extensions;
using DFM.Generic;
using DFM.Generic.UniqueIdentity;

namespace DFM.Authentication
{
    public class Current
    {
        private ISafeService userService { get; set; }

        public Current(ISafeService userService)
        {
            this.userService = userService;
        }


        private const String keyName = "UserTicket";

        public User User
        {
            get
            {
                if (!Identity.Exists(keyName))
                    return null;

                try
                {
                    var ticket = Identity.GetKeyFor(keyName);

                    return userService.GetUserByTicket(ticket);
                }
                catch (DFMException)
                {
                    return null;
                }
            }
        }



        public Boolean IsAuthenticated
        {
            get { return User != null; }
        }


        public String Language {
            get { return User == null ? null : User.Language; }
        }

        
        public Boolean IsAdm
        {
            get { return IsAuthenticated && User.IsAdm(); }
        }



        public void Set(String username, String password)
        {
            var ticket = userService.ValidateUserAndGetTicket(username, password);

            Identity.SetKeyFor(keyName, ticket);
        }

        public void Reset(String username, String password)
        {
            Clean();
            Set(username, password);
        }

        public void Clean()
        {
            if (!Identity.Exists(keyName))
                return;

            var ticket = Identity.GetKeyFor(keyName);

            userService.DisableTicket(ticket);

            Identity.KillKeyFor(keyName);
        }


    }
}