using System;
using System.Collections.Generic;
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


        readonly static Identity<String> identity = new Identity<String>();


        public User User
        {
            get
            {
                if (!identity.Exists)
                    return null;

                try
                {
                    return userService.GetUserByTicket(identity.ID);
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
            var ticket = userService.ValidateUserAndCreateTicket(username, password);

            identity.Add(ticket);
        }

        public void Reset(String username, String password)
        {
            Clean();
            Set(username, password);
        }

        public void Clean()
        {
            if (!identity.Exists)
                return;

            userService.DisableTicket(identity.ID);

            identity.Kill();
        }


    }
}