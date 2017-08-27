using System;
using DFM.Entities;
using DFM.Entities.Extensions;
using DFM.Generic;

namespace DFM.Authentication
{
    public class Current
    {
        private ISafeService userService { get; set; }

        public Current(ISafeService userService)
        {
            this.userService = userService;
        }


        public String Ticket
        {
            get { return MyCookie.Get(); }
        }

        public User User
        {
            get
            {
                try
                {
                    return userService.GetUserByTicket(Ticket);
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


        public String Language
        {
            get { return User == null ? null : User.Language; }
        }

        
        public Boolean IsAdm
        {
            get { return IsAuthenticated && User.IsAdm(); }
        }



        public void Set(String username, String password)
        {
            userService.ValidateUserAndCreateTicket(username, password, Ticket);
        }

        public void Reset(String username, String password)
        {
            Clean();
            Set(username, password);
        }

        public void Clean()
        {
            userService.DisableTicket(Ticket);
        }



    }
}