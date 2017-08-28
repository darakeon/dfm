using System;
using DK.MVC.Cookies;
using DFM.Entities;
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


        public PseudoTicket Ticket => MyCookie.Get();

	    public User User
        {
            get
            {
                try
                {
                    return userService.GetUserByTicket(Ticket.Key);
                }
                catch (DFMException)
                {
                    return null;
                }
            }
        }



        public Boolean IsAuthenticated => User != null;


	    public String Language => User == null ? null : User.Config.Language;


	    public Boolean IsAdm => IsAuthenticated && User.IsAdm();


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
            userService.DisableTicket(Ticket.Key);
        }
        


    }
}